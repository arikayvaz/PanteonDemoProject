using Common;
using UnityEngine;

namespace Gameplay
{
    public class CameraController : Singleton<CameraController>, IController
    {
        private Camera cameraMain;

        public void InitController()
        {
            cameraMain = Camera.main;

            InitCamera(GameBoardManager.BoardSettings.BoardSize.x
                , GameBoardManager.BoardSettings.BoardSize.y
                , GameBoardManager.BoardSettings.CellSize
                , GameUIController.Instance.GameViewStartPct
                , GameUIController.Instance.GameViewEndPct);
        }

        public Vector2 GetWorldPosition(Vector2 screenPosition) 
        {
            return cameraMain.ScreenToWorldPoint(screenPosition);
        }

        private void InitCamera(int boardSizeX, int boardSizeY, int cellSize, float gameViewStartPct, float gameViewEndPct) 
        {
            float ortographicSizeVertical = CalculateVerticalOrthographicSize(boardSizeX, cellSize, gameViewStartPct, gameViewEndPct);
            float ortographicSizeHorizontal = CalculateHorizontalOrthographicSize(boardSizeY, cellSize);

            float ortographicSize = Mathf.Max(ortographicSizeVertical, ortographicSizeHorizontal);
            Vector2 position = CalculateCameraPosition(boardSizeX, boardSizeY, cellSize, gameViewStartPct, gameViewEndPct);

            AdjustCamera(ortographicSize, position);
        }

        private void AdjustCamera(float orthographicSize, Vector2 position) 
        {
            cameraMain.orthographicSize = orthographicSize;

            cameraMain.transform.position = new Vector3(position.x, position.y, cameraMain.transform.position.z);

        }

        private float CalculateHorizontalOrthographicSize(int boardSizeY, int cellSize) 
        {
            return boardSizeY * 0.5f * cellSize;
        }

        private float CalculateVerticalOrthographicSize(int boardSizeX, int cellSize, float gameViewStartPct, float gameViewEndPct) 
        {
            float gameView = 1f - gameViewStartPct - (1f - gameViewEndPct);
            float boardWidth = boardSizeX * cellSize;

            return (boardWidth * Screen.height / Screen.width * 0.5f) * (1f / gameView);
        }

        private Vector2 CalculateCameraPosition(int boardSizeX, int boardSizeY, int cellSize, float gameViewStartPct, float gameViewEndPct) 
        {
            float posX = boardSizeX * cellSize * 0.5f;
            float posY = boardSizeY * cellSize * 0.5f;

            posX += (1f - gameViewEndPct - gameViewStartPct) * (boardSizeX * cellSize);

            return new Vector2(posX, posY);
        }
    }
}