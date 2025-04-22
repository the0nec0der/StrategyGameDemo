using UnityEngine;

namespace Gameplay.StatSystem
{
    [System.Serializable]
    public class RangeData : IRangeStat
    {
        [SerializeField] private GridRangePattern attackPattern;

        public GridRangePattern AttackPattern => attackPattern;
    }
}
