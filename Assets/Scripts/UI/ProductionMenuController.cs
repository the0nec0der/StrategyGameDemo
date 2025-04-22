using System.Collections.Generic;

using Gameplay.Product;

using UnityEngine;

using Utilities;
using Utilities.Pooling;

namespace UI
{
    public class ProductionMenuController : MenuControllerBase
    {
        [SerializeField] private ProductCard productCardPrefab;
        [SerializeField] private Transform contentParent;

        private List<ProductCard> activeCards = new();

        private void Start()
        {
            PoolManager.Instance.CreatePool(productCardPrefab, 30);

            IProduct[] products = ResourceLoader.LoadAllFromResources<IProduct, ScriptableObject>();

            foreach (var product in products)
            {
                var card = PoolManager.Instance.Get<ProductCard>(Vector3.zero, Quaternion.identity);
                card.transform.SetParent(contentParent, false);
                card.SetProductCard(product);
                activeCards.Add(card);
            }
        }
    }
}
