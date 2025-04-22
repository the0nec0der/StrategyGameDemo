using System.Collections.Generic;
using Gameplay.Buildings;
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

        protected override void MenuOpened()
        {
            base.MenuOpened();

            PoolManager.Instance.CreatePool(productCardPrefab, 30);

            IProduct[] products = ResourceLoader.LoadAllFromResources<IProduct, BuildingData>();

            int i = 0;
            for (; i < products.Length; i++)
            {
                ProductCard card;
                if (i < activeCards.Count)
                {
                    card = activeCards[i];
                }
                else
                {
                    card = PoolManager.Instance.Get<ProductCard>(Vector3.zero, Quaternion.identity);
                    card.transform.SetParent(contentParent, false);
                    activeCards.Add(card);
                }

                card.gameObject.SetActive(true);
                card.SetProductCard(products[i]);
            }

            // Disable unused cards
            for (; i < activeCards.Count; i++)
            {
                activeCards[i].gameObject.SetActive(false);
            }
        }
    }
}
