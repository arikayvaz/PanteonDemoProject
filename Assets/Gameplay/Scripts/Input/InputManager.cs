using Common;
using UnityEngine;

namespace Gameplay
{
    public class InputManager : Singleton<InputManager>
    {
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                GameBoardCoordinates coord = GetCoordinateFromInputPosition(Input.mousePosition);
            }
        }

        private GameBoardCoordinates GetCoordinateFromInputPosition(Vector2 inputPos) 
        {
            Vector3 worldPos = CameraController.Instance.GetWorldPosition(inputPos);

            GameBoardCoordinates coord = new GameBoardCoordinates();

            int fixedX = Mathf.CeilToInt(worldPos.x);
            int fixedY = Mathf.CeilToInt(worldPos.y);

            coord.x = Mathf.RoundToInt(fixedX / GameBoardManager.BoardSettings.cellSize);
            coord.y = Mathf.RoundToInt(fixedY / GameBoardManager.BoardSettings.cellSize);

            return coord;
        }
    }
}