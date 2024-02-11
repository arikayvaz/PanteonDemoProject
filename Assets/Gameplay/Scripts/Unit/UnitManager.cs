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

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                PickUnit(UnitTypes.Soldier00);
                return;
            }

            if (Input.GetKeyDown(KeyCode.T))
            {
                PickUnit(UnitTypes.Soldier01);
                return;
            }

            if (Input.GetKeyDown(KeyCode.Y))
            {
                PickUnit(UnitTypes.Soldier02);
                return;
            }


            if (Input.GetKeyDown(KeyCode.G))
            {
                DropUnit();
                return;
            }

            if (Input.GetKeyDown(KeyCode.H))
            {
                PlaceUnit();
                return;
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                SelectUnit(InputManager.Instance.CurrentInputCoordinate);
                return;
            }

            if (Input.GetKeyDown(KeyCode.V))
            {
                DeselectUnit();
                return;
            }

            if (Input.GetMouseButtonDown(1))
            {
                HandleUnitCommand(InputManager.Instance.CurrentInputCoordinate);
                return;
            }
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
        public void PickUnit(UnitTypes unitType)
        {
            if (pickController.IsPickedUnit)
                pickController.DropObject();

            UnitDataSO unitData = GetUnitData(unitType);

            if (unitData == null)
                return;

            UnitModel model = new UnitModel(unitData);

            UnitController unit = spawnController.SpawnUnitForPicking(model);

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

        public void PlaceUnit()
        {
            if (!pickController.IsPickedUnit)
                return;

            bool isPlacingSuccess = placeController.PlaceUnit(pickController.PickedUnit);

            if (isPlacingSuccess)
                pickController.DropObject();

        }

        #endregion

        #region Selecting

        public void SelectUnit(BoardCoordinate coordinate) 
        {
            bool isSelectSuccess = selectController.SelectObject(coordinate);

            if (isSelectSuccess)
                OnUnitSelected?.Invoke();
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

        private void HandleUnitCommand(BoardCoordinate coordinate) 
        {
            if (!selectController.IsSelectedObject)
                return;

            if (!GameBoardManager.Instance.IsCoordinateInBoardBounds(coordinate))
                return;

            bool isAttackingSuccess = Attack(coordinate);

            if (isAttackingSuccess)
                return;

            MoveToCoordinate(coordinate);
        }

        private bool Attack(BoardCoordinate coordinate) 
        {
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
    }
}