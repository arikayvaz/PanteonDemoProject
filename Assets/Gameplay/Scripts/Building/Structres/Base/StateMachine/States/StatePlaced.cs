using Common.GenericStateMachine;
using UnityEngine;

namespace Gameplay.BuildingControllerStateMachine
{
    public class StatePlaced : StateBase
    {
        public override States StateId => States.Placed;

        public StatePlaced(GenericStateMachine<States, StateInfo> stateMachine) : base(stateMachine) { }
    }
}