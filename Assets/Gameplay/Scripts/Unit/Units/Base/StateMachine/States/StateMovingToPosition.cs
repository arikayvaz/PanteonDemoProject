using Common.GenericStateMachine;
using System;
using System.Collections;
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

            if (info.movePath?.Length < 1 || info.targetCoordinate == BoardCoordinate.Invalid || info.targetCoordinate == info.currentCoordinate) 
            {
                OnMovementComplete(info);
                return;
            }

            Debug.Log("Start moving");

            float moveInterval = 1f / info.unitData.MoveSpeed;

            info.controller.HandleCoroutine(MovementCoroutine(info, moveInterval, () => OnMovementComplete(info)));
        }

        public override void OnExit(StateInfo info)
        {
            base.OnExit(info);

            Debug.Log("End moving");
        }

        private IEnumerator MovementCoroutine(StateInfo info, float moveInterval, Action OnComplete) 
        {
            int index = 0;
            while (index < info.movePath.Length)
            {
                BoardCoordinate coordinate = info.movePath[index];
                bool success = GameBoardManager.Instance.UpdatePlaceableCoordinate(info.currentCoordinate, coordinate);

                if (!success)
                    break;

                info.controller.UpdateCoordinate(coordinate);
                index++;

                if (index > info.movePath.Length - 1)
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
    }
}