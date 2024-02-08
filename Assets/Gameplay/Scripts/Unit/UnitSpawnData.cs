using System;
using UnityEngine;

namespace Gameplay
{
    [Serializable]
    public class UnitSpawnData
    {
        [field: SerializeField] public string Name { get; private set; } = "";
        [field: SerializeField] public UnitTypes UnitType { get; private set; } = UnitTypes.None;
        [field: SerializeField] public UnitDataSO UnitData { get; private set; } = null;
        [field: SerializeField] public UnitPooler Pooler { get; private set; } = null;
    }
}