using UnityEngine;
using Common;

namespace Gameplay
{
    public class BuildingSpawnController : MonoBehaviour, IController
    {
        [SerializeField] BuildingSpawnData[] spawnDatas = null;

        public void InitController()
        {
            
        }

        public BuildingControllerBase SpawnBuildingForPicking(BuildingTypes buildingType) 
        {
            return SpawnBuilding(buildingType, new BoardCoordinate());
        }

        public BuildingControllerBase SpawnBuilding(BuildingTypes buildingType, BoardCoordinate coordinate) 
        {
            BuildingSpawnData spawnData = GetBuildingSpawnData(buildingType);

            if (spawnData == null)
                return null;

            BuildingControllerBase building = spawnData.Pooler.GetGo<BuildingControllerBase>();

            if (building == null)
                return null;

            building.InitController(spawnData.BuildingData, coordinate);
            building.gameObject.SetActive(true);

            return building;
        }

        private BuildingSpawnData GetBuildingSpawnData(BuildingTypes buildingType) 
        {
            if (spawnDatas == null || spawnDatas.Length < 1)
                return null;

            foreach (BuildingSpawnData data in spawnDatas)
            {
                if (data.BuildingType != buildingType)
                    continue;

                return data;
            }

            return null;
        }
    }
}