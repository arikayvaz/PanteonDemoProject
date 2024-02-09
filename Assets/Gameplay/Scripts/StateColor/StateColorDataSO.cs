using UnityEngine;

namespace Gameplay
{
    [CreateAssetMenu(fileName="New State Color Data", menuName="Gameplay/State Color/New Data")]
    public class StateColorDataSO : ScriptableObject
    {
        [field: SerializeField] public Color ColorPlaceable { get; private set; } = Color.white;
        [field: SerializeField] public Color ColorUnPlaceable { get; private set; } = Color.white;
        [field: SerializeField] public Color ColorSelected { get; private set; } = Color.white;
    }
}