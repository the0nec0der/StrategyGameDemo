using Gameplay.Product;

using UnityEngine;

namespace Gameplay.Buildings
{
    [CreateAssetMenu(fileName = "NewBuilding", menuName = "Game/Building Data")]
    public class BuildingData : ProductData, IBuilding
    {
        [Header("Building Data")]
        [SerializeField] private Vector2Int size;

        public Vector2Int Size => size;
    }
}
