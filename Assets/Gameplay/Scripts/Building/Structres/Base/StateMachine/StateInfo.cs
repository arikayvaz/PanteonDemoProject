using System;
using UnityEngine;
using Common.GenericStateMachine;

namespace Gameplay.BuildingControllerStateMachine
{
    [Serializable]
    public class StateInfo : GenericStateInfo
    {
        public Transform transform = null;
        public SpriteRenderer spriteVisual = null;
        public Transform trVisual = null;
        public BuildingController controller = null;

        [HideInInspector]
        [NonSerialized]
        public BuildingViewModel viewModel;
    }
}