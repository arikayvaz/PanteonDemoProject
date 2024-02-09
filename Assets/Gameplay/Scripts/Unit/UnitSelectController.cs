using Common;
using UnityEngine;

namespace Gameplay
{
    public class UnitSelectController : MonoBehaviour, IController
    {
        public bool IsUnitSelected => selectedUnit != null;
        private UnitControllerBase selectedUnit = null;

        public void InitController() 
        {
            selectedUnit = null;
        }

        public bool SelectUnit(BoardCoordinate coordinate) 
        {
            IPlaceable placedObject = GameBoardManager.Instance.GetPlacedObject(coordinate);

            if (placedObject == null)
                return false;

            UnitControllerBase unit = placedObject as UnitControllerBase;

            if (unit == null)
                return false;

            if (!unit.CanSelect() || unit == selectedUnit)
                return false;

            if (IsUnitSelected)
                DeselectUnit();

            selectedUnit = unit;
            selectedUnit.Select();

            return true;
        }

        public void DeselectUnit() 
        {
            if (!IsUnitSelected)
                return;

            selectedUnit.Deselect();
            selectedUnit = null;
        }
    }
}