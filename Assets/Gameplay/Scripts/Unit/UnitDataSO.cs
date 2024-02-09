using UnityEngine;

namespace Gameplay
{
    [CreateAssetMenu(fileName = "New Unit Data", menuName = "Gameplay/Units/New Data")]
    public class UnitDataSO : ScriptableObject
    {
        [field: SerializeField, Min(0)] public int CellSizeX { get; private set; } = 0;
        [field: SerializeField, Min(0)] public int CellSizeY { get; private set; } = 0;
        [field: SerializeField, Min(0)] public int Health { get; private set; } = 0;
        [field: SerializeField, Min(0)] public int AttackDamage { get; private set; } = 0;
        [field: SerializeField, Min(0f)] public float AttackTimeInterval { get; private set; } = 0;
        [field: SerializeField, Min(1)] public int MoveSpeed { get; private set; } = 1;
        [field: SerializeField] public Color UnitColor { get; private set; } = Color.white;
        [field: SerializeField] public StateColorDataSO StateColorData { get; private set; } = null;
        public Color ColorPlaceable => StateColorData?.ColorPlaceable ?? Color.white;
        public Color ColorUnPlaceable => StateColorData?.ColorUnPlaceable ?? Color.white;
    }
}