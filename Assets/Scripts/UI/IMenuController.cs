namespace UI
{
    public interface IMenuController
    {
        event MenuOpenedDelegate OnMenuOpened;
        event MenuClosedDelegate OnMenuClosed;

        bool IsOpen { get; }

        void OpenMenu(System.Action onMenuClosedCallback = null);
        void CloseMenu();

        public delegate void MenuOpenedDelegate(IMenuController sender);
        public delegate void MenuClosedDelegate(IMenuController sender);
    }
}
