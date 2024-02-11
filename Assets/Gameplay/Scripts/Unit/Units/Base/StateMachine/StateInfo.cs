using Common.GenericStateMachine;
using System;
using UnityEngine;

namespace Gameplay.UnitControllerStateMachine
{
    [Serializable]
    public class StateInfo : GenericStateInfo
    {
        public Transform transform = null;
        public UnitController controller = null;
        public BoardVisual boardVisual = null;
        public UnitMovementController movementController = null;

        [NonSerialized]
        [HideInInspector]
        public UnitViewModel viewModel = null;

        [NonSerialized]
        [HideInInspector]
        public BoardCoordinate targetCoordinate;

        [NonSerialized]
        [HideInInspector]
        public IDamageable attackTarget;
    }
}