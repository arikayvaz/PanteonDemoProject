using Common.GenericStateMachine;
using UnityEngine;

namespace Gameplay.GameManagerStateMachine
{
    public class StateLevelLoad : StateBase
    {
        public override States StateId => States.LevelLoad;

        public StateLevelLoad(GenericStateMachine<States, StateInfo> stateMachine) : base(stateMachine) { }

        public override void OnEnter(StateInfo info)
        {
            base.OnEnter(info);

            GameBoardManager.Instance.InitManager();

            CameraController.Instance.InitController();

            InputManager.Instance.InitManager();

            Pathfinder.Instance.InitPathfinder(GameBoardManager.BoardSettings.boardSize.x, GameBoardManager.BoardSettings.boardSize.y);

            BuildingManager.Instance.InitManager();
            UnitManager.Instance.InitManager();

            BuildingProductionUIController.Instance.InitController();
        }
    }
}
