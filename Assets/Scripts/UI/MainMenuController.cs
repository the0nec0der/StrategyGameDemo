using System;
using Gameplay;

using GridSystem;

using Misc;
using PlacingSystem;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MainMenuController : MenuControllerBase
    {
        [Header("Menu Panels")]
        [SerializeField] private GameObject mainMenu;
        [SerializeField] private GameObject gameMenu;
        [SerializeField] private GameObject pauseMenu;

        [Header("Game Menu Components")]
        [SerializeField] private Button cancelButton;

        private GameLogicMediator GameLogicMediator => GameLogicMediator.Instance;
        private GameStateManager GameStateManager => GameStateManager.Instance;
        private TransitionController TransitionController => GameLogicMediator.TransitionController;
        private BuildingPlacer BuildingPlacer => GameLogicMediator.BuildingPlacer;

        private Action cancelButtonAction = null;

        protected override void Awake()
        {
            base.Awake();
            SetMenuState(main: true, pause: false, game: false);
        }

        private void OnEnable()
        {
            BuildingPlacer.OnPlacementConfirmed += DisableCancelButton;
            BuildingPlacer.OnProductConfirmed += action => ShowCancelButton(action);
        }
        private void OnDisable()
        {
            BuildingPlacer.OnPlacementConfirmed -= DisableCancelButton;
            BuildingPlacer.OnProductConfirmed -= action => ShowCancelButton(action);
        }

        public void PlayGame()
        {
            TransitionController.FadeToBlack(1f, 0f, onFadeFinish: () =>
            {
                ResumeGame();
                GridManager.Instance.InitializeGrid();
                TransitionController.FadeFromBlack(1f, 0f);
            });
        }

        public void ResumeGame()
        {
            GameStateManager.RestorePreviousState();
            SetMenuState(main: false, pause: false, game: true);
        }

        public void PauseGame()
        {
            GameStateManager.SetState(Enums.GameStateType.UI, true);
            SetMenuState(main: false, pause: true, game: false);
        }

        public void ShowCancelButton(Action action)
        {
            cancelButtonAction = action;
            if (cancelButtonAction != null)
            {
                cancelButton.gameObject.SetActive(true);
                cancelButton.onClick.RemoveAllListeners();
                cancelButton.onClick.AddListener(() =>
                {
                    cancelButtonAction?.Invoke();
                    DisableCancelButton();
                });
            }
        }

        private void DisableCancelButton()
        {
            cancelButtonAction = null;
            cancelButton.gameObject.SetActive(false);
            cancelButton.onClick.RemoveAllListeners();
        }

        public void OpenSettings() => GameLogicMediator.SettingsMenuController.OpenMenu();

        public void OpenProductionMenu() => GameLogicMediator.ProductionMenuController.OpenMenu();

        public void QuitGame() => Application.Quit();

        private void SetMenuState(bool main, bool pause, bool game)
        {
            mainMenu.SetActive(main);
            pauseMenu.SetActive(pause);
            gameMenu.SetActive(game);
            RefreshLayouts();
        }
    }
}
