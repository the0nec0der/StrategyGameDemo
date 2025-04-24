using System.Collections.Generic;

using Gameplay.Buildings;

using TMPro;

using UI;

using UnityEngine;
using UnityEngine.UI;

public class BuildingInformationMenuController : MenuControllerBase
{
    [SerializeField] private TMP_Text buildingName;
    [SerializeField] private TMP_Text buildingDescription;
    [SerializeField] private Image buildingIconImage;

    [Header("Placing UI")]
    [SerializeField] private Button placeBuildingButton;
    [SerializeField] private TMP_Text placeBuildingText;
    [SerializeField] private Image placeBuildingImage;

    [Header("Producer UI")]
    [SerializeField] private Transform produciblesMenuTransform;
    [SerializeField] private Transform produciblesContentTransform;
    [SerializeField] private ProductCard productCardPrefab;

    private ProductCardDisplayer cardDisplayer = null;
    private readonly List<ProductCard> activeCards = new();

    protected override void Awake()
    {
        base.Awake();

        EnsureCardDisplayerAssigned();
    }

    public void SetBuildingInformationPanel(IBuilding building)
    {
        if (building is null or not IBuilding)
            return;

        EnsureCardDisplayerAssigned();

        buildingName.text = building.Name;
        buildingDescription.text = building.Description;
        buildingIconImage.sprite = building.Icon;

        placeBuildingButton.onClick.RemoveAllListeners();
        placeBuildingButton.onClick.AddListener(() =>
        {
            GameLogicMediator.Instance.BuildingPlacer.StartPlacingBuilding(building);
            GameLogicMediator.Instance.ProductionMenuController.CloseMenu();
            CloseMenu();
        });
        placeBuildingImage.sprite = building.Icon;
        placeBuildingText.text = $"Place The {building.Name}";

        if (building is IProducerBuilding producerBuilding)
        {
            produciblesMenuTransform.gameObject.SetActive(true);

            cardDisplayer.DisplayProducts(
                activeCards,
                producerBuilding.ProducibleSoldiers,
                productCardPrefab,
                produciblesContentTransform,
                10
            );
        }
        else
        {
            produciblesMenuTransform.gameObject.SetActive(false);
            cardDisplayer.HideAll(activeCards);
        }

        OpenMenu();
    }
    private void EnsureCardDisplayerAssigned()
    {
        cardDisplayer = cardDisplayer != null ? cardDisplayer : GetComponent<ProductCardDisplayer>() ?? gameObject.AddComponent<ProductCardDisplayer>();
    }
}
