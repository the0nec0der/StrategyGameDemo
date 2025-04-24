using Gameplay.Product;
using Gameplay.StatSystem;

using GridSystem;

using UnityEngine;

namespace Gameplay.Buildings
{
    [CreateAssetMenu(fileName = "NewBuilding", menuName = "Game/Building Data")]
    public class BuildingData : ScriptableObject, IBuilding
    {
        [Header("Product Info")]
        [SerializeField] private ProductData productData;

        [Header("Stat Components")]
        [SerializeField] private DefenseData defenseData;
        [SerializeField] private HealthData healthData;

        [Header("Tile Components")]
        [SerializeField] private SizeData sizeData;
        [SerializeField] private TileColorData tileColorData;

        // Product
        public string Id => productData.Id;
        public string Name => productData.Name;
        public string Description => productData.Description;
        public Sprite Icon => productData.Icon;
        public GameObject Prefab => productData.Prefab;

        // Size Stat
        public Vector2Int Size => sizeData.Size;

        // Defense Stat
        public float Armor => defenseData.Armor;
        public float Resistance => defenseData.Resistance;

        // Health Stat
        public float MaxHealth => healthData.MaxHealth;

        // Tile Color
        public Gradient OccupiedGradient => tileColorData.OccupiedGradient;
    }
}
