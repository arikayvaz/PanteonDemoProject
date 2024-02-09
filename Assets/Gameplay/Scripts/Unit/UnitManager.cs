using Common;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay
{
    public class UnitManager : Singleton<UnitManager>, IManager
    {
        [SerializeField] UnitSpawnController unitSpawner = null;
        [SerializeField] UnitPickController unitPicker = null;
        [SerializeField] UnitPlaceController unitPlacer = null;

        [HideInInspector]
        public UnityEvent OnUnitPicked;

        List<UnitControllerBase> units = null;

        private void Start()
        {
            InitManager();
        }

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
        }

        public void InitManager()
        {
            units = new List<UnitControllerBase>();

            unitSpawner.InitController();
            unitPicker.InitController();
            unitPlacer.InitController();

            BuildingManager.Instance.OnBuildingPicked.AddListener(OnBuildingPicked);
        }

        public void SpawnUnit(UnitTypes unitType, BoardCoordinate coordinate) 
        {
            UnitControllerBase unit = unitSpawner.SpawnUnit(unitType, coordinate);

            if (unit == null)
                return;

            AddUnit(unit);
        }

        public void PickUnit(UnitTypes unitType)
        {
            if (unitPicker.IsPickedUnit)
                unitPicker.DropObject();

            UnitControllerBase unit = unitSpawner.SpawnUnitForPicking(unitType);

            if (unit == null)
                return;

            unitPicker.PickObject(unit);

            OnUnitPicked?.Invoke();
        }

        public void DropUnit()
        {
            unitPicker.DropObject();
        }

        public void PlaceUnit()
        {
            if (!unitPicker.IsPickedUnit)
                return;

            bool isPlacingSuccess = unitPlacer.PlaceUnit(unitPicker.PickedUnit);

            if (isPlacingSuccess)
                unitPicker.DropObject();

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

        private void OnBuildingPicked() 
        {
            DropUnit();
        }
    }
}