using UnityEngine;

namespace Gameplay
{
    public class UnitViewModel
    {
        private UnitModel model = null;

        public string Name => model.Name;
        public UnitTypes UnitType => model.UnitType;
        public int CellSizeX => model.CellSizeX;
        public int CellSizeY => model.CellSizeY;
        public BoardCoordinate Coordinate => model.coordinate;
        public Sprite SpriteUnit => model.SpriteUnit;
        public Color UnitColor => model.UnitColor;
        public int MoveSpeed => model.MoveSpeed;
        public Color ColorSelected => model.StateColorData.ColorSelected;
        public Color ColorPlaceable => model.StateColorData.ColorPlaceable;
        public Color ColorUnplaceable => model.StateColorData.ColorUnPlaceable;
        public int AttackDamage => model.AttackDamage;
        public float AttackDelay => model.AttackDelay;
        public int Health => model.health;

        public UnitViewModel(UnitModel model)
        {
            this.model = model;
        }

        public void UpdateCoordinate(BoardCoordinate coordinate)
        {
            model.coordinate = coordinate;
        }

        public void SetHealth(int health) 
        {
            model.health = health;
        }

        public void AddHealth(int healthDelta) 
        {
            model.health += healthDelta;

            if (model.health < 0)
                model.health = 0;
        }
    }
}