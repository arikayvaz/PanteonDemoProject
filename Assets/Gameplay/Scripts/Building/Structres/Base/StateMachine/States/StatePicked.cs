using Common;
using Common.GenericStateMachine;
using UnityEditor;
using UnityEngine;

namespace Gameplay.BuildingControllerStateMachine
{
    public class StatePicked : StateBase
    {
        public override States StateId => States.Picked;

        public StatePicked(GenericStateMachine<States, StateInfo> stateMachine) : base(stateMachine) { }

        private StateInfo stateInfo = null;

        public override void OnEnter(StateInfo info)
        {
            base.OnEnter(info);

            UpdatePosition(info);

            stateInfo = info;

            OnInputCoordinateChanged(InputManager.Instance.CurrentInputCoordinate);
            InputManager.Instance.OnInputCoordinateChange += OnInputCoordinateChanged;
        }

        public override void OnExit(StateInfo info)
        {
            base.OnExit(info);

            stateInfo = null;

            InputManager.Instance.OnInputCoordinateChange -= OnInputCoordinateChanged;
        }

        public override void OnUpdate(StateInfo info)
        {
            base.OnUpdate(info);

            UpdatePosition(info);
        }

        private void UpdatePosition(StateInfo info) 
        {
            info.transform.position = InputManager.Instance.WorldPosition;
        }

        private void OnInputCoordinateChanged(BoardCoordinate updatedCoordinate) 
        {
            bool isCoordinatePlaceable = GameBoardManager.Instance.IsCoordinatesPlaceable(stateInfo.controller.GetPlaceCoordinates(updatedCoordinate));
            Color visualColor = isCoordinatePlaceable ? stateInfo.viewModel.ColorPlaceable : stateInfo.viewModel.ColorUnplaceable;

            stateInfo.controller.UpdateVisualColor(visualColor);
        }
    }
}