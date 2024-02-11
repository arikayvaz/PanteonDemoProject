using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Gameplay
{
    public class UnitProductionSelectItem : MonoBehaviour
    {
        [SerializeField] Image imageUnit = null;

        private UnitTypes unitType = UnitTypes.None;
        private UnityEvent<UnitTypes> onItemClick = null;

        public void InitItem(UnitTypes unitType, Sprite spriteUnit, Color colorUnit, UnityEvent<UnitTypes> onItemClick) 
        {
            this.unitType = unitType;
            this.onItemClick = onItemClick;

            imageUnit.sprite = spriteUnit;
            imageUnit.color = colorUnit;
        }

        public void OnItemClick() 
        {
            onItemClick?.Invoke(unitType);
        }
    }
}