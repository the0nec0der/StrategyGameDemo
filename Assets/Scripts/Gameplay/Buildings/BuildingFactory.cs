using Core.InstanceSystem;

using UnityEngine;

using Utilities.Pooling;

namespace Gameplay.Buildings
{
    public class BuildingFactory : MonoBehaviour
    {
        public static BuildingFactory Instance => Instanced<BuildingFactory>.Instance;

        public void PrepareBuildingPool(BuildingData buildingData, int initialCount = 5)
        {
            if (buildingData == null || buildingData.Prefab == null)
            {
                Debug.LogWarning("Invalid building data!");
                return;
            }

            var prefab = buildingData.Prefab.GetComponent<BuildingController>();
            PoolManager.Instance.CreatePool(prefab, initialCount);
        }

        public BuildingController CreateBuilding(BuildingData buildingData, Vector3 position)
        {
            var prefab = buildingData.Prefab.GetComponent<BuildingController>();
            var building = PoolManager.Instance.Get<BuildingController>(position, Quaternion.identity);
            building.Initialize(buildingData);
            return building;
        }
    }
}
