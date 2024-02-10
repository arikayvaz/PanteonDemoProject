using Common.GenericStateMachine;
using UnityEngine;

namespace Gameplay.BuildingControllerStateMachine
{
    public class StateSelected : StateBase
    {
        public override States StateId => States.Selected;

        public StateSelected(GenericStateMachine<States, StateInfo> stateMachine) : base(stateMachine) { }

        public override void OnEnter(StateInfo info)
        {
            base.OnEnter(info);

            info.controller.UpdateVisualColor(info.viewModel.ColorSelected);
        }

        public override void OnExit(StateInfo info)
        {
            base.OnExit(info);

            info.controller.UpdateVisualColor(info.viewModel.BuildingColor);
        }
    }
}