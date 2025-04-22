using UnityEngine;

namespace Gameplay.StatSystem
{
    [System.Serializable]
    public class SizeData : ISizeStat
    {
        [SerializeField] private Vector2Int size;

        public Vector2Int Size => size;
    }
}
