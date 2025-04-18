using UnityEngine;

namespace Core.InstanceSystem
{
    internal static class InstanceParent
    {
        private static Transform _transform = null;
        public static Transform Transform
        {
            get
            {
                if (_transform == null)
                {
                    _transform = new GameObject("Instances").transform;
                    Object.DontDestroyOnLoad(_transform);
                }
                return _transform;
            }
        }
    }
}
