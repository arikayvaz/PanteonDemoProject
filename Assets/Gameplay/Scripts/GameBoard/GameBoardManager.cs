using Common;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

namespace Gameplay
{
    public class GameBoardManager : Singleton<GameBoardManager>, IManager
    {
        [SerializeField] GameBoardSettingsSO boardSettings = null;
        public static GameBoardSettingsSO BoardSettings => Instance?.boardSettings;

        [Space]
        [SerializeField] GameObject goCell = null;

        private GameBoardCoordinates[] boardCoordinates = null;

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

        public Vector2 GetWorldPositionFromCoordinate(GameBoardCoordinates coordinate) 
        {
            return new Vector2(coordinate.x * BoardSettings.cellSize, coordinate.y * BoardSettings.cellSize);
        }

        public bool IsCoordinateInBoardBounds(GameBoardCoordinates coordinate) 
        {
            return GameBoardCoordinates.IsValid(coordinate)
                && coordinate.x < BoardSettings.boardSize.x
                && coordinate.y < BoardSettings.boardSize.y;
        }

        public bool IsCoordinatesPlaceable(IEnumerable<GameBoardCoordinates> coordinates) 
        {
            foreach (GameBoardCoordinates coordinate in coordinates)
            {
                if (!IsCoordinatePlaceable(coordinate))
                    return false;
            }

            return true;
        }

        public bool IsCoordinatePlaceable(GameBoardCoordinates coordinate) 
        {
            if (!IsCoordinateInBoardBounds(coordinate))
                return false;

            foreach (GameBoardCoordinates boardCoord in boardCoordinates)
            {
                if (!GameBoardCoordinates.IsValid(boardCoord))
                    continue;

                return false;
            }

            return true;
        }

        public GameBoardCoordinates GetCoordinateFromWorldPosition(Vector3 worldPosition) 
        {
            GameBoardCoordinates coordinate = new GameBoardCoordinates();

            int fixedX = Mathf.CeilToInt(worldPosition.x);
            int fixedY = Mathf.CeilToInt(worldPosition.y);

            coordinate.x = fixedX < 0 ? -1 : Mathf.RoundToInt(fixedX / GameBoardManager.BoardSettings.cellSize);
            coordinate.y = fixedY < 0 ? -1 : Mathf.RoundToInt(fixedY / GameBoardManager.BoardSettings.cellSize);

            return coordinate;
        }

        private void InitBoardCoordinates() 
        {
            boardCoordinates = new GameBoardCoordinates[boardSettings.boardSize.x * boardSettings.boardSize.y];

            for (int i = 0; i < boardCoordinates.Length; i++)
                boardCoordinates[i] = GameBoardCoordinates.Invalid;
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
                    rend.color = (x + y) % 2 == 0 ? UnityEngine.Color.white : UnityEngine.Color.black;
                }
            }
        }
    }
}