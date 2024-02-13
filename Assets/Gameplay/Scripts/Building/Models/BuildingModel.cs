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
        public Sprite SpriteBuilding => buildingData.SpriteBuilding;
        public UnitTypes[] ProducibleUnits => buildingData.producibleUnits;
        public bool IsProduceUnits => buildingData.IsProduceUnits;

        public BoardCoordinate SpawnPointCoordinate => coordinate + SpawnPointOffsetCoordinate;
        public BoardCoordinate SpawnPointOffsetCoordinate => new BoardCoordinate(buildingData.SpawnPointCoordinateOffsetX, buildingData.SpawnPointCoordinateOffsetY);

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