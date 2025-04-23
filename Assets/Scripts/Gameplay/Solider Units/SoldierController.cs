using UnityEngine;

using Utilities.Pooling;

namespace Gameplay.SoldierUnits
{
    public class SoldierController : HealthEntity, IPoolable
    {
        private ISoliderUnit data;

        public string SoldierName => data.Name;
        public float Damage => data.Damage;

        public void Initialize(ISoliderUnit soldierData)
        {
            data = soldierData;
            InitializeMaxHP(data.MaxHealth);
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
