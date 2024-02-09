using Common.GenericStateMachine;
using System;
using UnityEngine;

namespace Gameplay.UnitControllerStateMachine
{
    [Serializable]
    public class StateInfo : GenericStateInfo
    {
        public Transform transform = null;
        public SpriteRenderer spriteVisual = null;
        public Transform trVisual = null;
        public UnitControllerBase controller = null;

        [HideInInspector]
        [NonSerialized]
        public UnitDataSO unitData;
        [NonSerialized]
        [HideInInspector]
        public BoardCoordinate currentCoordinate;
        [NonSerialized]
        [HideInInspector]
        public BoardCoordinate targetCoordinate;
        [NonSerialized]
        [HideInInspector]
        public BoardCoordinate[] movePath;
    }
}