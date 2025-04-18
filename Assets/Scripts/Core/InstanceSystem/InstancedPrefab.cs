using UnityEngine;

namespace Core.InstanceSystem
{
    internal static class InstancedPrefab<T> where T : MonoBehaviour
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
                    T[] instancesInScene = Object.FindObjectsByType<T>(FindObjectsInactive.Include, FindObjectsSortMode.None);
                    for (int i = instancesInScene.Length - 1; i >= 1; i--)
                        Object.Destroy(instancesInScene[i]);

                    if (instancesInScene.Length == 1)
                        _instance = instancesInScene[0];
                    else
                    {
                        T[] prefabs = Resources.LoadAll<T>("");

                        if (prefabs.Length == 0)
                            throw new System.Exception($"Prefab with type \"{typeof(T).Name}\" couldn't be found.");

                        _instance = Object.Instantiate(prefabs[0]);
                    }

                    _instance.gameObject.name = $"Instanced Prefab - {typeof(T).Name}";

                    _instance.transform.SetParent(InstanceParent.Transform);
                }

                return _instance;
            }
        }
    }
}
