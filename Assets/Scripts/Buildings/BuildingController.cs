using UnityEngine;

using Utilities.Pooling;

namespace Gameplay.Buildings
{
    public class BuildingController : MonoBehaviour, IPoolable
    {
        [SerializeField] private BuildingData data;
        private int currentHP;

        public BuildingData Data => data;

        public void Initialize(BuildingData newData)
        {
            data = newData;
            currentHP = data.Health;
            //TODO handle visual of building
        }

        public void TakeDamage(int amount)
        {
            currentHP -= amount;
            if (currentHP <= 0)
                DestroyBuilding();
        }

        private void DestroyBuilding()
        {
            PoolManager.Instance.Return(this);
        }

        public int GetCurrentHP() => currentHP;

        public void OnSpawnFromPool() { }

        public void OnReturnToPool() { }
    }
}
