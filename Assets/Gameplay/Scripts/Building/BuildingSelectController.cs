using Common;
using UnityEngine;

namespace Gameplay
{
    public class BuildingSelectController : MonoBehaviour, IController
    {
        public bool IsBuildingSelected => selectedBuilding != null;

        private ISelectable selectedBuilding = null;

        public void InitController() 
        {
            selectedBuilding = null;
        }

        public bool SelectBuilding(BoardCoordinate coordinate) 
        {
            IPlaceable placedObject = GameBoardManager.Instance.GetPlacedObject(coordinate);

            if (placedObject == null)
                return false;

            selectedBuilding = placedObject as ISelectable;

            selectedBuilding?.Select();
            return selectedBuilding != null;
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