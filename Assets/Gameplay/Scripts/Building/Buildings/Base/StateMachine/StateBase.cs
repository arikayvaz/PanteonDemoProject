using Common.GenericStateMachine;
using UnityEngine;

namespace Gameplay.BuildingControllerStateMachine
{
    public abstract class StateBase : GenericStateBase<States, StateInfo>
    {
        public StateBase(GenericStateMachine<States, StateInfo> stateMachine) : base(stateMachine) { }
    }
}