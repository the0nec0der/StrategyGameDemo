using UnityEngine;

namespace Gameplay.Product
{
    public class ProductData : ScriptableObject, IProduct
    {
        [Header("Product Data")]
        [SerializeField, TextArea(2, 4)] private string description;
        [SerializeField] private Sprite icon;
        [SerializeField] private float health;
        [SerializeField] private GameObject prefab;

        [Space, SerializeField, TextArea(4, 4)] private string notes;

        public string Id => GetInstanceID().ToString();
        public string Name => name;
        public string Description => description;
        public Sprite Icon => icon;
        public float Health => health;
        public GameObject Prefab => prefab;
    }
}
