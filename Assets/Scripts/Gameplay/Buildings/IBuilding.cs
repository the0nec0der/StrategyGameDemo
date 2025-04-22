using Gameplay.Product;
using UnityEngine;

namespace Gameplay.Buildings
{
    public interface IBuilding : IProduct
    {
        Vector2Int Size { get; }
    }
}
