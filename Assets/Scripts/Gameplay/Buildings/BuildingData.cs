using Gameplay.SoldierUnits;

using UnityEngine;

namespace Gameplay.Buildings
{
    [CreateAssetMenu(fileName = "NewBuilding", menuName = "Game/Building Data")]
    public class BuildingData : ScriptableObject
    {
        [Header("Building Data")]
        [SerializeField] private string buildingName;
        [SerializeField] private string purpose;
        [SerializeField] private string notes;
        [SerializeField] private Sprite buildingSprite;
        [SerializeField] private Vector2Int size;
        [SerializeField] private int health;
        [SerializeField] private GameObject prefab;

        [Header("Production Options")]
        [SerializeField] private SoldierData[] producibleSoldiers;

        public string BuildingName => buildingName;
        public string Purpose => purpose;
        public Sprite Sprite => buildingSprite;
        public Vector2Int Size => size;
        public int Health => health;
        public GameObject Prefab => prefab;

        public SoldierData[] ProducibleSoldiers => producibleSoldiers;
        public bool CanProduceSoldiers => producibleSoldiers != null && producibleSoldiers.Length > 0;
    }
}
