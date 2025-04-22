using UnityEngine;

namespace Gameplay.StatSystem
{
    [System.Serializable]
    public class DamageData : IDamageStat
    {
        [SerializeField] private float damage;
        [SerializeField] private float attackSpeed;
        [SerializeField] private float criticalChance;
        [SerializeField] private float criticalMultiplier;

        public float Damage => damage;
        public float AttackSpeed => attackSpeed;
        public float CriticalChance => criticalChance;
        public float CriticalMultiplier => criticalMultiplier;
    }
}
