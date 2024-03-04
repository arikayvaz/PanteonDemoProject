using Common;
using UnityEngine;

namespace Gameplay
{
    public class UnitPlaceController : MonoBehaviour, IController
    {
        public void InitController()
        {
            
        }

        public bool PlaceUnit(IPlaceable placeable, BoardCoordinate placeCoordinate)
        {
            if (!GameBoardManager.Instance.IsCoordinatePlaceable(placeCoordinate))
                return false;

            if (!GameBoardManager.Instance.IsCoordinatesPlaceable(placeable.GetPlaceCoordinates(placeCoordinate)))
                return false;

            placeable.Place(placeCoordinate);
            GameBoardManager.Instance.OnUnitPlaced(placeable, placeable.GetPlaceCoordinates());
            return true;
        }
    }
}