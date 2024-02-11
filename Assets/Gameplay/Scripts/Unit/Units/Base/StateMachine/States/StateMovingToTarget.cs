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

            Debug.Log("Moving to Target");

            info.targetCoordinate = info.attackTarget.GetAttackableCoordinate();

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

        public override void OnExit(StateInfo info)
        {
            base.OnExit(info);

            Debug.Log("Stopped moving to Target");
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