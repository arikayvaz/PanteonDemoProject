using Common.GenericStateMachine;
using System;
using UnityEngine;

namespace Gameplay.UnitControllerStateMachine
{
    [Serializable]
    public class StateInfo : GenericStateInfo
    {
        public Transform transform = null;
        public SpriteRenderer spriteVisual = null;
        public Transform trVisual = null;
        public UnitController controller = null;

        [NonSerialized]
        [HideInInspector]
        public UnitViewModel viewModel = null;

        [NonSerialized]
        [HideInInspector]
        public BoardCoordinate targetCoordinate;
        [NonSerialized]
        [HideInInspector]
        public BoardCoordinate[] movePath;
    }
}