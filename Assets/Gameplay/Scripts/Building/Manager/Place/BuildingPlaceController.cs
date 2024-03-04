using UnityEngine;
using Common;

namespace Gameplay
{
    public class BuildingPlaceController : MonoBehaviour, IController
    {
        public void InitController()
        {
            
        }

        public bool PlaceBuilding(IPlaceable placeable) 
        {
            Vector2 inputWorldPos = InputManager.Instance.WorldPosition;
            BoardCoordinate placeCoord = GameBoardManager.Instance.GetCoordinateFromWorldPosition(inputWorldPos);

            if (!GameBoardManager.Instance.IsCoordinatePlaceable(placeCoord))
                return false;

            if (!GameBoardManager.Instance.IsCoordinatesPlaceable(placeable.GetPlaceCoordinates(placeCoord, true)))
                return false;

            placeable.Place(placeCoord);
            GameBoardManager.Instance.OnBuildingPlaced(placeable, placeable.GetPlaceCoordinates(false));
            return true;
        }
    }
}