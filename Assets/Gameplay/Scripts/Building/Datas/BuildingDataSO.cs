using UnityEngine;

namespace Gameplay
{
    [CreateAssetMenu(fileName = "New Building Data", menuName = "Gameplay/Building/New Data")]
    public class BuildingDataSO : ScriptableObject
    {
        [field: SerializeField] public BuildingTypes BuildingType { get; private set; } = BuildingTypes.None;
        [field: SerializeField] public string BuildingName { get; private set; } = "";
        [field: SerializeField, Min(0)] public int CellSizeX { get; private set; } = 0;
        [field: SerializeField, Min(0)] public int CellSizeY { get; private set; } = 0;
        [field: SerializeField, Min(0)] public int Health { get; private set; } = 0;
        [field: SerializeField] public Sprite SpriteBuilding { get; private set; } = null;
        [field: SerializeField] public Color BuildingColor { get; private set; } = Color.white;
        [field: SerializeField] public StateColorDataSO StateColorData { get; private set; } = null;
    }
}