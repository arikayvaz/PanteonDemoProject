using UnityEngine;
using Common;

namespace Gameplay
{
    public class UnitSpawnController : MonoBehaviour, IController
    {
        [SerializeField] UnitSpawnData[] spawnDatas = null;

        public void InitController()
        {
            
        }

        public UnitControllerBase SpawnUnitForPicking(UnitTypes unitType)
        {
            return SpawnUnit(unitType, BoardCoordinate.Invalid);
        }

        public UnitControllerBase SpawnUnit(UnitTypes unitType, BoardCoordinate coordinate)
        {
            UnitSpawnData spawnData = GetUnitSpawnData(unitType);

            if (spawnData == null)
                return null;

            UnitControllerBase unit = spawnData.Pooler.GetGo<UnitControllerBase>();

            if (unit == null)
                return null;

            unit.InitController(spawnData.UnitData, coordinate);
            unit.gameObject.SetActive(true);

            return unit;
        }

        private UnitSpawnData GetUnitSpawnData(UnitTypes unitType)
        {
            if (spawnDatas == null || spawnDatas.Length < 1)
                return null;

            foreach (UnitSpawnData data in spawnDatas)
            {
                if (data.UnitType != unitType)
                    continue;

                return data;
            }

            return null;
        }
    }
}