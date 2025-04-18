using System.Collections.Generic;

using UnityEngine;

namespace Utilities.Pooling
{
    public class Pool<T> where T : MonoBehaviour
    {
        private readonly Queue<T> _pool = new Queue<T>();
        private readonly T _prefab;
        private readonly Transform _parent;

        public Pool(T prefab, int initialSize, Transform parent)
        {
            _prefab = prefab;
            _parent = parent;

            for (int i = 0; i < initialSize; i++)
            {
                AddToPool(CreateInstance());
            }
        }

        private T CreateInstance()
        {
            var obj = Object.Instantiate(_prefab, _parent);
            obj.gameObject.SetActive(false);
            return obj;
        }

        private void AddToPool(T obj)
        {
            _pool.Enqueue(obj);
        }

        public T Get(Vector3 position, Quaternion rotation)
        {
            T obj = _pool.Count > 0 ? _pool.Dequeue() : CreateInstance();
            obj.transform.SetPositionAndRotation(position, rotation);
            obj.gameObject.SetActive(true);

            if (obj is IPoolable poolable)
                poolable.OnSpawnFromPool();

            return obj;
        }

        public void ReturnToPool(T obj)
        {
            if (obj is IPoolable poolable)
                poolable.OnReturnToPool();

            obj.gameObject.SetActive(false);
            AddToPool(obj);
        }
    }
}
