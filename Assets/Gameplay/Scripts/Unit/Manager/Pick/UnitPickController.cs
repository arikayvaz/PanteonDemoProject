using Common;
using UnityEngine;

namespace Gameplay
{
    public class UnitPickController : MonoBehaviour, IController, IPicker
    {
        public bool IsPickedUnit => PickedUnit != null;

        public IPickable PickedUnit { get; private set; } = null;

        public void InitController()
        {
            
        }

        public void PickObject(IPickable pickable)
        {
            if (IsPickedUnit)
                pickable.Drop();

            pickable.Pick();
            PickedUnit = pickable;
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