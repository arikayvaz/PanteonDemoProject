using UnityEngine;

namespace Gameplay
{
    [CreateAssetMenu(fileName = "New Unit Data", menuName = "Gameplay/Units/New Data")]
    public class UnitDataSO : ScriptableObject
    {
        [field: SerializeField] public UnitTypes UnitType { get; private set; } = UnitTypes.None;
        [field: SerializeField] public string UnitName { get; private set; } = "";
        public int CellSizeX => 1;
        public int CellSizeY => 1;
        [field: SerializeField, Min(0)] public int Health { get; private set; } = 0;
        [field: SerializeField, Min(0)] public int AttackDamage { get; private set; } = 0;
        [field: SerializeField, Min(0.01f)] public float AttackDelay { get; private set; } = 0;
        [field: SerializeField, Min(1)] public int MoveSpeed { get; private set; } = 1;
        [field: SerializeField] public Sprite SpriteUnit { get; private set; } = null;
        [field: SerializeField] public Color UnitColor { get; private set; } = Color.white;
        [field: SerializeField] public StateColorDataSO StateColorData { get; private set; } = null;
    }
}