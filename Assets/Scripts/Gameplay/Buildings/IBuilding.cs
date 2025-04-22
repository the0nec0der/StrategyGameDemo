using Gameplay.Product;
using Gameplay.StatSystem;

namespace Gameplay.Buildings
{
    public interface IBuilding : IProduct, IBuildingStats, ISizeStat
    {
    }
}
