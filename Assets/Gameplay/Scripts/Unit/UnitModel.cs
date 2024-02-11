using UnityEngine;

namespace Gameplay
{
    public class UnitModel
    {
        public UnitTypes UnitType => unitData.UnitType;
        public string Name => unitData.UnitName;
        public int CellSizeX => unitData.CellSizeX;
        public int CellSizeY => unitData.CellSizeY;
        public Sprite SpriteUnit => unitData.SpriteUnit;
        public Color UnitColor => unitData.UnitColor;
        public int MoveSpeed => unitData.MoveSpeed;
        public StateColorDataSO StateColorData => unitData.StateColorData;

        public int health = 0;
        public BoardCoordinate coordinate = BoardCoordinate.Invalid;

        private UnitDataSO unitData;

        public UnitModel(UnitDataSO unitData)
        {
            this.unitData = unitData;

            health = unitData.Health;
        }
    }
}