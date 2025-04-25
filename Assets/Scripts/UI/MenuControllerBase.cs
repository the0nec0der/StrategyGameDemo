using System;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using EditorHelper;
#endif

using Enums;

using Gameplay;

using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public abstract class MenuControllerBase : MonoBehaviour, IMenuController
    {
        [SerializeField] private bool activeOnStart = false;
        [SerializeField] private bool isScreenSpaceRenderModOn = true;

        public event IMenuController.MenuOpenedDelegate OnMenuOpened;
        public event IMenuController.MenuClosedDelegate OnMenuClosed;

        private Canvas canvas;
        private readonly List<RectTransform> rebuildableLayouts = new();
        private Action onMenuClosedCallback = null;

        public bool IsOpen => canvas != null && canvas.enabled;

        private GameStateManager GameStateManager => GameStateManager.Instance;

        protected virtual void Awake()
        {
            canvas = GetComponent<Canvas>();

            if (activeOnStart)
            {
                OnMenuOpened?.Invoke(this);

                MenuOpened();
            }

            if (isScreenSpaceRenderModOn)
            {
                Camera mainCam = Camera.main;
                if (mainCam != null)
                {
                    canvas.worldCamera = mainCam;
                    canvas.renderMode = RenderMode.ScreenSpaceCamera;
                    canvas.planeDistance = 10;
                }
            }

            canvas.enabled = activeOnStart;
        }

#if UNITY_EDITOR
        [Button]
#endif

        public void OpenMenu(Action onMenuClosedCallback = null)
        {
            if (IsOpen) return;

            this.onMenuClosedCallback = onMenuClosedCallback;

            MenuOpened();

            GameStateManager.SetState(GameStateType.UI, true);

            OnMenuOpened?.Invoke(this);
            canvas.enabled = true;
        }

#if UNITY_EDITOR
        [Button]
#endif
        public void CloseMenu()
        {
            if (!IsOpen) return;

            GameStateManager.RestorePreviousState();

            Action tempOnMenuClosedCallback = onMenuClosedCallback;
            onMenuClosedCallback = null;

            tempOnMenuClosedCallback?.Invoke();
            OnMenuClosed?.Invoke(this);
            canvas.enabled = false;

            MenuClosed();
        }

        public void CloseMenuOnUI()
        {
            if (GameStateManager.PreviousState == null)
                GameStateManager.SetState(Enums.GameStateType.Idle);
            else
                GameStateManager.RestorePreviousState();
            CloseMenu();
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
