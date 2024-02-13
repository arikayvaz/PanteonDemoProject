using Common;
using Gameplay.GameManagerStateMachine;
using UnityEngine;

namespace Gameplay
{
    public class GameManager : Singleton<GameManager>, IManager
    {
        public bool IsGameRunning => stateMachine?.State?.StateId == States.Game;

        [SerializeField] StateInfo stateInfo = new StateInfo();

        private StateMachine stateMachine;
        private StateFactory stateFactory;

        protected override void Awake()
        {
            base.Awake();

            InitManager();
        }

        private void Start()
        {
            LoadLevel();
        }

        public void InitManager() 
        {
            stateMachine = gameObject.AddComponent<StateMachine>();
            stateFactory = new StateFactory();

            stateMachine.InitStateMachine(stateInfo, stateFactory.GetStates(stateMachine));
            ChangeState(States.None);
        }

        private void LoadLevel() 
        {
            ChangeState(States.LevelLoad);
        }

        private void ChangeState(States stateNew) 
        {
            stateMachine.ChangeState(stateNew);
        }
    }
}