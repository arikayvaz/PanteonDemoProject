using Common;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class BuildingManager : Singleton<BuildingManager>, IManager
    {
        [SerializeField] BuildingSpawnController buildingSpawner = null;
        [SerializeField] BuildingPickController buildingPicker = null;
        [SerializeField] BuildingPlaceController buildingPlacer = null;

        List<BuildingControllerBase> buildings = null;

        protected override void Awake()
        {
            base.Awake();

            InitManager();
        }

        private void Update()
        {
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
        }

        public void InitManager()
        {
            buildings = new List<BuildingControllerBase>();

            buildingSpawner.InitController();
            buildingPicker.InitController();
            buildingPlacer.InitController();
        }

        public void PickBuilding(BuildingTypes buildingType) 
        {
            BuildingControllerBase building = buildingSpawner.SpawnBuildingForPicking(buildingType);
            buildingPicker.PickObject(building);
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

        public void SpawnBuilding(BuildingTypes buildingType, GameBoardCoordinates coordinates) 
        {
            BuildingControllerBase building = buildingSpawner.SpawnBuilding(buildingType, coordinates);

            if (building == null)
                return;

            AddBuilding(building);
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
    }
}