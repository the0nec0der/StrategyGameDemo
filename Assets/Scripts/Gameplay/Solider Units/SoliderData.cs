using Gameplay.Product;

using UnityEngine;

namespace Gameplay.SoldierUnits
{
    [CreateAssetMenu(fileName = "NewSoldier", menuName = "Game/Soldier Data")]
    public class SoldierData : ProductData, ISoliderUnit
    {
        [Header("Solider Data")]
        [SerializeField] private int damage;

        public float Damage => damage;
    }
}
