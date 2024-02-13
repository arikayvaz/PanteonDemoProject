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

            Pathfinder.Instance.InitPathfinder(GameBoardManager.BoardSettings.BoardSize.x, GameBoardManager.BoardSettings.BoardSize.y);

            UnitManager.Instance.InitManager();
            BuildingManager.Instance.InitManager();

            GameUIController.Instance.InitController();

            BuildingManager.Instance.SubscribeEvents();
            UnitManager.Instance.SubscribeEvents();

            stateMachine.ChangeState(States.Game);
        }
    }
}
