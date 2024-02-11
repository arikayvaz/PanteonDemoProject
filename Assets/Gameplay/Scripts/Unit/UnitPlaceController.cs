using Common;
using UnityEngine;

namespace Gameplay
{
    public class UnitPlaceController : MonoBehaviour, IController
    {
        public void InitController()
        {
            
        }

        public bool PlaceUnit(UnitController unit)
        {
            Vector2 inputWorldPos = InputManager.Instance.WorldPosition;
            BoardCoordinate placeCoord = GameBoardManager.Instance.GetCoordinateFromWorldPosition(inputWorldPos);

            if (!GameBoardManager.Instance.IsCoordinatePlaceable(placeCoord))
                return false;

            if (!GameBoardManager.Instance.IsCoordinatesPlaceable(unit.GetPlaceCoordinates(placeCoord)))
                return false;

            unit.Place(placeCoord);
            GameBoardManager.Instance.OnUnitPlaced(unit, unit.GetPlaceCoordinates());
            return true;
        }
    }
}