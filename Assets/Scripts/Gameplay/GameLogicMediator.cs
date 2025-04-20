using Core.InstanceSystem;
using GridSystem;
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

    public SettingsMenuController SettingsMenuController { get; private set; }

    private void Start()
    {
        if (Instance != this)
            return;

        SaveManager = Instanced<SaveManager>.Instance;
        AudioManager = Instanced<AudioManager>.Instance;
        GridManager = Instanced<GridManager>.Instance;

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
