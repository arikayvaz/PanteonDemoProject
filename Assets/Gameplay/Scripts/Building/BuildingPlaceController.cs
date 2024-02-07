using UnityEngine;
using Common;

namespace Gameplay
{
    public class BuildingPlaceController : MonoBehaviour, IController
    {
        public void InitController()
        {
            
        }

        public bool PlaceBuilding(BuildingControllerBase building) 
        {
            Vector2 inputWorldPos = InputManager.Instance.WorldPosition;
            GameBoardCoordinates placeCoord = GameBoardManager.Instance.GetCoordinateFromWorldPosition(inputWorldPos);

            if (!GameBoardManager.Instance.IsCoordinatePlaceable(placeCoord))
                return false;

            if (!GameBoardManager.Instance.IsCoordinatesPlaceable(building.GetPlaceCoordinates(placeCoord)))
                return false;

            building.Place(placeCoord);
            return true;
        }
    }
}