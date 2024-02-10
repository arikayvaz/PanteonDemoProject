using Common;
using System;
using UnityEngine;

namespace Gameplay
{
    public class InputManager : Singleton<InputManager>, IManager
    {
        public Vector2 WorldPosition { get; private set; } = Vector2.zero;
        private Vector2 lastInputPosition = Vector2.zero;
        public BoardCoordinate CurrentInputCoordinate { get; private set; } = BoardCoordinate.Invalid;
        public Action<BoardCoordinate> OnInputCoordinateChange;

        private bool isInitialized = false;

        public void InitManager() 
        {
            isInitialized = true;
        }

        private void Update()
        {
            if (!isInitialized)
                return;

            CalculateWorldPosition(Input.mousePosition);

            if (Input.GetMouseButtonDown(0))
            {
                BoardCoordinate coord = GetCoordinateFromInputPosition(Input.mousePosition);

                HandleLeftClickInput(coord);
                return;
            }
        }

        private void HandleLeftClickInput(BoardCoordinate coordinate) 
        {
            bool isHandled = BuildingManager.Instance.HandleLeftClickInput(coordinate);

            if (isHandled)
                return;
        }

        private BoardCoordinate GetCoordinateFromInputPosition(Vector2 inputPos) 
        {
            Vector2 worldPos = CameraController.Instance.GetWorldPosition(inputPos);

            BoardCoordinate coord = new BoardCoordinate();

            int fixedX = Mathf.CeilToInt(worldPos.x);
            int fixedY = Mathf.CeilToInt(worldPos.y);

            coord.x = Mathf.RoundToInt(fixedX / GameBoardManager.BoardSettings.cellSize);
            coord.y = Mathf.RoundToInt(fixedY / GameBoardManager.BoardSettings.cellSize);

            return coord;
        }

        private void CalculateWorldPosition(Vector2 inputPos) 
        {
            const float UPDATE_POSITION_DISTANCE_THRESHOLD = 0.1f;

            if (Vector2.Distance(inputPos, lastInputPosition) < UPDATE_POSITION_DISTANCE_THRESHOLD)
                return;

            WorldPosition = CameraController.Instance.GetWorldPosition(inputPos);
            lastInputPosition = inputPos;
            CalculateCurrentGameBoardCoordinate(WorldPosition);
        }

        private void CalculateCurrentGameBoardCoordinate(Vector2 worldPos) 
        {
            BoardCoordinate calculatedCoord = GameBoardManager.Instance.GetCoordinateFromWorldPosition(worldPos);

            if (calculatedCoord == CurrentInputCoordinate)
                return;

            CurrentInputCoordinate = calculatedCoord;
            OnInputCoordinateChange?.Invoke(CurrentInputCoordinate);
        }
    }
}