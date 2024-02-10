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

        public BuildingViewModel(BuildingModel model)
        {
            this.model = model;
        }

        public void UpdateCoordinate(BoardCoordinate coordinate) 
        {
            model.coordinate = coordinate;
        }

    }
}