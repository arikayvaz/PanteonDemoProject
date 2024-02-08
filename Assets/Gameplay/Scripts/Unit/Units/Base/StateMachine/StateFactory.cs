using Common.GenericStateMachine;
using UnityEngine;

namespace Gameplay.UnitControllerStateMachine
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
                case States.Idle:
                    return new StateIdle(stateMachine);
                case States.MovingToPosition:
                    return new StateMovingToPosition(stateMachine);
                case States.MovingToTarget:
                    return new StateMovingToTarget(stateMachine);
                case States.Attacking:
                    return new StateAttacking(stateMachine);
                default:
                    return null;
            }
        }
    }
}