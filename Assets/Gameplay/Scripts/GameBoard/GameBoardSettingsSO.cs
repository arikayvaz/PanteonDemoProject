using UnityEngine;

namespace Gameplay
{
    [CreateAssetMenu(fileName="New GameBoard Settings", menuName="Gameplay/Board/Settings")]
    public class GameBoardSettingsSO : ScriptableObject
    {
        [field: SerializeField] public Vector2Int BoardSize { get; private set; } = Vector2Int.zero;
        [field: SerializeField] public int CellSize { get; private set; } = 32;

        [field: SerializeField, Space] public int BoardSortingOrderGround { get; private set; } = 0;
        [field: SerializeField] public int BoardSortingOrderSpawnPoint { get; private set; } = 0;
        [field: SerializeField] public int BoardSortingOrderPlaced { get; private set; } = 0;
        [field: SerializeField] public int BoardSortingOrderPicked { get; private set; } = 0;
    }
}