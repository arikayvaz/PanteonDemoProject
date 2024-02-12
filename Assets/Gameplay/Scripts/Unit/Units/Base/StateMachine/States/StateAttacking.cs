using Common.GenericStateMachine;
using UnityEngine;

namespace Gameplay.UnitControllerStateMachine
{
    public class StateAttacking : StateBase
    {
        public override States StateId => States.Attacking;

        public StateAttacking(GenericStateMachine<States, StateInfo> stateMachine) : base(stateMachine) { }

        public override void OnEnter(StateInfo info)
        {
            base.OnEnter(info);

            info.attackController.StartAttacking(info.attackTarget
                , info.viewModel.AttackDamage
                , info.viewModel.AttackDelay
                , () => OnTargetDied(info));
        }

        private void OnTargetDied(StateInfo info) 
        {
            info.attackTarget.OnDied();
            info.attackTarget = null;

            stateMachine.ChangeState(States.Idle);
        }
    }
}
