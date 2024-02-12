using Common.GenericStateMachine;
using Gameplay.BuildingControllerStateMachine;
using Gameplay.GameManagerStateMachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.UnitControllerStateMachine
{
    public class StateMovingToPosition : StateBase
    {
        public override States StateId => States.MovingToPosition;

        public StateMovingToPosition(GenericStateMachine<States, StateInfo> stateMachine) : base(stateMachine) { }

        public override void OnEnter(StateInfo info)
        {
            base.OnEnter(info);

            info.movementController.MoveForWandering(info.controller
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
            info.targetCoordinate = BoardCoordinate.Invalid;

            stateMachine.ChangeState(States.Idle);
        }

    }
}