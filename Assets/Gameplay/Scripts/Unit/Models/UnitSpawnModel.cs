using System;
using UnityEngine;

namespace Gameplay
{
    [Serializable]
    public class UnitSpawnModel
    {
        [field: SerializeField] public UnitTypes UnitType { get; private set; } = UnitTypes.None;
        [field: SerializeField] public UnitPooler Pooler { get; private set; } = null;
    }
}