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

        private void Start()
        {
            Pathfinder.Instance.InitPathfinder(boardSettings.boardSize.x, boardSettings.boardSize.y);
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
                AddPlacedObject(placedObject, coordinate);
            }
        }

        public void OnUnitPlaced(IPlaceable placedObject, IEnumerable<BoardCoordinate> coordinates) 
        {
            foreach (BoardCoordinate coordinate in coordinates)
            {
                AddPlacedObject(placedObject, coordinate);
            }
        }

        

        public BoardCoordinate GetCoordinateFromWorldPosition(Vector3 worldPosition) 
        {
            BoardCoordinate coordinate = new BoardCoordinate();

            int fixedX = Mathf.CeilToInt(worldPosition.x);
            int fixedY = Mathf.CeilToInt(worldPosition.y);

            coordinate.x = fixedX < 0 ? -1 : Mathf.RoundToInt(fixedX / BoardSettings.cellSize);
            coordinate.y = fixedY < 0 ? -1 : Mathf.RoundToInt(fixedY / BoardSettings.cellSize);

            return coordinate;
        }

        private void InitPlacedObjects() 
        {
            placedObjects = new Dictionary<BoardCoordinate, IPlaceable>(boardSettings.boardSize.x * boardSettings.boardSize.y);
        }

        private void AddPlacedObject(IPlaceable placedObject, BoardCoordinate coordinate) 
        {
            if (placedObjects.ContainsKey(coordinate))
            {
                Debug.LogError("GameBoardManager: AddPlaceObject: contains key:" + coordinate);
                return;
            }

            placedObjects.Add(coordinate, placedObject);
            Pathfinder.Instance.UpdatePathNodeState(coordinate.x, coordinate.y, false);
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