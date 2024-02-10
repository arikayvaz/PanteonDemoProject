using Common.GenericStateMachine;
using UnityEngine;

namespace Gameplay.GameManagerStateMachine
{
    public class StateGame : StateBase
    {
        public override States StateId => States.Game;

        public StateGame(GenericStateMachine<States, StateInfo> stateMachine) : base(stateMachine) { }
    }
}
