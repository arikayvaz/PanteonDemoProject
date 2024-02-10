using UnityEngine;

namespace Gameplay
{
    public class BuildingModel
    {
        public BuildingTypes BuildingType => buildingData.BuildingType;
        public string Name => buildingData.BuildingName;
        public int CellSizeX => buildingData.CellSizeX;
        public int CellSizeY => buildingData.CellSizeY;
        public Color BuildingColor => buildingData.BuildingColor;
        public StateColorDataSO StateColorData => buildingData.StateColorData;

        public int health = 0;
        public BoardCoordinate coordinate = BoardCoordinate.Invalid;

        private BuildingDataSO buildingData;

        public BuildingModel(BuildingDataSO buildingData)
        {
            this.buildingData = buildingData;

            health = buildingData.Health;
        }
    }
}