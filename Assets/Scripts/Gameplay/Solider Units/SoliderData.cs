using UnityEngine;

namespace Gameplay.SoldierUnits
{
    [CreateAssetMenu(fileName = "NewSoldier", menuName = "Game/Soldier Data")]
    public class SoldierData : ScriptableObject
    {
        [SerializeField] private string soldierName;
        [SerializeField, TextArea(2, 4)] private string description;
        [SerializeField] private int damage;
        [SerializeField] private int health = 10;
        [SerializeField] private Sprite sprite;
        [SerializeField] private GameObject prefab;

        public string SoldierName => soldierName;
        public string Description => description;
        public int Damage => damage;
        public int Health => health;
        public Sprite Sprite => sprite;
        public GameObject Prefab => prefab;

        [Space, SerializeField, TextArea(4, 4)] private string notes;
    }
}
