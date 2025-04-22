using UnityEngine;

namespace Gameplay.StatSystem
{
    [System.Serializable]
    public class HealthData : IHealthStat
    {
        [SerializeField] private float maxHealth;

        public float MaxHealth => maxHealth;
    }

}
