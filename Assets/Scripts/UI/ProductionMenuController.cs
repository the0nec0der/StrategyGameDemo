using System.Collections.Generic;

using Core.InstanceSystem;

using Gameplay.Buildings;
using Gameplay.Product;
using GridSystem;
using PlacingSystem;
using UnityEngine;

using Utilities;

namespace UI
{
    public class ProductionMenuController : MenuControllerBase
    {
        [SerializeField] private ProductCard productCardPrefab;
        [SerializeField] private Transform contentParent;

        private BuildingInformationMenuController BuildingInformationMenuController = null;
        private BuildingPlacer BuildingPlacer = null;
        private ProductCardDisplayer ProductCardDisplayer = null;
        private List<ProductCard> activeCards = new();

        protected override void Awake()
        {
            base.Awake();
            EnsureCardDisplayerAssigned();
        }

        protected override void MenuOpened()
        {
            base.MenuOpened();
            EnsureCardDisplayerAssigned();

            BuildingInformationMenuController = Instanced<BuildingInformationMenuController>.Instance;
            BuildingPlacer = Instanced<BuildingPlacer>.Instance;

            IProduct[] products = ResourceLoader.LoadAllFromResources<IProduct, BuildingData>();

            ProductCardDisplayer.DisplayProducts(
                activeCards,
                products,
                productCardPrefab,
                contentParent,
                30,
                (product) => () =>
                {
                    BuildingInformationMenuController.SetBuildingInformationPanel(product as IBuilding);
                    BuildingPlacer.StartPlacingBuilding(product as IBuilding);
                    CloseMenu();
                }
            );
        }
        private void EnsureCardDisplayerAssigned()
        {
            ProductCardDisplayer = ProductCardDisplayer != null ? ProductCardDisplayer : GetComponent<ProductCardDisplayer>() ?? gameObject.AddComponent<ProductCardDisplayer>();
        }
    }
}
