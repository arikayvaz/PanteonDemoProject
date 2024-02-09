using Common;
using Unity.VisualScripting;
using UnityEngine;

namespace Gameplay
{
    public class GameBoardSelectController<T> : IController where T : ISelectable
    {
        public bool IsSelectedObject => selectedObject != null;
        private ISelectable selectedObject = null;

        public void InitController() 
        {
            selectedObject = null;
        }

        public bool SelectObject(BoardCoordinate coordinate) 
        {
            IPlaceable placedObject = GameBoardManager.Instance.GetPlacedObject(coordinate);

            if (placedObject == null)
                return false;

            T foundedObject = (T)placedObject;

            if (foundedObject == null)
                return false;

            if (!foundedObject.CanSelect() || foundedObject.IsEqual(selectedObject))
                return false;

            if (IsSelectedObject)
                DeselectObject();

            foundedObject.Select();
            selectedObject = foundedObject;

            return true;
        }

        public void DeselectObject() 
        {
            if (!IsSelectedObject)
                return;

            selectedObject.Deselect();
            selectedObject = null;
        }

        public T GetSelectedObject() 
        {
            return (T)selectedObject;
        } 
    }
}