using UnityEngine;
using Common;
using System.Collections.Generic;

namespace Gameplay
{
    public class BuildingPickController : MonoBehaviour, IController, IPicker<BuildingController>
    {
        public bool IsPickedBuilding => PickedBuilding != null;

        public BuildingController PickedBuilding { get; private set; } = null;

        public void InitController()
        {
            
        }

        public void PickObject(BuildingController pickObject)
        {
            if (IsPickedBuilding)
                pickObject.Drop();

            pickObject.Pick();
            PickedBuilding = pickObject;
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