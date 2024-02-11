using UnityEngine;

namespace Gameplay
{
    public class SpawnPoint : MonoBehaviour
    {
        [SerializeField] BoardVisual boardVisual = null;
        [SerializeField] SpawnPointSettingsSO settings = null;

        public BoardCoordinate SpawnCoordinate { get; private set; } = BoardCoordinate.Invalid;

        public void InitSpawnPoint(BoardCoordinate spawnCoordinate, int boardSizeX, int boardSizeY, int cellSize) 
        {
            SpawnCoordinate = spawnCoordinate;

            boardVisual.InitVisual(settings.SpriteVisual
                , settings.ColorSpawnPoint
                , boardSizeX
                , boardSizeY
                , cellSize);

            boardVisual.UpdateSortingOrder(GameBoardManager.BoardSettings.BoardSortingOrderSpawnPoint);

            UpdatePosition(spawnCoordinate);
        }

        public void OnPlaced(BoardCoordinate placedCoordinate) 
        {
            SpawnCoordinate = placedCoordinate;

            ResetColor();
            UpdateVisualSortingOrder(GameBoardManager.BoardSettings.BoardSortingOrderSpawnPoint);
            UpdatePosition(SpawnCoordinate);
        }

        public void OnPicked() 
        {
            UpdateVisualSortingOrder(GameBoardManager.BoardSettings.BoardSortingOrderPicked);
        }

        public void UpdateColor(Color colorUpdated)
        {
            boardVisual.UpdateColor(colorUpdated);
        }

        private void ResetColor() 
        {
            boardVisual.UpdateColor(settings.ColorSpawnPoint);
        }

        private void UpdateVisualSortingOrder(int order) 
        {
            boardVisual.UpdateSortingOrder(order);
        }

        private void UpdatePosition(BoardCoordinate coordinate) 
        {
            transform.position = GameBoardManager.Instance.GetWorldPositionFromCoordinate(coordinate);
        }
    }
}