using Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay
{
    public class UnitManager : Singleton<UnitManager>, IManager
    {
        [SerializeField] UnitDataSO[] unitDatas = null;

        [Space]
        [SerializeField] UnitSpawnController spawnController = null;
        [SerializeField] UnitPickController pickController = null;
        [SerializeField] UnitPlaceController placeController = null;

        [HideInInspector]
        public UnityEvent OnUnitPicked;

        [HideInInspector]
        public UnityEvent OnUnitSelected;

        List<UnitController> units = null;

        private GameBoardSelectController<UnitController> selectController = null;

        protected override void OnDestroy()
        {
            base.OnDestroy();

            OnUnitPicked?.RemoveAllListeners();
        }

        public void InitManager()
        {
            units = new List<UnitController>();

            spawnController.InitController();
            pickController.InitController();
            placeController.InitController();

            selectController = new GameBoardSelectController<UnitController>();
            selectController.InitController();

            BuildingManager.Instance.OnBuildingPicked.AddListener(OnBuildingPicked);
            BuildingManager.Instance.OnBuildingSelected.AddListener(OnBuildingSelected);
        }

        public bool HandleLeftClickInput(BoardCoordinate coordinate) 
        {
            bool isCoordinateInBoardBounds = GameBoardManager.Instance.IsCoordinateInBoardBounds(coordinate);
            bool isCoordinatePlaceable = GameBoardManager.Instance.IsCoordinatePlaceable(coordinate);
            IPlaceable placeable = null;

            if (selectController.IsSelectedObject)
            {
                if (!isCoordinateInBoardBounds)
                {
                    DeselectUnit();
                    return false;
                }

                if (!isCoordinatePlaceable)
                {
                    placeable = GameBoardManager.Instance.GetPlacedObject(coordinate);

                    if (!IsPlaceableValid(placeable)) 
                    {
                        DeselectUnit();
                        return false;
                    }
                    else
                    {
                        DeselectUnit();
                        return SelectUnit(coordinate);
                    }
                }

                DeselectUnit();
                return false;
            }

            if (!isCoordinateInBoardBounds || isCoordinatePlaceable)
                return false;

            placeable = GameBoardManager.Instance.GetPlacedObject(coordinate);

            if (!IsPlaceableValid(placeable))
                return false;

            bool isSelectSuccess = SelectUnit(coordinate);

            return isSelectSuccess;
        }

        public bool HandleRightClickInput(BoardCoordinate coordinate) 
        {
            if (!selectController.IsSelectedObject)
                return false;

            bool isCoordinateInBoardBounds = GameBoardManager.Instance.IsCoordinateInBoardBounds(coordinate);
            bool isCoordinatePlaceable = GameBoardManager.Instance.IsCoordinatePlaceable(coordinate);

            if (!isCoordinateInBoardBounds)
                return false;

            if (isCoordinatePlaceable)
            {
                MoveToCoordinate(coordinate);
                return true;
            }
            else
            {
                return Attack(coordinate);
            }
        }

        public void ProduceUnit(UnitTypes unitType, BoardCoordinate coordinate) 
        {
            //PickUnit(unitType, coordinate);
            PlaceUnit(unitType, coordinate);
        }

        #region Spawning

        public void SpawnUnit(UnitTypes unitType, BoardCoordinate coordinate) 
        {
            UnitDataSO unitData = GetUnitData(unitType);

            if (unitData == null)
                return;

            UnitModel unitModel = new UnitModel(unitData);

            UnitController unit = spawnController.SpawnUnit(unitModel, coordinate);

            if (unit == null)
                return;

            AddUnit(unit);
        }

        #endregion

        #region Picking
        public void PickUnit(UnitTypes unitType, BoardCoordinate coordinate)
        {
            if (pickController.IsPickedUnit)
                pickController.DropObject();

            UnitDataSO unitData = GetUnitData(unitType);

            if (unitData == null)
                return;

            UnitModel model = new UnitModel(unitData);

            UnitController unit = spawnController.SpawnUnit(model, coordinate);

            if (unit == null)
                return;

            pickController.PickObject(unit);

            OnUnitPicked?.Invoke();
        }

        public void DropUnit()
        {
            pickController.DropObject();
        }

        #endregion

        #region Placing

        public void PlaceUnit(UnitTypes unitType, BoardCoordinate placeCoordinate)
        {
            /*
            if (!pickController.IsPickedUnit)
                return;
            */

            if (!GameBoardManager.Instance.IsCoordinatePlaceable(placeCoordinate))
                return;

            UnitDataSO unitData = GetUnitData(unitType);

            if (unitData == null)
                return;

            UnitModel model = new UnitModel(unitData);

            UnitController unit = spawnController.SpawnUnit(model, placeCoordinate);

            if (unit == null)
                return;

            placeController.PlaceUnit(unit, placeCoordinate);

            /*
            if (isPlacingSuccess)
                pickController.DropObject();
            */

        }

        #endregion

        #region Selecting

        public bool SelectUnit(BoardCoordinate coordinate) 
        {
            bool isSelectSuccess = selectController.SelectObject(coordinate);

            if (isSelectSuccess)
                OnUnitSelected?.Invoke();

            return isSelectSuccess;
        }

        public void DeselectUnit() 
        {
            selectController.DeselectObject();
        }

        #endregion

        public void AddUnit(UnitController unit) 
        {
            if (units.Contains(unit))
                return;

            units.Add(unit);
        }

        public void RemoveUnit(UnitController unit) 
        {
            DeleteUnit(unit);

            GameBoardManager.Instance.OnUnitDestroyed(unit, unit.GetPlaceCoordinates());
        }

        public void DeleteUnit(UnitController unit) 
        {
            if (units.Count < 1 || !units.Contains(unit))
                return;

            units.Remove(unit);
        }

        public void ShowProducibleUnitInformation(IEnumerable<UnitTypes> producibleUnits) 
        {
            if (producibleUnits == null)
                return;

            IEnumerable<UnitDataSO> unitDatas = GetUnitDatas(producibleUnits);

            if (unitDatas == null)
                return;

            GameUIController.Instance.ShowProducibleUnitInformationPanel(unitDatas);
        }

        private bool Attack(BoardCoordinate coordinate) 
        {
            IAttacker attackUnit = selectController.GetSelectedObject() as IAttacker;

            if (attackUnit == null)
                return false;

            IDamageable attackTarget = GameBoardManager.Instance.GetPlacedObject(coordinate) as IDamageable;

            if (attackTarget == null || !attackTarget.IsAlive())
                return false;

            attackUnit.Attack(coordinate, attackTarget);

            return false;
        }

        private void MoveToCoordinate(BoardCoordinate coordinate) 
        {
            UnitController selectedUnit = selectController.GetSelectedObject();

            if (selectedUnit == null)
                return;

            selectController.DeselectObject();
            selectedUnit.Move(coordinate);
        }

        private void OnBuildingPicked() 
        {
            DropUnit();
        }

        private void OnBuildingSelected() 
        {
            DeselectUnit();
        }

        private UnitDataSO GetUnitData(UnitTypes unitType) 
        {
            if (unitDatas?.Length < 1)
                return null;

            foreach (UnitDataSO data in unitDatas)
            {
                if (data.UnitType == unitType)
                    return data;
            }

            return null;
        }

        private IEnumerable<UnitDataSO> GetUnitDatas(IEnumerable producibleUnits) 
        {
            foreach (UnitTypes unitType in producibleUnits)
            {
                UnitDataSO data = GetUnitData(unitType);

                if (data == null)
                    continue;

                yield return data;
            }
        }

        private bool IsPlaceableValid(IPlaceable placeable) 
        {
            if (placeable == null)
                return false;

            return (placeable as UnitController) != null;
        }
    }
}