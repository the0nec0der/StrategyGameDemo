using Core.InstanceSystem;

using UnityEngine;

using Utilities.Pooling;

namespace Gameplay.Buildings
{
    public class BuildingFactory : MonoBehaviour
    {
        public void PrepareBuildingPool(IBuilding buildingData, int initialCount = 5)
        {
            if (!IsValid(buildingData)) return;

            var prefab = buildingData.Prefab.GetComponent<BuildingController>();
            PoolManager.Instance.CreatePool(prefab, initialCount, transform);
        }

        public BuildingController CreateBuilding(IBuilding buildingData, Vector3 position)
        {
            if (!IsValid(buildingData)) return null;

            var prefab = buildingData.Prefab.GetComponent<BuildingController>() ?? buildingData.Prefab.AddComponent<BuildingController>();

            if (!PoolManager.Instance.HasPool<BuildingController>())
            {
                PoolManager.Instance.CreatePool(prefab, 1, transform);
            }

            var building = PoolManager.Instance.Get(prefab, position, Quaternion.identity);
            building.transform.SetParent(transform);
            building.Initialize(buildingData);
            return building;
        }

        private bool IsValid(IBuilding buildingData)
        {
            if (buildingData == null || buildingData.Prefab == null)
            {
                Debug.LogWarning("Invalid building data!");
                return false;
            }
            return true;
        }
    }
}
