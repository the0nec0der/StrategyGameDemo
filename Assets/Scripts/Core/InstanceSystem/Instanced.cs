using UnityEngine;

namespace Core.InstanceSystem
{
    public static class Instanced<T> where T : MonoBehaviour
    {
        private static T _instance = null;

        public static T Instance
        {
            get
            {
#if UNITY_EDITOR
                if (!Application.isPlaying)
                    throw new System.Exception("Game is not running.");
#endif
                if (_instance == null)
                {
                    try
                    {
                        _instance = InstancedPrefab<T>.Instance;
                    }
                    catch
                    {
                        T[] instancesInScene = Object.FindObjectsByType<T>(FindObjectsInactive.Include, FindObjectsSortMode.None);
                        for (int i = instancesInScene.Length - 1; i >= 1; i--)
                            Object.Destroy(instancesInScene[i]);

                        if (instancesInScene.Length > 0)
                            _instance = instancesInScene[0];

                        if (_instance == null)
                            _instance = new GameObject().AddComponent<T>();

                        _instance.gameObject.name = $"Instance - {typeof(T).Name}";
                    }

                    _instance.transform.SetParent(InstanceParent.Transform);
                }

                return _instance;
            }
        }
    }
}
