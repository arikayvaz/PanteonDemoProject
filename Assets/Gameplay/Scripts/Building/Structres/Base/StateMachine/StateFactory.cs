using Common.GenericStateMachine;
using UnityEngine;

namespace Gameplay.BuildingControllerStateMachine
{
    public class StateFactory : GenericStateFactory<States, StateInfo, StateMachine>
    {
        public override GenericStateBase<States, StateInfo> CreateState(States stateId, StateMachine stateMachine)
        {
            switch (stateId)
            {
                case States.None:
                    return new StateNone(stateMachine);
                case States.Picked:
                    return new StatePicked(stateMachine);
                case States.Placed:
                    return new StatePlaced(stateMachine);
                default:
                    return null;
            }
        }
    }
}