using System.Collections.Generic;

using Core.InstanceSystem;

using UnityEngine;

namespace Utilities.Pooling
{
    public class PoolManager : MonoBehaviour
    {
        public static PoolManager Instance => Instanced<PoolManager>.Instance;

        private Dictionary<string, object> _pools = new Dictionary<string, object>();

        public void CreatePool<T>(T prefab, int size = 10) where T : MonoBehaviour
        {
            string key = typeof(T).Name;
            if (!_pools.ContainsKey(key))
            {
                var pool = new Pool<T>(prefab, size, InstanceParent.Transform);
                _pools[key] = pool;
            }
        }

        public T Get<T>(Vector3 position, Quaternion rotation) where T : MonoBehaviour
        {
            string key = typeof(T).Name;
            if (_pools.TryGetValue(key, out var pool))
            {
                return ((Pool<T>)pool).Get(position, rotation);
            }

            Debug.LogError($"No pool found for type {key}");
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
                Debug.LogWarning($"Returning object of type {key} which has no pool.");
                Object.Destroy(obj.gameObject);
            }
        }
    }
}
