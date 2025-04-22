using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
    public static class ResourceLoader
    {
        public static TInterface[] LoadAllFromResources<TInterface, TAsset>(string path = "")
            where TInterface : class
            where TAsset : ScriptableObject
        {
            var assets = string.IsNullOrEmpty(path)
                ? Resources.LoadAll<TAsset>("")
                : Resources.LoadAll<TAsset>(path);

            var result = new List<TInterface>();

            foreach (var asset in assets)
            {
                if (asset is TInterface casted)
                    result.Add(casted);
            }

            return result.ToArray();
        }
    }
}
