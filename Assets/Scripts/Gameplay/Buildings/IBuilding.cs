using Gameplay.Product;
using Gameplay.StatSystem;

using GridSystem;

namespace Gameplay.Buildings
{
    public interface IBuilding : IProduct, IBuildingStats, ISizeStat, ITileColor
    {
    }
}
