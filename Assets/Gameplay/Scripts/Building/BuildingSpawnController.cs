using UnityEngine;
using Common;
using System.Collections.Generic;

namespace Gameplay
{
    public class BuildingSpawnController : MonoBehaviour, IController
    {
        [SerializeField] BuildingSpawnModel[] spawnModels = null;

        public void InitController()
        {
            
        }

        public BuildingController SpawnBuildingForPicking(BuildingModel model) 
        {
            return SpawnBuilding(model, BoardCoordinate.Invalid);
        }

        public BuildingController SpawnBuilding(BuildingModel model, BoardCoordinate coordinate) 
        {
            BuildingSpawnModel spawnModel = GetBuildingSpawnModel(model.BuildingType);

            if (spawnModel == null)
                return null;

            BuildingController building = spawnModel.Pooler.GetGo<BuildingController>();

            if (building == null)
                return null;

            building.InitController(model, coordinate);
            building.gameObject.SetActive(true);

            return building;
        }

        public IEnumerable<BuildingSpawnModel> GetSpawnModels() 
        {
            foreach (BuildingSpawnModel model in spawnModels)
                yield return model;

        }

        private BuildingSpawnModel GetBuildingSpawnModel(BuildingTypes buildingType) 
        {
            if (spawnModels == null || spawnModels.Length < 1)
                return null;

            foreach (BuildingSpawnModel model in spawnModels)
            {
                if (model.BuildingType != buildingType)
                    continue;

                return model;
            }

            return null;
        }
    }
}