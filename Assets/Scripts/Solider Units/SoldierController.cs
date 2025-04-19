using System;

using UnityEngine;

using Utilities.Pooling;

namespace Gameplay.SoldierUnits
{
    public class SoldierController : MonoBehaviour, IPoolable
    {
        [SerializeField] private SpriteRenderer spriteRenderer;

        private SoldierData data;
        private int currentHP;

        public event Action<SoldierController, int> OnHealthChanged;

        public string SoldierName => data.SoldierName;
        public int Damage => data.Damage;
        public int MaxHP => data.Health;
        public bool IsAlive => currentHP > 0;

        public int CurrentHP
        {
            get => currentHP;
            set
            {
                if (value > MaxHP)
                    throw new Exception($"HP can't be higher than MaxHP ({MaxHP})");

                if (currentHP == value)
                    return;

                int delta = value - currentHP;
                currentHP = value;
                OnHealthChanged?.Invoke(this, delta);

                if (currentHP <= 0)
                    Die();
            }
        }

        public void Initialize(SoldierData soldierData)
        {
            data = soldierData;
            CurrentHP = data.Health;
            spriteRenderer.sprite = data.Sprite;
        }

        public void TakeDamage(int amount)
        {
            if (!IsAlive) return;
            CurrentHP -= amount;
        }

        private void Die()
        {
            PoolManager.Instance.Return(this);
        }

        public void OnSpawnFromPool() { }
        public void OnReturnToPool() { }
    }
}
