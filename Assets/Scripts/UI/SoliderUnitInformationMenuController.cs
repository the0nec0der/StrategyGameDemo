using Gameplay.SoldierUnits;

using TMPro;

using UI;

using UnityEngine;
using UnityEngine.UI;

public class SoliderUnitInformationMenuController : MenuControllerBase
{
    [SerializeField] private TMP_Text soliderUnitName;
    [SerializeField] private TMP_Text soliderUnitDescription;
    [SerializeField] private Image soliderUnitIconImage;

    [Header("Placing UI")]
    [SerializeField] private Button placeSoliderUnitButton;
    [SerializeField] private TMP_Text placeSoliderUnitText;
    [SerializeField] private Image placeSoliderUnitImage;

    private GameLogicMediator GameLogicMediator => GameLogicMediator.Instance;
    private ProductCardDisplayer cardDisplayer = null;

    protected override void Awake()
    {
        base.Awake();

        EnsureCardDisplayerAssigned();
    }

    public void SetInformationPanel(ISoliderUnit soliderUnit, bool isPlaced = false)
    {
        if (soliderUnit is null or not ISoliderUnit)
            return;

        EnsureCardDisplayerAssigned();

        soliderUnitName.text = soliderUnit.Name;
        soliderUnitDescription.text = soliderUnit.Description;
        soliderUnitIconImage.sprite = soliderUnit.Icon;

        if (!isPlaced)
        {
            placeSoliderUnitButton.onClick.RemoveAllListeners();
            placeSoliderUnitButton.onClick.AddListener(() =>
            {
                GameLogicMediator.SoldierPlacer.StartPlacingSoldier(soliderUnit);
                GameLogicMediator.BuildingInformationMenuController.CloseMenu();
                CloseMenu();
            });

            placeSoliderUnitImage.sprite = soliderUnit.Icon;
            placeSoliderUnitText.text = $"Place The {soliderUnit.Name}";

            placeSoliderUnitButton.gameObject.SetActive(true);
        }
        else
            placeSoliderUnitButton.gameObject.SetActive(false);

        OpenMenu();
    }

    private void EnsureCardDisplayerAssigned()
    {
        cardDisplayer = cardDisplayer != null ? cardDisplayer : GetComponent<ProductCardDisplayer>() ?? gameObject.AddComponent<ProductCardDisplayer>();
    }
}
