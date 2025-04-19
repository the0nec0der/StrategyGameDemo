using Gameplay.SoldierUnits;

using UnityEngine;

using Utilities.Pooling;

namespace Gameplay.Buildings
{
    public class BuildingController : HealthEntity, IPoolable
    {
        [SerializeField] private SpriteRenderer spriteRenderer;

        private BuildingData data;
        private ProducerBuildingData producerData;

        public string BuildingName => data.BuildingName;
        public bool CanProduceUnits => producerData != null && producerData.CanProduceSoldiers;
        public Vector2Int Size => data.Size;

        public void Initialize(BuildingData buildingData)
        {
            data = buildingData;
            producerData = buildingData as ProducerBuildingData;

            InitializeMaxHP(data.Health);
            spriteRenderer.sprite = data.Sprite;
        }

        public SoldierData[] GetProducibleSoldiers()
        {
            return producerData?.ProducibleSoldiers;
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
