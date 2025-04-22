using Gameplay.Product;

namespace Gameplay.SoldierUnits
{
    public interface ISoliderUnit : IProduct
    {
        float Damage { get; }
    }
}
