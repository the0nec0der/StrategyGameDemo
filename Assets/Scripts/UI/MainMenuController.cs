using GridSystem;
using Misc;
using UnityEngine;

namespace UI
{
    public class MainMenuController : MenuControllerBase
    {
        public void PlayGame()
        {
            TransitionController transitionController = GameLogicMediator.Instance.TransitionController;
            transitionController.FadeToBlack(1f, 0f, onFadeFinish: () =>
            {
                CloseMenu();
                GridManager.Instance.InitializeGrid();
                transitionController.FadeFromBlack(1f, 0f);
            });
        }

        public void OpenSettings()
        {
            GameLogicMediator.Instance.SettingsMenuController.OpenMenu();
        }

        public void QuitGame() => Application.Quit();
    }
}
