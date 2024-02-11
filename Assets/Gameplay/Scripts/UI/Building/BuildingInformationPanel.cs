using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
    public class BuildingInformationPanel : MonoBehaviour
    {
        [SerializeField] GameObject goPanelRoot = null;
        [SerializeField] TMP_Text textBuildingName = null;
        [SerializeField] Image imageBuilding = null;

        public void InitPanel(string buildingName, Sprite spriteBuilding, Color colorBuilding) 
        {
            textBuildingName.text = buildingName;
            imageBuilding.sprite = spriteBuilding;
            imageBuilding.color = colorBuilding;
        }

        public void ShowPanel(string buildingName, Sprite spriteBuilding, Color colorBuilding) 
        {
            InitPanel(buildingName, spriteBuilding, colorBuilding);
            goPanelRoot.SetActive(true);
        }

        public void HidePanel() 
        {
            goPanelRoot.SetActive(false);
        }
    }
}