using System;

using Misc;

using UnityEngine;

namespace Gameplay.Product
{
    [Serializable]
    public class ProductData : IProduct
    {
        [Header("Product Data")]
        [SerializeField, ReadOnly] private string id = Guid.NewGuid().ToString();
        [SerializeField] private string productName;
        [SerializeField, TextArea(2, 4)] private string description;
        [SerializeField] private Sprite icon;
        [SerializeField] private GameObject prefab;

        [Space, SerializeField, TextArea(4, 4)] private string notes;

        public string Id => id;
        public string Name => productName;
        public string Description => description;
        public Sprite Icon => icon;
        public GameObject Prefab => prefab;
    }
}
