using UnityEngine;

namespace Gameplay.Product
{
    public interface IProduct
    {
        string Id { get; }
        string Name { get; }
        string Description { get; }
        Sprite Icon { get; }
        float Health { get; }
        GameObject Prefab { get; }
    }
}
