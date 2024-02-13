using UnityEngine;

namespace Gameplay
{
    [CreateAssetMenu(fileName="New Spawn Point Settings", menuName="Gameplay/Building/Spawn Point Data")]
    public class SpawnPointSettingsSO : ScriptableObject
    {
        [field: SerializeField] public Sprite SpriteVisual { get; private set; } = null;
        [field: SerializeField] public Color ColorSpawnPoint { get; private set; } = Color.white;
    }
}