using Gameplay.SoldierUnits;

using UnityEngine;

namespace Gameplay.Buildings
{
    [CreateAssetMenu(fileName = "NewProducerBuilding", menuName = "Game/Building Data (Producer)")]
    public class ProducerBuildingData : BuildingData
    {
        [Header("Production Options")]
        [SerializeField] private SoldierData[] producibleSoldiers;

        public SoldierData[] ProducibleSoldiers => producibleSoldiers;
        public bool CanProduceSoldiers => producibleSoldiers != null && producibleSoldiers.Length > 0;
    }
}
