using Common.GenericStateMachine;
using UnityEngine;

namespace Gameplay.UnitControllerStateMachine
{
    public class StateSelected : StateBase
    {
        public override States StateId => States.Selected;

        public StateSelected(GenericStateMachine<States, StateInfo> stateMachine) : base(stateMachine) { }

        public override void OnEnter(StateInfo info)
        {
            base.OnEnter(info);

            info.controller.UpdateVisualColor(info.unitData.StateColorData.ColorSelected);
        }

        public override void OnExit(StateInfo info)
        {
            base.OnExit(info);

            info.controller.UpdateVisualColor(info.unitData.UnitColor);
        }
    }
}