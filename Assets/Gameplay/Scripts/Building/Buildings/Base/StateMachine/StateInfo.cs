using System;
using UnityEngine;
using Common.GenericStateMachine;
using TMPro;

namespace Gameplay.BuildingControllerStateMachine
{
    [Serializable]
    public class StateInfo : GenericStateInfo
    {
        public Transform transform = null;
        public BuildingController controller = null;
        public BoardVisual boardVisual = null;
        public TextMeshPro textBuildingName = null;
        public SpawnPoint spawnPoint = null;

        [HideInInspector]
        [NonSerialized]
        public BuildingViewModel viewModel;
    }
}