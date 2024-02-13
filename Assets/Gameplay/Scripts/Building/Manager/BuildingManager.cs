using Common;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay
{
    public class BuildingManager : Singleton<BuildingManager>, IManager
    {
        [HideInInspector]
        public UnityEvent OnBuildingPicked { get; private set; } = null;

        [HideInInspector]
        public UnityEvent OnBuildingSelected { get; private set; } = null;

        [SerializeField] BuildingDataSO[] buildingDatas = null;

        [Space]
        [SerializeField] BuildingSpawnController spawnController = null;
        [SerializeField] BuildingPickController pickController = null;
        [SerializeField] BuildingPlaceController placeController = null;

        private GameBoardSelectController<BuildingController> selectController = null;

        List<BuildingController> buildings = null;

        protected override void OnDestroy()
        {
            base.OnDestroy();

            OnBuildingPicked?.RemoveAllListeners();
        }

        public void InitManager()
        {
            buildings = new List<BuildingController>();

            spawnController.InitController();
            pickController.InitController();
            placeController.InitController();

            selectController = new GameBoardSelectController<BuildingController>();
            selectController.InitController();

            OnBuildingPicked = new UnityEvent();
            OnBuildingSelected = new UnityEvent();
        }

        public void SubscribeEvents() 
        {
            UnitManager.Instance.OnUnitPicked.AddListener(OnUnitPicked);
            UnitManager.Instance.OnUnitSelected.AddListener(OnUnitSelected);
            GameUIController.Instance.OnBuildingProduced.AddListener(OnBuildingProduced);
        }

        public bool HandleLeftClickInput(BoardCoordinate coordinate)
        {
            bool isCoordinateInBoardBounds = GameBoardManager.Instance.IsCoordinateInBoardBounds(coordinate);
            bool isCoordinatePlaceable = GameBoardManager.Instance.IsCoordinatePlaceable(coordinate);
            bool isPlaceSuccess = false;
            bool isSelectSuccess = false;

            if (pickController.IsPickedBuilding)
            {
                if (!isCoordinateInBoardBounds) 
                {
                    DropBuilding();
                    return false;
                }

                if (!isCoordinatePlaceable)
                    return false;

                isPlaceSuccess = PlaceBuilding();

                return isPlaceSuccess;
            }

            if (selectController.IsSelectedObject)
            {
                if (!isCoordinateInBoardBounds)
                {
                    DeselectBuilding();
                    return false;
                }

                if (isCoordinatePlaceable)
                {
                    DeselectBuilding();
                    return false;
                }

                BuildingController selectedBuilding = selectController.GetSelectedObject();
                IPlaceable inputPlaceable = GameBoardManager.Instance.GetPlacedObject(coordinate);

                if (selectedBuilding.IsEqual(inputPlaceable) && selectedBuilding.IsCoordinateInBounds(coordinate))
                    return false;

                if (!IsPlaceableValid(inputPlaceable))
                    return false;

                isSelectSuccess = SelectBuilding(coordinate);

                return isSelectSuccess;
            }

            if (!isCoordinateInBoardBounds || isCoordinatePlaceable)
                return false;

            IPlaceable placeable = GameBoardManager.Instance.GetPlacedObject(coordinate);

            if (!IsPlaceableValid(placeable))
                return false;

            isSelectSuccess = SelectBuilding(coordinate);

            return isSelectSuccess;
        }

        public BoardCoordinate GetSelectedBuildingSpawnCoordinate() 
        {
            if (!selectController.IsSelectedObject)
                return BoardCoordinate.Invalid;

            return selectController.GetSelectedObject().SpawnPointCoordinate;
        }

        #region Pick

        public void PickBuilding(BuildingTypes buildingType) 
        {
            if (pickController.IsPickedBuilding)
                pickController.DropObject();

            BuildingDataSO buildingData = GetBuildingData(buildingType);

            if (buildingData == null)
                return;

            BuildingModel model = new BuildingModel(buildingData);

            BuildingController building = spawnController.SpawnBuildingForPicking(model);

            if (building == null)
                return;

            pickController.PickObject(building);

            if (selectController.IsSelectedObject)
                DeselectBuilding();

            OnBuildingPicked?.Invoke();
        }

        public void DropBuilding() 
        {
            pickController.DropObject();
        }

        #endregion

        #region Place

        public bool PlaceBuilding() 
        {
            bool isPlacingSuccess = placeController.PlaceBuilding(pickController.PickedBuilding);

            if (isPlacingSuccess)
                pickController.DropObject();

            return isPlacingSuccess;
        }

        public void RemoveBuilding(BuildingController building) 
        {
            DeleteBuilding(building);

            GameBoardManager.Instance.OnBuildingDestroyed(building, building.GetPlaceCoordinates());
        }

        #endregion

        #region Spawn

        public void SpawnBuilding(BuildingModel model, BoardCoordinate coordinate) 
        {
            BuildingController building = spawnController.SpawnBuilding(model, coordinate);

            if (building == null)
                return;

            AddBuilding(building);
        }

        #endregion

        #region Select

        public bool SelectBuilding(BoardCoordinate coordinate) 
        {
            bool isSelectionSuccess = selectController.SelectObject(coordinate);

            if (isSelectionSuccess) 
            {
                OnBuildingSelected?.Invoke();
            }

            return isSelectionSuccess;
        }

        public void DeselectBuilding() 
        {
            selectController.DeselectObject();
        }

        #endregion

        #region Add & Delete
        private void AddBuilding(BuildingController building) 
        {
            if (buildings.Contains(building))
                return;

            buildings.Add(building);
        }

        private void DeleteBuilding(BuildingController building) 
        {
            if (buildings.Count < 1 || !buildings.Contains(building))
                return;

            buildings.Remove(building);
        }

        #endregion

        public IEnumerable<BuildingDataSO> GetBuildingDatas() 
        {
            if (buildingDatas?.Length < 1)
                yield return null;

            foreach (BuildingDataSO data in buildingDatas)
                yield return data;

        }

        private void OnUnitPicked() 
        {
            DropBuilding();
        }

        private void OnUnitSelected() 
        {
            DeselectBuilding();
        }
        
        private BuildingDataSO GetBuildingData(BuildingTypes buildingType) 
        {
            if (buildingDatas?.Length < 1)
                return null;

            foreach (BuildingDataSO data in buildingDatas)
            {
                if (data.BuildingType == buildingType)
                    return data;
            }

            return null;
        }

        private bool IsPlaceableValid(IPlaceable placeable) 
        {
            if (placeable == null)
                return false;

            return (placeable as BuildingController) != null;
        }

        private void OnBuildingProduced(BuildingTypes buildingType) 
        {
            if (selectController.IsSelectedObject)
                selectController.DeselectObject();
        }
    }
}