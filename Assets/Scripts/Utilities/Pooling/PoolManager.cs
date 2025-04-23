using System.Collections.Generic;

using Core.InstanceSystem;

using UnityEngine;

namespace Utilities.Pooling
{
    public class PoolManager : MonoBehaviour
    {
        public static PoolManager Instance => Instanced<PoolManager>.Instance;

        private Dictionary<string, object> _pools = new();

        public void CreatePool<T>(T prefab, int size = 10, Transform parent = null) where T : MonoBehaviour
        {
            string key = GetPoolKey(prefab);

            if (_pools.ContainsKey(key))
                return;

            if (parent == null)
            {
                var containerObj = new GameObject($"{key}_Container");
                containerObj.transform.SetParent(transform, false);
                parent = containerObj.transform;
            }

            var pool = new Pool<T>(prefab, size, parent);
            _pools[key] = pool;
        }

        public T Get<T>(T prefab, Vector3 position, Quaternion rotation) where T : MonoBehaviour
        {
            string key = GetPoolKey(prefab);

            if (!_pools.ContainsKey(key))
                CreatePool(prefab, 1); // auto-create with default 1 if missing

            return ((Pool<T>)_pools[key]).Get(position, rotation);
        }

        public void Return<T>(T obj) where T : MonoBehaviour
        {
            string key = typeof(T).Name;

            if (_pools.TryGetValue(key, out var pool))
            {
                ((Pool<T>)pool).ReturnToPool(obj);
            }
            else
            {
                Debug.LogWarning($"[PoolManager] Returning object of type {key} which has no pool. Destroying it.");
                Destroy(obj.gameObject);
            }
        }
        public bool HasPool<T>() where T : MonoBehaviour
        {
            return _pools.ContainsKey(typeof(T).Name);
        }
        private string GetPoolKey<T>(T prefab) where T : MonoBehaviour
        {
            return typeof(T).Name + "_" + prefab.name;
        }
    }
}
