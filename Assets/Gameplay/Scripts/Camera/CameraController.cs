using Common;
using UnityEngine;

namespace Gameplay
{
    public class CameraController : Singleton<CameraController>, IController
    {
        Camera camera;

        public void InitController()
        {
            camera = Camera.main;
            AdjustCamera(GameBoardManager.BoardSettings.boardSize, GameBoardManager.BoardSettings.cellSize);
        }

        public Vector2 GetWorldPosition(Vector2 screenPosition) 
        {
            return camera.ScreenToWorldPoint(screenPosition);
        }

        private void AdjustCamera(Vector2Int boardSize, int cellSize) 
        {
            float boardOrthographicSize = (cellSize * 0.5f) * boardSize.y;
            camera.orthographicSize = boardOrthographicSize;

            camera.transform.position = new Vector3(boardOrthographicSize
                , boardOrthographicSize
                , camera.transform.position.z);
        }
    }
}