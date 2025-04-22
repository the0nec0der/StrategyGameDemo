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

        public void SetProductCard(IProduct productReference)
        {
            productName.text = productReference?.Name;
            productIcon.sprite = productReference?.Icon;
        }
    }
}
