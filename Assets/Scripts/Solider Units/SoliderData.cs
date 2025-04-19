using UnityEngine;

namespace Gameplay.SoldierUnits
{
    [CreateAssetMenu(fileName = "NewSoldier", menuName = "Game/Soldier Data")]
    public class SoldierData : ScriptableObject
    {
        [SerializeField] private string soldierName;
        [SerializeField] private int damage;
        [SerializeField] private int health = 10;
        [SerializeField] private Sprite sprite;
        [SerializeField] private GameObject prefab;

        public string SoldierName => soldierName;
        public int Damage => damage;
        public int Health => health;
        public Sprite Sprite => sprite;
        public GameObject Prefab => prefab;
    }
}
