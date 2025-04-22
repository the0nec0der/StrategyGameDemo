using UnityEngine;

using Utilities.Pooling;

namespace Gameplay.SoldierUnits
{
    public class SoldierController : HealthEntity, IPoolable
    {
        private SoldierData data;

        public string SoldierName => data.Name;
        public float Damage => data.Damage;

        public void Initialize(SoldierData soldierData)
        {
            data = soldierData;
            InitializeMaxHP(data.Health);
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
