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

        //private BoardCoordinate[] boardCoordinates = null;
        private Dictionary<BoardCoordinate, IPlaceable> placedObjects = null;

        protected override void Awake()
        {
            base.Awake();

            InitManager();
        }

        public void InitManager()
        {
            InitPlacedObjects();
            SpawnBoardCells();
        }

        public Vector2 GetWorldPositionFromCoordinate(BoardCoordinate coordinate) 
        {
            return new Vector2(coordinate.x * BoardSettings.cellSize, coordinate.y * BoardSettings.cellSize);
        }

        public bool IsCoordinateInBoardBounds(BoardCoordinate coordinate) 
        {
            return BoardCoordinate.IsValid(coordinate)
                && coordinate.x < BoardSettings.boardSize.x
                && coordinate.y < BoardSettings.boardSize.y;
        }

        public bool IsCoordinatesPlaceable(IEnumerable<BoardCoordinate> coordinates) 
        {
            foreach (BoardCoordinate coordinate in coordinates)
            {
                if (!IsCoordinatePlaceable(coordinate))
                    return false;
            }

            return true;
        }

        public bool IsCoordinatePlaceable(BoardCoordinate coordinate) 
        {
            if (!IsCoordinateInBoardBounds(coordinate))
                return false;

            if (placedObjects.Count < 1)
                return true;

            IPlaceable placedObject = null;
            placedObjects.TryGetValue(coordinate, out placedObject);

            return placedObject == null;
        }

        public void OnBuildingPlaced(IPlaceable placedObject, IEnumerable<BoardCoordinate> coordinates) 
        {
            foreach (BoardCoordinate coordinate in coordinates)
            {
                if (placedObjects.ContainsKey(coordinate)) 
                {
                    Debug.LogError("GameBoardManager: OnBuildingPlaced: contains key:" + coordinate);
                    continue;
                }

                placedObjects.Add(coordinate, placedObject);
            }
        }

        public BoardCoordinate GetCoordinateFromWorldPosition(Vector3 worldPosition) 
        {
            BoardCoordinate coordinate = new BoardCoordinate();

            int fixedX = Mathf.CeilToInt(worldPosition.x);
            int fixedY = Mathf.CeilToInt(worldPosition.y);

            coordinate.x = fixedX < 0 ? -1 : Mathf.RoundToInt(fixedX / GameBoardManager.BoardSettings.cellSize);
            coordinate.y = fixedY < 0 ? -1 : Mathf.RoundToInt(fixedY / GameBoardManager.BoardSettings.cellSize);

            return coordinate;
        }

        private void InitPlacedObjects() 
        {
            /*
            boardCoordinates = new BoardCoordinate[boardSettings.boardSize.x * boardSettings.boardSize.y];

            for (int i = 0; i < boardCoordinates.Length; i++)
                boardCoordinates[i] = BoardCoordinate.Invalid;
            */

            placedObjects = new Dictionary<BoardCoordinate, IPlaceable>(boardSettings.boardSize.x * boardSettings.boardSize.y);

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