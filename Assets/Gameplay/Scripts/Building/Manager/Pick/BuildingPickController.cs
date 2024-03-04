using UnityEngine;
using Common;
using System.Collections.Generic;

namespace Gameplay
{
    public class BuildingPickController : MonoBehaviour, IController, IPicker
    {
        public bool IsPickedBuilding => PickedBuilding != null;

        public IPickable PickedBuilding { get; private set; } = null;

        public void InitController()
        {
            
        }

        public void PickObject(IPickable pickable)
        {
            if (IsPickedBuilding)
                pickable.Drop();

            pickable.Pick();
            PickedBuilding = pickable;
        }

        public void DropObject()
        {
            if (!IsPickedBuilding)
                return;

            PickedBuilding.Drop();
            PickedBuilding = null;
        }
    }
}