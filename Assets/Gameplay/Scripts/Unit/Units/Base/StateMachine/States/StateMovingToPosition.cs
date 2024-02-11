using Common.GenericStateMachine;
using Gameplay.BuildingControllerStateMachine;
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

        private StateInfo stateInfo = null;
        private IEnumerator movementEnumerator = null;
        private int movementIndex = -1;

        public override void OnEnter(StateInfo info)
        {
            base.OnEnter(info);

            if (info.movePath?.Length < 1 || info.targetCoordinate == BoardCoordinate.Invalid || info.targetCoordinate == info.viewModel.Coordinate) 
            {
                OnMovementComplete(info);
                return;
            }

            Debug.Log("Start moving");

            stateInfo = info;

            float moveInterval = 1f / info.viewModel.MoveSpeed;
            movementEnumerator = MovementCoroutine(info, moveInterval, () => OnMovementComplete(info));

            info.controller.HandleCoroutine(movementEnumerator);

            GameBoardManager.Instance.OnObjectPlaced.AddListener(OnObjectPlaced);
            GameBoardManager.Instance.OnPlaceObjectUpdated.AddListener(OnPlaceObjectUpdated);
        }

        public override void OnExit(StateInfo info)
        {
            base.OnExit(info);

            Debug.Log("End moving");

            stateInfo = null;
            movementEnumerator = null;
            movementIndex = -1;

            GameBoardManager.Instance.OnObjectPlaced.RemoveListener(OnObjectPlaced);
            GameBoardManager.Instance.OnPlaceObjectUpdated.RemoveListener(OnPlaceObjectUpdated);
        }

        private IEnumerator MovementCoroutine(StateInfo info, float moveInterval, Action OnComplete) 
        {
            movementIndex = 0;
            while (movementIndex < info.movePath.Length)
            {
                BoardCoordinate coordinate = info.movePath[movementIndex];
                bool success = GameBoardManager.Instance.UpdatePlaceableCoordinate(info.viewModel.Coordinate, coordinate);

                if (!success)
                    break;

                info.controller.UpdateCoordinate(coordinate);
                movementIndex++;

                if (movementIndex > info.movePath.Length - 1)
                    break;
                else
                    yield return new WaitForSeconds(moveInterval);
            }

            OnComplete?.Invoke();
        }

        private void OnMovementComplete(StateInfo info) 
        {
            info.targetCoordinate = BoardCoordinate.Invalid;
            info.movePath = null;

            stateMachine.ChangeState(States.Idle);
        }

        private void OnObjectPlaced(IPlaceable placedObject, IEnumerable<BoardCoordinate> coordinates) 
        {
            if (stateInfo.controller.IsEqual(placedObject))
                return;

            bool isInWay = false;
            bool isTargetOccupied = false;
            foreach (BoardCoordinate coordinate in coordinates)
            {
                if (!IsCoordinateInMovePath(coordinate))
                    continue;

                isInWay = true;
                isTargetOccupied = coordinate == stateInfo.targetCoordinate;

                if (isTargetOccupied)
                    break;
            }

            if (!isInWay)
                return;

            if (isTargetOccupied)
            {
                stateInfo.controller.TerminateCoroutine(movementEnumerator);
                OnMovementComplete(stateInfo);
                return;
            }

            stateInfo.controller.TerminateCoroutine(movementEnumerator);
            stateInfo.movePath = Pathfinder.Instance.CalculatePathCoordinates(stateInfo.viewModel.Coordinate, stateInfo.targetCoordinate).ToArray();
            stateInfo.controller.HandleCoroutine(movementEnumerator);
        }

        private void OnPlaceObjectUpdated(IPlaceable placedObject, BoardCoordinate coordinate) 
        {
            if (stateInfo.controller.IsEqual(placedObject))
                return;

            if (movementIndex < 0)
                return;

            int nextMoveIndex = 0;
            if (movementIndex + stateInfo.viewModel.MoveSpeed > stateInfo.movePath.Length - 1)
                nextMoveIndex = stateInfo.movePath.Length - 1;
            else
                nextMoveIndex = movementIndex + stateInfo.viewModel.MoveSpeed;

            BoardCoordinate nextCoordinate = stateInfo.movePath[nextMoveIndex];

            if (nextCoordinate != coordinate)
                return;

            stateInfo.controller.TerminateCoroutine(movementEnumerator);
            stateInfo.movePath = Pathfinder.Instance.CalculatePathCoordinates(stateInfo.viewModel.Coordinate, stateInfo.targetCoordinate).ToArray();
            stateInfo.controller.HandleCoroutine(movementEnumerator);

        }

        private bool IsCoordinateInMovePath(BoardCoordinate targetCoordinate) 
        {
            if (stateInfo.movePath?.Length < 1)
                return false;

            foreach (BoardCoordinate coordinate in stateInfo.movePath)
            {
                if (coordinate == targetCoordinate)
                    return true;
            }

            return false;
        }
    }
}