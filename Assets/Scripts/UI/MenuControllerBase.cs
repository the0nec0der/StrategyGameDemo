using System;
using EditorHelper;
using UnityEngine;

namespace UI
{
    public abstract class MenuControllerBase : MonoBehaviour, IMenuController
    {
        [SerializeField] private bool activeOnStart = false;

        public event IMenuController.MenuOpenedDelegate OnMenuOpened;
        public event IMenuController.MenuClosedDelegate OnMenuClosed;

        private Canvas canvas;
        private Action onMenuClosedCallback = null;

        public bool IsOpen => canvas != null && canvas.enabled;

        protected virtual void Awake()
        {
            canvas = GetComponent<Canvas>();

            if (activeOnStart)
            {
                OnMenuOpened?.Invoke(this);

                MenuOpened();
            }

            canvas.enabled = activeOnStart;
        }

        [Button]
        public void OpenMenu(Action onMenuClosedCallback = null)
        {
            if (IsOpen) return;

            this.onMenuClosedCallback = onMenuClosedCallback;

            MenuOpened();

            OnMenuOpened?.Invoke(this);
            canvas.enabled = true;
        }

        [Button]
        public void CloseMenu()
        {
            if (!IsOpen) return;

            Action tempOnMenuClosedCallback = onMenuClosedCallback;
            onMenuClosedCallback = null;

            tempOnMenuClosedCallback?.Invoke();
            OnMenuClosed?.Invoke(this);
            canvas.enabled = false;

            MenuClosed();
        }

        protected virtual void MenuOpened() { }
        protected virtual void MenuClosed() { }
    }
}
