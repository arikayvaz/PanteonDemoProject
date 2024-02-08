using Common.GenericStateMachine;
using UnityEngine;

namespace Gameplay.UnitControllerStateMachine
{
    public class StateMovingToTarget : StateBase
    {
        public override States StateId => States.MovingToTarget;

        public StateMovingToTarget(GenericStateMachine<States, StateInfo> stateMachine) : base(stateMachine) { }
    }
}