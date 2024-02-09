using Common;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay
{
    public class BuildingManager : Singleton<BuildingManager>, IManager
    {
        [SerializeField] BuildingSpawnController spawnController = null;
        [SerializeField] BuildingPickController pickController = null;
        [SerializeField] BuildingPlaceController placeController = null;

        List<BuildingControllerBase> buildings = null;

        private GameBoardSelectController<BuildingControllerBase> selectController = null;

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

            spawnController.InitController();
            pickController.InitController();
            placeController.InitController();

            selectController = new GameBoardSelectController<BuildingControllerBase>();
            selectController.InitController();

            UnitManager.Instance.OnUnitPicked.AddListener(OnUnitPicked);
            UnitManager.Instance.OnUnitSelected.AddListener(OnUnitSelected);
        }

        public void PickBuilding(BuildingTypes buildingType) 
        {
            if (pickController.IsPickedBuilding)
                pickController.DropObject();

            BuildingControllerBase building = spawnController.SpawnBuildingForPicking(buildingType);

            if (building == null)
                return;

            pickController.PickObject(building);

            OnBuildingPicked?.Invoke();
        }

        public void DropBuilding() 
        {
            pickController.DropObject();
        }

        public void PlaceBuilding() 
        {
            if (!pickController.IsPickedBuilding)
                return;

            bool isPlacingSuccess = placeController.PlaceBuilding(pickController.PickedBuilding);

            if (isPlacingSuccess)
                pickController.DropObject();

        }

        public void SpawnBuilding(BuildingTypes buildingType, BoardCoordinate coordinate) 
        {
            BuildingControllerBase building = spawnController.SpawnBuilding(buildingType, coordinate);

            if (building == null)
                return;

            AddBuilding(building);
        }

        public void SelectBuilding(BoardCoordinate coordinate) 
        {
            bool isSelectionSuccess = selectController.SelectObject(coordinate);

            if (isSelectionSuccess)
                OnBuildingSelected?.Invoke();
        }

        public void DeselectBuilding() 
        {
            selectController.DeselectObject();
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

        private void OnUnitSelected() 
        {
            DeselectBuilding();
        }
    }
}