using Common.GenericStateMachine;
using UnityEngine;

namespace Gameplay.UnitControllerStateMachine
{
    public class StateMovingToPosition : StateBase
    {
        public override States StateId => States.MovingToPosition;

        public StateMovingToPosition(GenericStateMachine<States, StateInfo> stateMachine) : base(stateMachine) { }
    }
}