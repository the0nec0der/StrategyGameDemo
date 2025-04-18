using UnityEngine;

namespace Gameplay.Buildings
{
    [CreateAssetMenu(fileName = "NewBuilding", menuName = "Game/Building Data")]
    public class BuildingData : ScriptableObject
    {
        [SerializeField] private string buildingName;
        [SerializeField] private string purpose;
        [SerializeField] private string notes;
        [SerializeField] private Sprite buildingSprite;
        [SerializeField] private Vector2Int size;
        [SerializeField] private int health;
        [SerializeField] private GameObject prefab;

        public string BuildingName => buildingName;
        public string Purpose => purpose;
        public Sprite BuildingSprite => buildingSprite;
        public Vector2Int Size => size;
        public int Health => health;
        public GameObject Prefab => prefab;
    }
}
