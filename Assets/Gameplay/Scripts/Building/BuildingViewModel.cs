using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class BuildingViewModel
    {
        private BuildingModel model = null;

        public string Name => model.Name;
        public BuildingTypes BuildingType => model.BuildingType;
        public int CellSizeX => model.CellSizeX;
        public int CellSizeY => model.CellSizeY;
        public BoardCoordinate Coordinate => model.coordinate;
        public Color BuildingColor => model.BuildingColor;
        public Color ColorSelected => model.StateColorData.ColorSelected;
        public Color ColorPlaceable => model.StateColorData.ColorPlaceable;
        public Color ColorUnplaceable => model.StateColorData.ColorUnPlaceable;
        public Sprite SpriteBuilding => model.SpriteBuilding;

        public bool IsProduceUnits => model.IsProduceUnits;
        public BoardCoordinate SpawnPointCoordinate => model.SpawnPointCoordinate;
        public BoardCoordinate SpawnPointOffsetCoordinate => model.SpawnPointOffsetCoordinate;

        public int Health => model.health;

        public BuildingViewModel(BuildingModel model)
        {
            this.model = model;
        }

        public void UpdateCoordinate(BoardCoordinate coordinate) 
        {
            model.coordinate = coordinate;
        }

        public IEnumerable<UnitTypes> GetProducibleUnits() 
        {
            if (!IsProduceUnits)
                yield return UnitTypes.None;

            foreach (UnitTypes unitType in model.ProducibleUnits)
            {
                yield return unitType;
            }

        }

        public void SetHealth(int health) 
        {
            model.health = health;
        }

        public void AddHealth(int healthDelta) 
        {
            model.health += healthDelta;

            if (model.health < 0)
                model.health = 0;
        }

    }
}