using System.Collections.Generic;
using Gameplay;
using Gameplay.Buildings;
using Gameplay.Product;

using UnityEngine;

using Utilities;

namespace UI
{
    public class ProductionMenuController : MenuControllerBase
    {
        [SerializeField] private ProductCard productCardPrefab;
        [SerializeField] private Transform contentParent;

        private ProductCardDisplayer ProductCardDisplayer = null;
        private List<ProductCard> activeCards = new();

        private GameStateManager GameStateManager => GameStateManager.Instance;

        protected override void Awake()
        {
            base.Awake();
            EnsureCardDisplayerAssigned();
        }

        protected override void MenuOpened()
        {
            base.MenuOpened();
            EnsureCardDisplayerAssigned();

            IProduct[] products = ResourceLoader.LoadAllFromResources<IProduct, BuildingData>();

            ProductCardDisplayer.DisplayProducts(
                activeCards,
                products,
                productCardPrefab,
                contentParent,
                30,
                (product) => () =>
                {
                    GameLogicMediator.Instance.BuildingInformationMenuController.SetBuildingInformationPanel(product as IBuilding);
                    // CloseMenu();
                }
            );
        }

        public void CloseMenuOnUI()
        {
            if (GameStateManager.PreviousState == null)
                GameStateManager.SetState(Enums.GameStateType.Idle);
            else
                GameStateManager.Instance.RestorePreviousState();
            CloseMenu();
        }

        protected override void MenuClosed()
        {
            base.MenuClosed();
        }

        private void EnsureCardDisplayerAssigned()
        {
            ProductCardDisplayer = ProductCardDisplayer != null ? ProductCardDisplayer : GetComponent<ProductCardDisplayer>() ?? gameObject.AddComponent<ProductCardDisplayer>();
        }
    }
}
