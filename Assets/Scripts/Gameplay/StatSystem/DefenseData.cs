using UnityEngine;

namespace Gameplay.StatSystem
{
    [System.Serializable]
    public class DefenseData : IDefenseStat
    {
        [SerializeField] private float armor;
        [SerializeField] private float resistance;

        public float Armor => armor;
        public float Resistance => resistance;
    }
}
