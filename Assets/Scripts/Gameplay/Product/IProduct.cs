using UnityEngine;

namespace Gameplay.Product
{
    public interface IProduct
    {
        string Id { get; }
        string Name { get; }
        string Description { get; }
        Sprite Icon { get; }
        GameObject Prefab { get; }
    }
}
