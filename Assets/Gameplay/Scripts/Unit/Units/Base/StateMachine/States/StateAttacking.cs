using Common.GenericStateMachine;
using UnityEngine;

namespace Gameplay.UnitControllerStateMachine
{
    public class StateAttacking : StateBase
    {
        public override States StateId => States.Attacking;

        public StateAttacking(GenericStateMachine<States, StateInfo> stateMachine) : base(stateMachine) { }
    }
}
