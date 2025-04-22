using System;
using System.Collections;
using System.Collections.Generic;

using EditorHelper;

using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public abstract class MenuControllerBase : MonoBehaviour, IMenuController
    {
        [SerializeField] private bool activeOnStart = false;

        public event IMenuController.MenuOpenedDelegate OnMenuOpened;
        public event IMenuController.MenuClosedDelegate OnMenuClosed;

        private Canvas canvas;
        private readonly List<RectTransform> rebuildableLayouts = new();
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

        protected virtual void MenuOpened()
        {
            RefreshLayouts();
        }
        protected virtual void MenuClosed() { }

        protected void RefreshLayouts()
        {
            CacheRebuildableLayouts();
            StartCoroutine(RebuildLayoutsCoroutine());
        }

        private void CacheRebuildableLayouts()
        {
            rebuildableLayouts.Clear();

            foreach (RectTransform layoutGroup in GetComponentsInChildren<RectTransform>())
            {
                RectTransform rectTransform = layoutGroup.GetComponent<RectTransform>();
                if (rectTransform != null)
                    rebuildableLayouts.Add(rectTransform);
            }

            rebuildableLayouts.Reverse();
        }

        private IEnumerator RebuildLayoutsCoroutine()
        {
            yield return null;

            foreach (RectTransform rectTransform in rebuildableLayouts)
                LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
        }
    }
}
