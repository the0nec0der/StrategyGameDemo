using Gameplay.SoldierUnits;

namespace Gameplay.Buildings
{
    public interface IProducerBuilding : IBuilding
    {
        ISoliderUnit[] ProducibleSoldiers { get; }
        public bool CanProduceSoldiers => ProducibleSoldiers != null && ProducibleSoldiers.Length > 0;
    }
}
