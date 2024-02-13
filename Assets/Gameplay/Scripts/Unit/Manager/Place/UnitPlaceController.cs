using Common;
using UnityEngine;

namespace Gameplay
{
    public class UnitPlaceController : MonoBehaviour, IController
    {
        public void InitController()
        {
            
        }

        public bool PlaceUnit(UnitController unit, BoardCoordinate placeCoordinate)
        {
            if (!GameBoardManager.Instance.IsCoordinatePlaceable(placeCoordinate))
                return false;

            if (!GameBoardManager.Instance.IsCoordinatesPlaceable(unit.GetPlaceCoordinates(placeCoordinate)))
                return false;

            unit.Place(placeCoordinate);
            GameBoardManager.Instance.OnUnitPlaced(unit, unit.GetPlaceCoordinates());
            return true;
        }
    }
}