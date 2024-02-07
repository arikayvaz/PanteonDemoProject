using Common;
using UnityEngine;

namespace Gameplay
{
    public class GameBoardManager : Singleton<GameBoardManager>, IManager
    {
        [SerializeField] GameBoardSettingsSO boardSettings = null;
        public static GameBoardSettingsSO BoardSettings => Instance?.boardSettings;

        [Space]
        [SerializeField] GameObject goCell = null;

        private int[,] boardCoordinates = null;

        protected override void Awake()
        {
            base.Awake();

            InitManager();
        }

        public void InitManager()
        {
            InitBoardCoordinates();
            SpawnBoardCells();
        }

        private void InitBoardCoordinates() 
        {
            boardCoordinates = new int[boardSettings.boardSize.x, boardSettings.boardSize.y];
        }

        private void SpawnBoardCells() 
        {
            for (int y = 0; y < boardSettings.boardSize.y; y++)
            {
                for (int x = 0; x < boardSettings.boardSize.x; x++)
                {
                    Vector2 pos = new Vector2(x, y) * boardSettings.cellSize;
                    GameObject cell = Instantiate(goCell, pos, Quaternion.identity);
                    cell.name = $"Cell_({x},{y})";

                    SpriteRenderer rend = cell.GetComponentInChildren<SpriteRenderer>();
                    rend.color = (x + y) % 2 == 0 ? Color.white : Color.black;
                }
            }
        }
    }
}