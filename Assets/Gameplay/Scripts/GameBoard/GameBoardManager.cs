using Common;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay
{
    public class GameBoardManager : Singleton<GameBoardManager>, IManager
    {
        [SerializeField] GameBoardSettingsSO boardSettings = null;
        public static GameBoardSettingsSO BoardSettings => Instance?.boardSettings;

        [Space]
        [SerializeField] GameObject goCell = null;

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

            RemovePlacedObject(oldCoordinate);
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

        private void RemovePlacedObject(BoardCoordinate coordinate) 
        {
            if (placedObjects?.Length < 1)
                return;

            if (placedObjects[coordinate.x, coordinate.y] == null)
            {
                Debug.LogError("GameBoardManager: RemovePlacedObject: no placed object" + coordinate);
                return;
            }

            placedObjects[coordinate.x, coordinate.y] = null;
            Pathfinder.Instance.UpdatePathNodeState(coordinate.x, coordinate.y, true);
        }

        private void SpawnBoardCells() 
        {
            for (int y = 0; y < boardSettings.BoardSize.y; y++)
            {
                for (int x = 0; x < boardSettings.BoardSize.x; x++)
                {
                    Vector2 pos = new Vector2(x, y) * boardSettings.CellSize;
                    GameObject cell = Instantiate(goCell, pos, Quaternion.identity);
                    cell.name = $"Cell_({x},{y})";

                    SpriteRenderer rend = cell.GetComponentInChildren<SpriteRenderer>();
                    rend.color = (x + y) % 2 == 0 ? UnityEngine.Color.white : UnityEngine.Color.black;
                }
            }
        }
    }
}