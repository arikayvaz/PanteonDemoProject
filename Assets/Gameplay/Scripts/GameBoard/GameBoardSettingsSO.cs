using UnityEngine;

namespace Gameplay
{
    [CreateAssetMenu(fileName="New GameBoard Settings", menuName="Gameplay/Board/Settings")]
    public class GameBoardSettingsSO : ScriptableObject
    {
        public Vector2Int boardSize = Vector2Int.zero;
        public int cellSize = 32;
    }
}