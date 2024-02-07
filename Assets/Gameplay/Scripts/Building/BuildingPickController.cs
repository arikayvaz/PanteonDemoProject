using UnityEngine;
using Common;
using System.Collections.Generic;

namespace Gameplay
{
    public class BuildingPickController : MonoBehaviour, IController, IPicker<BuildingControllerBase>
    {
        public bool IsPickedBuilding => PickedBuilding != null;

        public BuildingControllerBase PickedBuilding { get; private set; } = null;

        public void InitController()
        {
            
        }

        public void PickObject(BuildingControllerBase pickObject)
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