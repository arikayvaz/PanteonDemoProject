using Common.GenericStateMachine;
using UnityEngine;

namespace Gameplay.BuildingControllerStateMachine
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
        }

        private void SetPosition(StateInfo info) 
        {
            info.transform.position = GameBoardManager.Instance.GetWorldPositionFromCoordinate(info.coordinate);
        }

        private void UpdateColor(StateInfo info) 
        {
            info.controller.UpdateVisualColor(info.colorPlaced);
        }
    }
}