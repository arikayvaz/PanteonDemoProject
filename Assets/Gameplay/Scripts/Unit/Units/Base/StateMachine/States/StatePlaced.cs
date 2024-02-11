using Common.GenericStateMachine;
using UnityEngine;

namespace Gameplay.UnitControllerStateMachine
{
    public class StatePlaced : StateBase
    {
        public override States StateId => States.Placed;

        public StatePlaced(GenericStateMachine<States, StateInfo> stateMachine) : base(stateMachine) { }

        public override void OnEnter(StateInfo info)
        {
            base.OnEnter(info);

            SetPosition(info);
            UpdateColor(info);

            info.boardVisual.UpdateSortingOrder(GameBoardManager.BoardSettings.BoardSortingOrderPlaced);

            stateMachine.ChangeState(States.Idle);
        }

        private void SetPosition(StateInfo info)
        {
            info.transform.position = GameBoardManager.Instance.GetWorldPositionFromCoordinate(info.viewModel.Coordinate);
        }

        private void UpdateColor(StateInfo info)
        {
            info.controller.UpdateVisualColor(info.viewModel.UnitColor);
        }
    }
}