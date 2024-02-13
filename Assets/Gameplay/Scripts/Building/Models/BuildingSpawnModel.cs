using Common;
using System;
using UnityEngine;

namespace Gameplay
{
    [Serializable]
    public class BuildingSpawnModel
    {
        [field: SerializeField] public BuildingTypes BuildingType { get; private set; } = BuildingTypes.None;
        [field:SerializeField] public BuildingPooler Pooler { get; private set; } = null;
    }
}