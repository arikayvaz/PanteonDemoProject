using Common;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay
{
    public class BuildingProductionUIController : Singleton<BuildingProductionUIController>, IController
    {
        [SerializeField] BuildingProductionSelectItem[] items = null;

        private UnityEvent<BuildingTypes> onItemClick;

        public void InitController() 
        {
            BuildingDataSO[] datas = BuildingManager.Instance.GetBuildingDatas().ToArray();

            if (datas == null)
                return;

            onItemClick = new UnityEvent<BuildingTypes>();
            onItemClick.AddListener(OnItemClicked);

            for (int i = 0; i < items.Length; i++)
            {
                BuildingProductionSelectItem item = items[i];

                if (i > datas.Length - 1) 
                {
                    item.gameObject.SetActive(false);
                    continue;
                }

                BuildingDataSO data = datas[i];

                item.InitItem(data.BuildingType, data.BuildingName, data.SpriteBuilding, data.BuildingColor, onItemClick);
            }
        }

        private void OnItemClicked(BuildingTypes buildingType) 
        {
            BuildingManager.Instance.PickBuilding(buildingType);
        }

    }
}