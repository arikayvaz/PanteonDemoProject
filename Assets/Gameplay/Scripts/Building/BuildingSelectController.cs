using Common;
using Unity.VisualScripting;
using UnityEngine;

namespace Gameplay
{
    public class BuildingSelectController : MonoBehaviour, IController
    {
        public bool IsBuildingSelected => selectedBuilding != null;

        private BuildingControllerBase selectedBuilding = null;

        public void InitController() 
        {
            selectedBuilding = null;
        }

        public bool SelectBuilding(BoardCoordinate coordinate) 
        {
            IPlaceable placedObject = GameBoardManager.Instance.GetPlacedObject(coordinate);

            if (placedObject == null)
                return false;

            BuildingControllerBase building = placedObject as BuildingControllerBase;

            if (building == null)
                return false;

            if (!building.CanSelect() || building == selectedBuilding)
                return false;

            if (IsBuildingSelected)
                DeselectBuilding();

            selectedBuilding = building;
            selectedBuilding.Select();

            return true;
        }

        public void DeselectBuilding() 
        {
            if (!IsBuildingSelected)
                return;

            selectedBuilding.Deselect();
            selectedBuilding = null;
        }
    }
}