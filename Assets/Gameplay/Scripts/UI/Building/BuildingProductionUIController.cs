using Common;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay
{
    public class BuildingProductionUIController : MonoBehaviour, IController
    {
        [SerializeField] InfiniteScrollView scrollView = null;
        [SerializeField] BuildingProductionSelectItem[] items = null;

        private UnityEvent<BuildingTypes> onItemClick;

        public void InitController() 
        {
            InitItems();
            scrollView.InitScrollView();
        }

        private void InitItems() 
        {
            BuildingDataSO[] datas = BuildingManager.Instance.GetBuildingDatas().ToArray();

            if (datas == null)
                return;

            onItemClick = new UnityEvent<BuildingTypes>();
            onItemClick.AddListener(OnItemClicked);

            if (datas.Length != items.Length)
            {
                Debug.LogError("Items and datas size different!");
                return;
            }

            for (int i = 0; i < datas.Length; i++)
            {
                BuildingProductionSelectItem item = items[i];

                if (item == null)
                    continue;

                BuildingDataSO data = datas[i];

                item.InitItem(data.BuildingType, data.BuildingName, data.SpriteBuilding, data.BuildingColor, onItemClick);
                item.gameObject.name = "BPSI_" + data.BuildingName;
            }
        }

        private void OnItemClicked(BuildingTypes buildingType) 
        {
            BuildingManager.Instance.PickBuilding(buildingType);
        }

    }
}