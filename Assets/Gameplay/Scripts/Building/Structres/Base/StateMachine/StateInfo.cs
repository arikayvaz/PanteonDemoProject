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

        [HideInInspector]
        [NonSerialized]
        public BuildingDataSO buildingData;
        [NonSerialized]
        [HideInInspector]
        public BoardCoordinate coordinate;

        public Color ColorBuilding => buildingData?.BuildingColor ?? Color.white;
    }
}