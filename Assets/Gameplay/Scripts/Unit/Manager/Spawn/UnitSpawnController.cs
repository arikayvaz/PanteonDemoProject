using UnityEngine;
using Common;

namespace Gameplay
{
    public class UnitSpawnController : MonoBehaviour, IController
    {
        [SerializeField] UnitSpawnModel[] spawnModels = null;

        public void InitController()
        {
            
        }

        public UnitController SpawnUnitForPicking(UnitModel model)
        {
            return SpawnUnit(model, BoardCoordinate.Invalid);
        }

        public UnitController SpawnUnit(UnitModel model, BoardCoordinate coordinate)
        {
            UnitSpawnModel spawnData = GetUnitSpawnModel(model.UnitType);

            if (spawnData == null)
                return null;

            UnitController unit = spawnData.Pooler.GetGo<UnitController>();

            if (unit == null)
                return null;

            unit.InitController(model, coordinate);
            unit.gameObject.SetActive(true);

            return unit;
        }

        private UnitSpawnModel GetUnitSpawnModel(UnitTypes unitType)
        {
            if (spawnModels == null || spawnModels.Length < 1)
                return null;

            foreach (UnitSpawnModel model in spawnModels)
            {
                if (model.UnitType != unitType)
                    continue;

                return model;
            }

            return null;
        }
    }
}