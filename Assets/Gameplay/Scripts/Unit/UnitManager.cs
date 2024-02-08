using Common;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class UnitManager : Singleton<UnitManager>, IManager
    {
        [SerializeField] UnitSpawnController unitSpawner = null;
        [SerializeField] UnitPickController unitPicker = null;
        [SerializeField] UnitPlaceController unitPlacer = null;

        List<UnitControllerBase> units = null;

        protected override void Awake()
        {
            base.Awake();

            InitManager();
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
            unitPicker.PickObject(unit);
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
    }
}