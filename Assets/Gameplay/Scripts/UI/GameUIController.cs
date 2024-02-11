using Common;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class GameUIController : Singleton<GameUIController>, IController
    {
        [SerializeField] BuildingProductionUIController buildingProductionController = null;
        [SerializeField] BuildingInformationUIController buildingInformationController = null;
        [SerializeField] UnitProductionUIController unitProductionController = null;

        [Space]
        [SerializeField] RectTransform rectTrLeftSide = null;
        [SerializeField] RectTransform rectTrRightSide = null;

        public float GameViewStartPct => rectTrLeftSide.anchorMax.x;
        public float GameViewEndPct => rectTrRightSide.anchorMin.x;

        public void InitController() 
        {
            buildingProductionController.InitController();
            buildingInformationController.InitController();
            unitProductionController.InitController();
        }

        public void ShowBuildingInformationPanel(string buildingName, Sprite spriteBuilding, Color colorBuilding) 
        {
            buildingInformationController.ShowInformationPanel(buildingName, spriteBuilding, colorBuilding);
        }

        public void HideBuildingInformationPanel() 
        {
            buildingInformationController.HideInformationPanel();
        }

        public void ShowProducibleUnitInformationPanel(IEnumerable<UnitDataSO> producibleUnitDatas) 
        {
            unitProductionController.ShowProduction(producibleUnitDatas);
        }

        public void HideProducibleUnitInformationPanel() 
        {
            unitProductionController.HideProduction();
        }
    }
}