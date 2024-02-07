using System;
using UnityEngine;
using Common.GenericStateMachine;

namespace Gameplay.BuildingControllerStateMachine
{
    [Serializable]
    public class StateInfo : GenericStateInfo
    {
        public Transform transform = null;
        public SpriteRenderer spriteVisual = null;
        public BuildingControllerBase controller = null;
        public Color colorPlaced = Color.white;

        [HideInInspector]
        [NonSerialized]
        public BuildingDataSO buildingData;
        [NonSerialized]
        [HideInInspector]
        public GameBoardCoordinates coordinate;
    }
}