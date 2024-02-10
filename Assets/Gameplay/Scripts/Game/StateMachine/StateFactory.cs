using Common.GenericStateMachine;
using UnityEngine;

namespace Gameplay.GameManagerStateMachine
{
    public class StateFactory : GenericStateFactory<States, StateInfo, StateMachine>
    {
        public override GenericStateBase<States, StateInfo> CreateState(States stateId, StateMachine stateMachine)
        {
            switch (stateId)
            {
                case States.None:
                    return new StateNone(stateMachine);
                case States.LevelLoad:
                    return new StateLevelLoad(stateMachine);
                case States.Game:
                    return new StateGame(stateMachine);
                default:
                    return null;
            }
        }
    }
}