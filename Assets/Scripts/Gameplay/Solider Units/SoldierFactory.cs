using Core.InstanceSystem;

using UnityEngine;

using Utilities.Pooling;

namespace Gameplay.SoldierUnits
{
    public class SoldierFactory : MonoBehaviour
    {
        public static SoldierFactory Instance => Instanced<SoldierFactory>.Instance;

        [SerializeField] private Transform soldierRoot;

        private void Awake()
        {
            if (Instance != this)
                return;

            if (soldierRoot == null)
            {
                soldierRoot = new GameObject("Soldier_Root").transform;
                soldierRoot.SetParent(transform, false);
            }
        }

        public void PrepareSoldierPool(ISoliderUnit data, int initialCount = 5)
        {
            if (!IsValid(data)) return;

            var prefab = data.Prefab.GetComponent<SoldierController>() ?? data.Prefab.AddComponent<SoldierController>();
            PoolManager.Instance.CreatePool(prefab, initialCount, soldierRoot);
        }

        public SoldierController CreateSoldier(ISoliderUnit data, Vector3 position)
        {
            if (!IsValid(data)) return null;

            var prefab = data.Prefab.GetComponent<SoldierController>() ?? data.Prefab.AddComponent<SoldierController>();

            var soldier = PoolManager.Instance.Get(prefab, position, Quaternion.identity);
            soldier.transform.SetParent(soldierRoot, false);
            soldier.Initialize(data);

            return soldier;
        }

        private SoldierController GetOrAddController(ISoliderUnit data)
        {
            var controller = data.Prefab.GetComponent<SoldierController>();
            if (controller == null)
            {
                Debug.LogWarning($"Soldier prefab '{data.Prefab.name}' missing SoldierController. Adding dynamically.");
                controller = data.Prefab.AddComponent<SoldierController>();
            }
            return controller;
        }

        private bool IsValid(ISoliderUnit data)
        {
            if (data == null || data.Prefab == null)
            {
                Debug.LogWarning("Invalid soldier data.");
                return false;
            }
            return true;
        }
    }
}
