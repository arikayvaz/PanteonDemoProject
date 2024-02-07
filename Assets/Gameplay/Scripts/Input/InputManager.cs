using Common;
using System;
using UnityEngine;

namespace Gameplay
{
    public class InputManager : Singleton<InputManager>
    {
        public Vector2 WorldPosition { get; private set; } = Vector2.zero;
        private Vector2 lastInputPosition = Vector2.zero;
        public GameBoardCoordinates CurrentInputCoordinate { get; private set; } = GameBoardCoordinates.Invalid;
        public Action<GameBoardCoordinates> OnInputCoordinateChange;

        private void Update()
        {
            CalculateWorldPosition(Input.mousePosition);

            if (Input.GetMouseButtonDown(0))
            {
                GameBoardCoordinates coord = GetCoordinateFromInputPosition(Input.mousePosition);
            }
        }

        private GameBoardCoordinates GetCoordinateFromInputPosition(Vector2 inputPos) 
        {
            Vector2 worldPos = CameraController.Instance.GetWorldPosition(inputPos);

            GameBoardCoordinates coord = new GameBoardCoordinates();

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
            GameBoardCoordinates calculatedCoord = GameBoardManager.Instance.GetCoordinateFromWorldPosition(worldPos);

            if (calculatedCoord == CurrentInputCoordinate)
                return;

            CurrentInputCoordinate = calculatedCoord;
            OnInputCoordinateChange?.Invoke(CurrentInputCoordinate);
        }
    }
}