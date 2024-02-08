using Common.GenericStateMachine;
using UnityEngine;

namespace Gameplay.UnitControllerStateMachine
{
    public abstract class StateBase : GenericStateBase<States, StateInfo>
    {
        protected StateBase(GenericStateMachine<States, StateInfo> stateMachine) : base(stateMachine) { }
    }
}