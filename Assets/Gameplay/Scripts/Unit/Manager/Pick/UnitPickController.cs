using Common;
using UnityEngine;

namespace Gameplay
{
    public class UnitPickController : MonoBehaviour, IController, IPicker<UnitController>
    {
        public bool IsPickedUnit => PickedUnit != null;

        public UnitController PickedUnit { get; private set; } = null;

        public void InitController()
        {
            
        }

        public void PickObject(UnitController pickObject)
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