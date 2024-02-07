using Common;
using System;
using UnityEngine;

namespace Gameplay
{
    [Serializable]
    public class BuildingSpawnData
    {
        [field: SerializeField] public string Name { get; private set; } = "";
        [field:SerializeField] public BuildingTypes BuildingType { get; private set; } = BuildingTypes.None;
        [field:SerializeField] public BuildingDataSO BuildingData { get; private set; } = null;
        [field:SerializeField] public BuildingPooler Pooler { get; private set; } = null;
    }
}