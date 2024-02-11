using UnityEngine;

namespace Gameplay
{
    public class BoardVisual : MonoBehaviour
    {
        [SerializeField] SpriteRenderer rendererVisual = null;

        public void InitVisual(Sprite spriteVisual, Color spriteColor, int boardSizeX, int boardSizeY, int cellSize) 
        {
            SetVisualSprite(spriteVisual, spriteColor);
            SetVisualSize(boardSizeX, boardSizeY, cellSize);
        }

        public void UpdateColor(Color color) 
        {
            rendererVisual.color = color;
        }

        public void UpdateSortingOrder(int order) 
        {
            rendererVisual.sortingOrder = order;
        }

        private void SetVisualSize(int boardSizeX, int boardSizeY, int cellSize) 
        {
            float width = boardSizeX * cellSize;
            float height = boardSizeY * cellSize;

            transform.localScale = new Vector3(width, height, 1f);
            transform.position = new Vector3(width / 2f, height / 2f, 0f);
        }

        private void SetVisualSprite(Sprite spriteVisual, Color spriteColor) 
        {
            rendererVisual.sprite = spriteVisual;
            UpdateColor(spriteColor);
        }
    }
}