using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Gameplay
{
    public class BuildingProductionSelectItem : MonoBehaviour
    {
        [SerializeField] Image imageIcon = null;
        [SerializeField] TMP_Text textName = null;

        private BuildingTypes buildingType = BuildingTypes.None;
        private UnityEvent<BuildingTypes> onItemClick;

        public void InitItem(BuildingTypes buildingType, string buildingName, Sprite spriteBuilding, Color colorBuilding, UnityEvent<BuildingTypes> onItemClick) 
        {
            this.buildingType = buildingType;
            this.onItemClick = onItemClick;

            textName.text = buildingName;
            imageIcon.sprite = spriteBuilding;
            imageIcon.color = colorBuilding;
        }

        public void OnItemClick() 
        {
            onItemClick?.Invoke(buildingType);
        }
    }
}