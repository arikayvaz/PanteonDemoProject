using Common.GenericStateMachine;
using UnityEngine;

namespace Gameplay.GameManagerStateMachine
{
    public abstract class StateBase : GenericStateBase<States, StateInfo>
    {
        protected StateBase(GenericStateMachine<States, StateInfo> stateMachine) : base(stateMachine) { }
    }
}