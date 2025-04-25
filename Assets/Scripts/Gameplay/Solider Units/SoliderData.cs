using Gameplay.Product;
using Gameplay.StatSystem;
using GridSystem;
using UnityEngine;

namespace Gameplay.SoldierUnits
{
    [CreateAssetMenu(fileName = "NewSoldier", menuName = "Game/Soldier Data")]
    public class SoldierData : ScriptableObject, ISoliderUnitData
    {
        [Header("Product Info")]
        [SerializeField] private ProductData productData;

        [Header("Stat Components")]
        [SerializeField] private DamageData damageData;
        [SerializeField] private DefenseData defenseData;
        [SerializeField] private HealthData healthData;
        [SerializeField] private RangeData rangeData;

        [Header("Tile Component")]
        [SerializeField] private TileColorData tileColorData;

        // Product
        public string Id => productData.Id;
        public string Name => productData.Name;
        public string Description => productData.Description;
        public Sprite Icon => productData.Icon;
        public GameObject Prefab => productData.Prefab;

        // Damage Stat
        public float Damage => damageData.Damage;
        public float AttackSpeed => damageData.AttackSpeed;
        public float CriticalChance => damageData.CriticalChance;
        public float CriticalMultiplier => damageData.CriticalMultiplier;

        // Defense Stat
        public float Armor => defenseData.Armor;
        public float Resistance => defenseData.Resistance;

        // Health Stat
        public float MaxHealth => healthData.MaxHealth;

        // Range
        public GridRangePattern AttackPattern => rangeData.AttackPattern;

        // Tile Color
        public Gradient OccupiedGradient => tileColorData.OccupiedGradient;
    }
}
