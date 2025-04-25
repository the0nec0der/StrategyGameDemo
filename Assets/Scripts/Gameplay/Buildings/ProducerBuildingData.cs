using Gameplay.SoldierUnits;

using UnityEngine;

namespace Gameplay.Buildings
{
    [CreateAssetMenu(fileName = "NewProducerBuilding", menuName = "Game/Building Data (Producer)")]
    public class ProducerBuildingData : BuildingData, IProducerBuilding
    {
        [Header("Production Options")]
        [SerializeField] private SoldierData[] producibleSoldiers;

        public ISoliderUnitData[] ProducibleSoldiers => producibleSoldiers;
    }
}
