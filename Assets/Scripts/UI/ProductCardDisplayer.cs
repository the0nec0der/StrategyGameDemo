using System;
using System.Collections.Generic;

using Gameplay.Product;

using UnityEngine;

using Utilities.Pooling;

namespace UI
{
    public class ProductCardDisplayer : MonoBehaviour
    {
        public void DisplayProducts(
            List<ProductCard> cardList,
            IProduct[] products,
            ProductCard prefab,
            Transform parent,
            int poolSize = 10,
            Func<IProduct, Action> onClickCard = null
        )
        {
            PoolManager.Instance.CreatePool(prefab, poolSize);

            int i = 0;
            for (; i < products.Length; i++)
            {
                ProductCard card;
                if (i < cardList.Count)
                {
                    card = cardList[i];
                }
                else
                {
                    card = PoolManager.Instance.Get<ProductCard>(Vector3.zero, Quaternion.identity);
                    card.transform.SetParent(parent, false);
                    cardList.Add(card);
                }

                card.gameObject.SetActive(true);
                card.SetProductCard(products[i], onClickCard?.Invoke(products[i]));
            }

            for (; i < cardList.Count; i++)
            {
                cardList[i].gameObject.SetActive(false);
            }
        }

        public void HideAll(List<ProductCard> cardList)
        {
            foreach (var card in cardList)
                card.gameObject.SetActive(false);
        }
    }
}
