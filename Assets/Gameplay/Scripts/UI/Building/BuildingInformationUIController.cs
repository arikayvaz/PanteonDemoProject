using Common;
using UnityEngine;

namespace Gameplay
{
    public class BuildingInformationUIController : MonoBehaviour, IController
    {
        [SerializeField] BuildingInformationPanel panel = null; 

        public void InitController() 
        {
            panel.HidePanel();
        }

        public void ShowInformationPanel(string buildingName, Sprite spriteBuilding, Color colorBuilding) 
        {
            panel.ShowPanel(buildingName, spriteBuilding, colorBuilding);
        }

        public void HideInformationPanel() 
        {
            panel.HidePanel();
        }
    }
}