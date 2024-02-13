using Common.GenericStateMachine;
using UnityEngine;

namespace Gameplay.BuildingControllerStateMachine
{
    public class StateNone : StateBase
    {
        public override States StateId => States.None;

        public StateNone(GenericStateMachine<States, StateInfo> stateMachine) : base(stateMachine) { }
    }
}