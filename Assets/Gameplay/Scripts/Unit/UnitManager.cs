using Common;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay
{
    public class UnitManager : Singleton<UnitManager>, IManager
    {
        [SerializeField] UnitSpawnController spawnController = null;
        [SerializeField] UnitPickController pickController = null;
        [SerializeField] UnitPlaceController placeController = null;

        [HideInInspector]
        public UnityEvent OnUnitPicked;

        [HideInInspector]
        public UnityEvent OnUnitSelected;

        List<UnitControllerBase> units = null;

        private GameBoardSelectController<UnitControllerBase> selectController = null;

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
            units = new List<UnitControllerBase>();

            spawnController.InitController();
            pickController.InitController();
            placeController.InitController();

            selectController = new GameBoardSelectController<UnitControllerBase>();
            selectController.InitController();

            BuildingManager.Instance.OnBuildingPicked.AddListener(OnBuildingPicked);
            BuildingManager.Instance.OnBuildingSelected.AddListener(OnBuildingSelected);
        }

        public void SpawnUnit(UnitTypes unitType, BoardCoordinate coordinate) 
        {
            UnitControllerBase unit = spawnController.SpawnUnit(unitType, coordinate);

            if (unit == null)
                return;

            AddUnit(unit);
        }

        public void PickUnit(UnitTypes unitType)
        {
            if (pickController.IsPickedUnit)
                pickController.DropObject();

            UnitControllerBase unit = spawnController.SpawnUnitForPicking(unitType);

            if (unit == null)
                return;

            pickController.PickObject(unit);

            OnUnitPicked?.Invoke();
        }

        public void DropUnit()
        {
            pickController.DropObject();
        }

        public void PlaceUnit()
        {
            if (!pickController.IsPickedUnit)
                return;

            bool isPlacingSuccess = placeController.PlaceUnit(pickController.PickedUnit);

            if (isPlacingSuccess)
                pickController.DropObject();

        }

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

        public void AddUnit(UnitControllerBase unit) 
        {
            if (units.Contains(unit))
                return;

            units.Add(unit);
        }

        public void DeleteUnit(UnitControllerBase unit) 
        {
            if (units.Count < 1 || !units.Contains(unit))
                return;

            units.Remove(unit);
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
            UnitControllerBase selectedUnit = selectController.GetSelectedObject();

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
    }
}