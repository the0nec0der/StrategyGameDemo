using UnityEngine;

using Utilities.Pooling;

namespace Gameplay.Buildings
{
    public class BuildingController : HealthEntity, IPoolable
    {
        [SerializeField] private SpriteRenderer spriteRenderer;

        private BuildingData data;

        public string BuildingName => data.BuildingName;

        public void Initialize(BuildingData buildingData)
        {
            data = buildingData;
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
