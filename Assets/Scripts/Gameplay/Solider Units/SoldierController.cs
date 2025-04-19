using UnityEngine;

using Utilities.Pooling;

namespace Gameplay.SoldierUnits
{
    public class SoldierController : HealthEntity, IPoolable
    {
        [SerializeField] private SpriteRenderer spriteRenderer;

        private SoldierData data;

        public string SoldierName => data.SoldierName;
        public int Damage => data.Damage;

        public void Initialize(SoldierData soldierData)
        {
            data = soldierData;
            InitializeMaxHP(data.Health);
            spriteRenderer.sprite = data.Sprite;
        }
        protected override void Elimination()
        {
            base.Elimination();
            PoolManager.Instance.Return(this);
        }

        public void OnSpawnFromPool() { }
        public void OnReturnToPool() { }
    }
}
