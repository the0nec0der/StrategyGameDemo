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
            string key = typeof(T).Name;

            if (_pools.ContainsKey(key))
                return;

            if (parent == null)
            {
                var containerObj = new GameObject($"{key}_Container");
                containerObj.transform.SetParent(transform, false);
                parent = containerObj.transform;
            }

            var pool = new Pool<T>(prefab, size, parent ?? transform);
            _pools[key] = pool;
        }

        public T Get<T>(Vector3 position, Quaternion rotation) where T : MonoBehaviour
        {
            string key = typeof(T).Name;

            if (_pools.TryGetValue(key, out var pool))
                return ((Pool<T>)pool).Get(position, rotation);

            Debug.LogError($"[PoolManager] No pool found for type {key}");
            return null;
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
    }
}
