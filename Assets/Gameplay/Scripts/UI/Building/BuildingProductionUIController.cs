using Common;
using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay
{
    public class BuildingProductionUIController : MonoBehaviour
    {
        [SerializeField] InfiniteScrollView scrollView = null;
        [SerializeField] BuildingProductionSelectItem selectItem = null;

        private UnityEvent<BuildingTypes> onItemClick;
        private UnityEvent<BuildingTypes> onItemProduced;

        public void InitController(UnityEvent<BuildingTypes> onItemProduced) 
        {
            InitItems();

            this.onItemProduced = onItemProduced;
        }

        private void InitItems() 
        {
            BuildingDataSO[] datas = BuildingManager.Instance.GetBuildingDatas().ToArray();

            if (datas == null)
                return;

            onItemClick = new UnityEvent<BuildingTypes>();
            onItemClick.AddListener(OnItemClicked);

            scrollView.InitScrollView(selectItem.gameObject, datas.Length, (spawnedItems) => 
            {
                int index = 0;
                foreach (GameObject goSpawnedItem in spawnedItems)
                {
                    BuildingProductionSelectItem item = goSpawnedItem.GetComponent<BuildingProductionSelectItem>();

                    if (item == null)
                        continue;

                    BuildingDataSO data = datas[index];

                    item.InitItem(data.BuildingType, data.BuildingName, data.SpriteBuilding, data.BuildingColor, onItemClick);
                    item.gameObject.name = "BPSI_" + data.BuildingName;

                    index++;
                }
            });
        }

        private void OnItemClicked(BuildingTypes buildingType) 
        {
            BuildingManager.Instance.PickBuilding(buildingType);
            onItemProduced?.Invoke(buildingType);
        }

    }
}