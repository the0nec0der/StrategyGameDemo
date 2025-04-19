using Core.InstanceSystem;

using UnityEngine;

using Utilities.Pooling;

namespace Gameplay.SoldierUnits
{
    public class SoldierFactory : MonoBehaviour
    {
        public static SoldierFactory Instance => Instanced<SoldierFactory>.Instance;

        public void PrepareSoldierPool(SoldierData soldierData, int initialCount = 5)
        {
            if (soldierData == null || soldierData.Prefab == null)
            {
                Debug.LogWarning("Invalid soldier data!");
                return;
            }

            var prefab = soldierData.Prefab.GetComponent<SoldierController>();
            if (prefab == null)
            {
                Debug.LogError("Soldier prefab does not have a SoldierController!");
                return;
            }

            PoolManager.Instance.CreatePool(prefab, initialCount);
        }

        public SoldierController CreateSoldier(SoldierData data, Vector3 position)
        {
            var soldier = PoolManager.Instance.Get<SoldierController>(position, Quaternion.identity);
            soldier.Initialize(data);
            return soldier;
        }
    }
}
