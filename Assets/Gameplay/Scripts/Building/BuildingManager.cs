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
        public UnityEvent OnBuildingPicked;

        [HideInInspector]
        public UnityEvent OnBuildingSelected;

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

            UnitManager.Instance.OnUnitPicked.AddListener(OnUnitPicked);
            UnitManager.Instance.OnUnitSelected.AddListener(OnUnitSelected);
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
                    return true;
                }

                if (!isCoordinatePlaceable)
                    return true;

                isPlaceSuccess = PlaceBuilding();

                if (isPlaceSuccess)
                    return true;

                return false;
            }

            if (selectController.IsSelectedObject)
            {
                if (!isCoordinateInBoardBounds)
                {
                    DeselectBuilding();
                    return true;
                }

                if (isCoordinatePlaceable)
                {
                    DeselectBuilding();
                    return true;
                }

                BuildingController selectedBuilding = selectController.GetSelectedObject();
                IPlaceable inputPlaceable = GameBoardManager.Instance.GetPlacedObject(coordinate);

                if (selectedBuilding.IsEqual(inputPlaceable) && selectedBuilding.IsCoordinateInBounds(coordinate))
                    return false;

                isSelectSuccess = SelectBuilding(coordinate);

                if (isSelectSuccess)
                    return true;

                return false;
            }

            if (!isCoordinateInBoardBounds)
                return false;

            if (isCoordinatePlaceable)
                return false;

            isSelectSuccess = SelectBuilding(coordinate);

            if (isSelectSuccess)
                return true;

            return false;
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
                ShowSelectedBuildingInformation();
            }

            return isSelectionSuccess;
        }

        public void DeselectBuilding() 
        {
            selectController.DeselectObject();
            HideSelectedBuildingInformation();
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

        private void ShowSelectedBuildingInformation() 
        {
            BuildingViewModel viewModel = selectController.GetSelectedObject().ViewModel;

            if (viewModel == null)
                return;

            GameUIController.Instance.ShowBuildingInformationPanel(viewModel.Name
                , viewModel.SpriteBuilding
                , viewModel.BuildingColor);

            if (viewModel.IsProduceUnits)
                UnitManager.Instance.ShowProducibleUnitInformation(viewModel.GetProducibleUnits());

        }

        private void HideSelectedBuildingInformation() 
        {
            GameUIController.Instance.HideBuildingInformationPanel();
            GameUIController.Instance.HideProducibleUnitInformationPanel();
        }
    }
}