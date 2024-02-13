using Common;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay
{
    public class UnitProductionUIController : MonoBehaviour, IController
    {
        [SerializeField] Pooler poolerSelectItem = null;
        [SerializeField] Transform trLayout = null;

        [Space]
        [SerializeField] GameObject goPanelRoot = null;

        private UnityEvent<UnitTypes> onItemClick = null;

        private List<UnitProductionSelectItem> items = null;

        public void InitController() 
        {
            items = new List<UnitProductionSelectItem>();

            onItemClick = new UnityEvent<UnitTypes>();
            onItemClick.AddListener(OnItemClick);
        }

        public void ShowProduction(IEnumerable<UnitDataSO> producibleUnitDatas)
        {
            ResetItems();

            InitItems(producibleUnitDatas);

            goPanelRoot.SetActive(true);
        }

        public void HideProduction() 
        {
            ResetItems();

            goPanelRoot.SetActive(false);
        }

        private void OnItemClick(UnitTypes unitType) 
        {
            BoardCoordinate spawnCoordinate = BuildingManager.Instance.GetSelectedBuildingSpawnCoordinate();

            UnitManager.Instance.ProduceUnit(unitType, spawnCoordinate);
        }

        private void InitItems(IEnumerable<UnitDataSO> producibleUnitDatas) 
        {
            foreach (UnitDataSO data in producibleUnitDatas)
            {
                UnitProductionSelectItem item = poolerSelectItem.GetGo<UnitProductionSelectItem>();

                item.InitItem(data.UnitType, data.SpriteUnit, data.UnitColor, onItemClick);

                //item.transform.parent = trLayout;
                item.transform.SetParent(trLayout);
                item.transform.localScale = Vector3.one;

                item.gameObject.SetActive(true);

                items.Add(item);
            }
        }

        private void ResetItems() 
        {
            foreach (UnitProductionSelectItem item in items)
            {
                item.gameObject.SetActive(false);
            }

            items.Clear();
        }
    }
}