using Common;
using UnityEngine;

namespace Gameplay
{
    public class UnitPickController : MonoBehaviour, IController, IPicker<UnitControllerBase>
    {
        public bool IsPickedUnit => PickedUnit != null;

        public UnitControllerBase PickedUnit { get; private set; } = null;

        public void InitController()
        {
            
        }

        public void PickObject(UnitControllerBase pickObject)
        {
            if (IsPickedUnit)
                pickObject.Drop();

            pickObject.Pick();
            PickedUnit = pickObject;
        }

        public void DropObject()
        {
            if (!IsPickedUnit)
                return;

            PickedUnit.Drop();
            PickedUnit = null;
        }


    }
}