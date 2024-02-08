using Common.GenericStateMachine;
using UnityEngine;

namespace Gameplay.UnitControllerStateMachine
{
    public class StateIdle : StateBase
    {
        public override States StateId => States.Idle;

        public StateIdle(GenericStateMachine<States, StateInfo> stateMachine) : base(stateMachine) { }
    }
}