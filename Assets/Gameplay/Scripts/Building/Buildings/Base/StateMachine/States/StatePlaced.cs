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

            info.boardVisual.UpdateSortingOrder(GameBoardManager.BoardSettings.BoardSortingOrderPlaced);
            info.textBuildingName.sortingOrder = GameBoardManager.BoardSettings.BoardSortingOrderPlaced;

            if (info.viewModel.IsProduceUnits)
                info.spawnPoint.OnPlaced(info.viewModel.SpawnPointCoordinate);

        }
    }
}