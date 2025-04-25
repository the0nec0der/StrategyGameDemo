using Gameplay.SoldierUnits;

namespace Gameplay.Buildings
{
    public interface IProducerBuilding : IBuilding
    {
        ISoliderUnitData[] ProducibleSoldiers { get; }
        public bool CanProduceSoldiers => ProducibleSoldiers != null && ProducibleSoldiers.Length > 0;
    }
}
