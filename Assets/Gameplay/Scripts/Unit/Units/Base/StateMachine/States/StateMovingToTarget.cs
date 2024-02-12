using Common.GenericStateMachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.UnitControllerStateMachine
{
    public class StateMovingToTarget : StateBase
    {
        public override States StateId => States.MovingToTarget;

        public StateMovingToTarget(GenericStateMachine<States, StateInfo> stateMachine) : base(stateMachine) { }

        public override void OnEnter(StateInfo info)
        {
            base.OnEnter(info);

            info.targetCoordinate = info.attackTarget.GetCoordinate();//info.attackTarget.GetAttackableCoordinate();

            if (info.targetCoordinate == BoardCoordinate.Invalid)
            {
                stateMachine.ChangeState(States.Idle);
                return;
            }

            info.movementController.MoveForAttacking(info.controller
                , info.viewModel.MoveSpeed
                , info.viewModel.Coordinate
                , info.targetCoordinate
                , (coordinate) => OnMoved(info, coordinate)
                , () => OnMovementComplete(info));
        }

        private void OnMoved(StateInfo info, BoardCoordinate coordinate) 
        {
            info.controller.UpdateCoordinate(coordinate);
        }

        private void OnMovementComplete(StateInfo info)
        {
            stateMachine.ChangeState(States.Attacking);
        }
    }
}