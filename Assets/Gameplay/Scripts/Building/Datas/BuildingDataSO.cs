using UnityEngine;

namespace Gameplay
{
    [CreateAssetMenu(fileName = "New Building Data", menuName = "Gameplay/Building/New Data")]
    public class BuildingDataSO : ScriptableObject
    {
        [field: SerializeField, Min(0)] public int CellSizeX { get; private set; } = 0;
        [field: SerializeField, Min(0)] public int CellSizeY { get; private set; } = 0;
        [field: SerializeField, Min(0)] public int Health { get; private set; } = 0;
        [field: SerializeField] public Color BuildingColor { get; private set; } = Color.white;
        [field: SerializeField] public StateColorDataSO StateColorData { get; private set; } = null;
    }
}