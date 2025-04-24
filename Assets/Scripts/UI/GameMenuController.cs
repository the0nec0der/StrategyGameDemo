namespace UI
{
    public class GameMenuController : MenuControllerBase
    {
        public void PauseGame()
        {

        }
        public void OpenProductionMenu()
        {
            GameLogicMediator.Instance.ProductionMenuController.OpenMenu();
        }
    }
}
