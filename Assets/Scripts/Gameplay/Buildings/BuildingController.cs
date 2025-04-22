using Gameplay.SoldierUnits;

using UnityEngine;

using Utilities.Pooling;

namespace Gameplay.Buildings
{
    public class BuildingController : HealthEntity, IPoolable
    {
        private IBuilding data;
        private IProducerBuilding producerData;

        public string BuildingName => data.Name;
        public bool CanProduceUnits => producerData != null && producerData.CanProduceSoldiers;
        public Vector2Int Size => data.Size;

        public void Initialize(BuildingData buildingData)
        {
            data = buildingData;
            producerData = buildingData as ProducerBuildingData;

            InitializeMaxHP(data.MaxHealth);
        }

        public ISoliderUnit[] GetProducibleSoldiers()
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
