using Common;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay
{
    public class BuildingManager : Singleton<BuildingManager>, IManager
    {
        [SerializeField] BuildingSpawnController buildingSpawner = null;
        [SerializeField] BuildingPickController buildingPicker = null;
        [SerializeField] BuildingPlaceController buildingPlacer = null;
        [SerializeField] BuildingSelectController buildingSelection = null;

        List<BuildingControllerBase> buildings = null;

        [HideInInspector]
        public UnityEvent OnBuildingPicked;

        [HideInInspector]
        public UnityEvent OnBuildingSelected;

        private void Start()
        {
            InitManager();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            OnBuildingPicked?.RemoveAllListeners();
        }

        private void Update()
        {
#warning remove test codes
            if (Input.GetKeyDown(KeyCode.A))
            {
                PickBuilding(BuildingTypes.Barracks);
                return;
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                PickBuilding(BuildingTypes.PowerPlant);
                return;
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                DropBuilding();
                return;
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                PlaceBuilding();
                return;
            }

            if (Input.GetKeyDown(KeyCode.Z))
            {
                SelectBuilding(InputManager.Instance.CurrentInputCoordinate);
                return;
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                DeselectBuilding();
                return;
            }
        }

        public void InitManager()
        {
            buildings = new List<BuildingControllerBase>();

            buildingSpawner.InitController();
            buildingPicker.InitController();
            buildingPlacer.InitController();

            UnitManager.Instance.OnUnitPicked.AddListener(OnUnitPicked);
        }

        public void PickBuilding(BuildingTypes buildingType) 
        {
            if (buildingPicker.IsPickedBuilding)
                buildingPicker.DropObject();

            BuildingControllerBase building = buildingSpawner.SpawnBuildingForPicking(buildingType);

            if (building == null)
                return;

            buildingPicker.PickObject(building);

            OnBuildingPicked?.Invoke();
        }

        public void DropBuilding() 
        {
            buildingPicker.DropObject();
        }

        public void PlaceBuilding() 
        {
            if (!buildingPicker.IsPickedBuilding)
                return;

            bool isPlacingSuccess = buildingPlacer.PlaceBuilding(buildingPicker.PickedBuilding);

            if (isPlacingSuccess)
                buildingPicker.DropObject();

        }

        public void SpawnBuilding(BuildingTypes buildingType, BoardCoordinate coordinate) 
        {
            BuildingControllerBase building = buildingSpawner.SpawnBuilding(buildingType, coordinate);

            if (building == null)
                return;

            AddBuilding(building);
        }

        public void SelectBuilding(BoardCoordinate coordinate) 
        {
            if (buildingSelection.IsBuildingSelected)
                buildingSelection.DeselectBuilding();

            bool isSelectionSuccess = buildingSelection.SelectBuilding(coordinate);

            if (isSelectionSuccess)
                OnBuildingSelected?.Invoke();
        }

        public void DeselectBuilding() 
        {
            buildingSelection.DeselectBuilding();
        }

        private void AddBuilding(BuildingControllerBase building) 
        {
            if (buildings.Contains(building))
                return;

            buildings.Add(building);
        }

        private void DeleteBuilding(BuildingControllerBase building) 
        {
            if (buildings.Count < 1 || !buildings.Contains(building))
                return;

            buildings.Remove(building);
        }

        private void OnUnitPicked() 
        {
            DropBuilding();
        }
    }
}