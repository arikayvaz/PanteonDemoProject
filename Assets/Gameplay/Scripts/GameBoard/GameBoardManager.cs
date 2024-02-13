using Common;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay
{
    public class GameBoardManager : Singleton<GameBoardManager>, IManager
    {
        [SerializeField] GameBoardSettingsSO boardSettings = null;
        public static GameBoardSettingsSO BoardSettings => Instance?.boardSettings;

        [Space]
        [SerializeField] Pooler poolerCell = null;

        private IPlaceable[,] placedObjects = null;

        [HideInInspector] public UnityEvent<IPlaceable, IEnumerable<BoardCoordinate>> OnObjectPlaced;
        [HideInInspector] public UnityEvent<IPlaceable, BoardCoordinate> OnPlaceObjectUpdated;

        public void InitManager()
        {
            InitPlacedObjects();
            SpawnBoardCells();
        }

        public Vector2 GetWorldPositionFromCoordinate(BoardCoordinate coordinate) 
        {
            return new Vector2(coordinate.x * BoardSettings.CellSize, coordinate.y * BoardSettings.CellSize);
        }

        public bool IsCoordinateInBoardBounds(BoardCoordinate coordinate) 
        {
            return BoardCoordinate.IsValid(coordinate)
                && coordinate.x < BoardSettings.BoardSize.x
                && coordinate.y < BoardSettings.BoardSize.y;
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

            if (placedObjects.Length < 1)
                return true;

            return placedObjects[coordinate.x, coordinate.y] == null;
        }

        public void OnBuildingPlaced(IPlaceable placedObject, IEnumerable<BoardCoordinate> coordinates) 
        {
            PlaceObject(placedObject, coordinates);
        }

        public void OnBuildingDestroyed(IPlaceable removedObject, IEnumerable<BoardCoordinate> coordinates) 
        {
            foreach (BoardCoordinate coordinate in coordinates)
            {
                RemovePlacedObject(removedObject, coordinate);
            }
        }

        public void OnUnitDestroyed(IPlaceable removedObject, IEnumerable<BoardCoordinate> coordinates) 
        {
            foreach (BoardCoordinate coordinate in coordinates)
            {
                RemovePlacedObject(removedObject, coordinate);
            }
        }

        public void OnUnitPlaced(IPlaceable placedObject, IEnumerable<BoardCoordinate> coordinates) 
        {
            PlaceObject(placedObject, coordinates);
        }

        public BoardCoordinate GetCoordinateFromWorldPosition(Vector3 worldPosition) 
        {
            BoardCoordinate coordinate = new BoardCoordinate();

            int fixedX = Mathf.CeilToInt(worldPosition.x);
            int fixedY = Mathf.CeilToInt(worldPosition.y);

            coordinate.x = fixedX < 0 ? -1 : Mathf.RoundToInt(fixedX / BoardSettings.CellSize);
            coordinate.y = fixedY < 0 ? -1 : Mathf.RoundToInt(fixedY / BoardSettings.CellSize);

            return coordinate;
        }

        private void PlaceObject(IPlaceable placedObject, IEnumerable<BoardCoordinate> coordinates) 
        {
            foreach (BoardCoordinate coordinate in coordinates)
                AddPlacedObject(placedObject, coordinate);

            OnObjectPlaced?.Invoke(placedObject, coordinates);
        }

        public IPlaceable GetPlacedObject(BoardCoordinate coordinate) 
        {
            if (placedObjects == null || placedObjects.Length < 1)
                return null;

            return placedObjects[coordinate.x, coordinate.y];
        }  

        public bool UpdatePlaceableCoordinate(BoardCoordinate oldCoordinate, BoardCoordinate newCoordinate) 
        {
            IPlaceable placeable = placedObjects[oldCoordinate.x, oldCoordinate.y];

            if (placeable == null) 
            {
                Debug.LogError("GameBoardManager: UpdatePlaceableCoordinate: No placeable on old coordinate: " + oldCoordinate);
                return false;
            }

            if (placedObjects[newCoordinate.x, newCoordinate.y] != null)
            {
                Debug.LogError("GameBoardManager: UpdatePlaceableCoordinate: New coordinate is not empty: " + newCoordinate);
                return false;
            }

            RemovePlacedObject(placeable, oldCoordinate);
            AddPlacedObject(placeable, newCoordinate);
            OnPlaceObjectUpdated?.Invoke(placeable, newCoordinate);
            return true;
        }

        private void InitPlacedObjects() 
        {
            placedObjects = new IPlaceable[boardSettings.BoardSize.x, boardSettings.BoardSize.y];
        }

        private void AddPlacedObject(IPlaceable placedObject, BoardCoordinate coordinate)
        {
            if (placedObjects == null)
                InitPlacedObjects();

            if (placedObjects[coordinate.x, coordinate.y] != null) 
            {
                Debug.LogError("GameBoardManager: AddPlaceObject: contains key:" + coordinate);
                return;
            }

            placedObjects[coordinate.x, coordinate.y] = placedObject;
            Pathfinder.Instance.UpdatePathNodeState(coordinate.x, coordinate.y, false);
        }

        private void RemovePlacedObject(IPlaceable removedObject, BoardCoordinate coordinate) 
        {
            if (placedObjects?.Length < 1)
                return;

            if (placedObjects[coordinate.x, coordinate.y] == null)
            {
                Debug.LogError("GameBoardManager: RemovePlacedObject: no placed object" + coordinate);
                return;
            }

            if (placedObjects[coordinate.x, coordinate.y] != removedObject)
                return;

            placedObjects[coordinate.x, coordinate.y] = null;
            Pathfinder.Instance.UpdatePathNodeState(coordinate.x, coordinate.y, true);
        }

        private void SpawnBoardCells() 
        {
            poolerCell.poolCount = boardSettings.BoardSize.x * boardSettings.BoardSize.y;

            for (int y = 0; y < boardSettings.BoardSize.y; y++)
            {
                for (int x = 0; x < boardSettings.BoardSize.x; x++)
                {
                    /*
                    Vector2 pos = new Vector2(x, y) * boardSettings.CellSize;
                    GameObject cell = Instantiate(goCell, pos, Quaternion.identity);
                    cell.name = $"Cell_({x},{y})";

                    SpriteRenderer rend = cell.GetComponentInChildren<SpriteRenderer>();
                    rend.color = (x + y) % 2 == 0 ? UnityEngine.Color.white : UnityEngine.Color.black;
                    */

                    GameObject goCell = poolerCell.GetGo();

                    if (goCell == null)
                        break;

                    goCell.SetActive(true);
                    Vector2 position = new Vector2(x, y) * boardSettings.CellSize;
                    goCell.transform.position = position;

                    goCell.name = $"Cell_({x},{y})";
                    SpriteRenderer rend = goCell.GetComponentInChildren<SpriteRenderer>();
                    rend.color = (x + y) % 2 == 0 ? UnityEngine.Color.white : UnityEngine.Color.black;
                }
            }
        }

        public IEnumerable<BoardCoordinate> GetArea(BoardCoordinate start, BoardCoordinate end, bool checkPlaceable) 
        {
            BoardCoordinate coordinate = BoardCoordinate.Invalid;

            for (int x = start.x; x <= end.x; x++)
            {
                for (int y = start.y; y <= end.y; y++)
                {
                    coordinate.x = x;
                    coordinate.y = y;

                    if (!IsCoordinateInBoardBounds(coordinate))
                        continue;

                    if (!checkPlaceable)
                    {
                        yield return new BoardCoordinate(x, y);
                    }

                    if (!IsCoordinatePlaceable(coordinate))
                        continue;

                    yield return new BoardCoordinate(x, y);
                }
            }
        }

        public BoardCoordinate GetClosestCoordinateFromArea(IEnumerable<BoardCoordinate> coordinates, bool checkPlaceable) 
        {
            int minX = int.MaxValue;
            int minY = int.MaxValue;
            int maxX = int.MinValue;
            int maxY = int.MinValue;

            foreach (BoardCoordinate coordinate in coordinates)
            {
                minX = Mathf.Min(minX, coordinate.x);
                maxX = Mathf.Max(maxX, coordinate.x);
                minY = Mathf.Min(minY, coordinate.y);
                maxY = Mathf.Max(maxY, coordinate.y);
            }

            BoardCoordinate start = new BoardCoordinate(minX, minY);
            BoardCoordinate end = new BoardCoordinate(maxX, maxY);

            start += new BoardCoordinate(-1, -1);
            end += new BoardCoordinate(1, 1);

            BoardCoordinate[] area = GetArea(start, end, true).ToArray();

            if (area == null || area.Length < 1)
                return BoardCoordinate.Invalid;

            int distance = int.MaxValue;
            BoardCoordinate result = area[0];

            for (int i = 1; i < area.Length; i++)
            {
                BoardCoordinate c1 = area[i];

                int dist = BoardCoordinate.Distance(c1, result);

                if (dist >= distance)
                    continue;

                distance = dist;
                result = c1;
            }

            return result;
        }
    }
}