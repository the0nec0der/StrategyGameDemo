using System;

using Gameplay.Product;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ProductCard : MonoBehaviour
    {
        [SerializeField] private TMP_Text productName = null;
        [SerializeField] private Image productIcon = null;
        [SerializeField] private Button productButton = null;

        private IProduct product = null;
        private Action onClickAction = null;

        public IProduct Product => product;

        public void SetProductCard(IProduct productReference, Action onClick = null)
        {
            ClearCard();

            product = productReference;

            productName.text = product?.Name;
            productIcon.sprite = product?.Icon;

            onClickAction = onClick;

            productButton.onClick.RemoveAllListeners();
            productButton.onClick.AddListener(() => onClickAction?.Invoke());
        }

        private void ClearCard()
        {
            product = null;
            productName.text = "";
            productIcon.sprite = default;

            if (onClickAction != null)
            {
                productButton.onClick.RemoveListener(() => onClickAction());
                onClickAction = null;
            }
        }
    }
}
