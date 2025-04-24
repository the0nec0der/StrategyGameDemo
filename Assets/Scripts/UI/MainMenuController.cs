using Gameplay;

using GridSystem;

using Misc;

using UnityEngine;

namespace UI
{
    public class MainMenuController : MenuControllerBase
    {
        [Header("Menu Panels")]
        [SerializeField] private GameObject mainMenu;
        [SerializeField] private GameObject gameMenu;
        [SerializeField] private GameObject pauseMenu;

        private GameLogicMediator GameLogicMediator => GameLogicMediator.Instance;
        private GameStateManager GameStateManager => GameStateManager.Instance;
        private TransitionController TransitionController => GameLogicMediator.TransitionController;

        protected override void Awake()
        {
            base.Awake();
            SetMenuState(main: true, pause: false, game: false);
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
