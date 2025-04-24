using Core.InstanceSystem;
using Gameplay.Buildings;
using Gameplay.SoldierUnits;
using GridSystem;
using PlacingSystem;
using SaveSystem;

using SettingSystem;

using UI;

using UnityEngine;

public class GameLogicMediator : MonoBehaviour
{
    public static GameLogicMediator Instance => Instanced<GameLogicMediator>.Instance;

    public SaveManager SaveManager { get; private set; }
    public AudioManager AudioManager { get; private set; }
    public GridManager GridManager { get; private set; }
    public BuildingPlacer BuildingPlacer { get; private set; }
    public BuildingFactory BuildingFactory { get; private set; }
    public SoldierPlacer SoldierPlacer { get; private set; }
    public SoldierFactory SoldierFactory { get; private set; }
    public ProductionMenuController ProductionMenuController { get; private set; }
    public BuildingInformationMenuController BuildingInformationMenuController { get; private set; }

    public SettingsMenuController SettingsMenuController { get; private set; }

    private void Start()
    {
        if (Instance != this)
            return;

        SaveManager = Instanced<SaveManager>.Instance;
        AudioManager = Instanced<AudioManager>.Instance;

        GridManager = Instanced<GridManager>.Instance;

        BuildingPlacer = Instanced<BuildingPlacer>.Instance;
        BuildingFactory = Instanced<BuildingFactory>.Instance;

        SoldierPlacer = Instanced<SoldierPlacer>.Instance;
        SoldierFactory = Instanced<SoldierFactory>.Instance;

        BuildingInformationMenuController = Instanced<BuildingInformationMenuController>.Instance;
        ProductionMenuController = Instanced<ProductionMenuController>.Instance;
        SettingsMenuController = Instanced<SettingsMenuController>.Instance;

        LoadGameData();
    }

    private void OnDestroy()
    {
        SaveGameData();
    }

    private void LoadGameData()
    {
        try
        {
            SaveManager?.LoadGame();
        }
        catch (SaveManager.SaveNotFoundException)
        {
            Debug.LogWarning("Save file not found. Starting with default values.");
            // TODO: Initialize default values here.
        }
    }

    private void SaveGameData()
    {
        SaveManager?.SaveGame();
    }
}
