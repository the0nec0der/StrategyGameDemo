using System;
using System.Collections;

using DG.Tweening;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Misc
{
    public class TransitionController : MenuControllerBase
    {
        public event OnFadeToBlackEventHandler OnFadeToBlackStarted = null;
        public event OnFadeToBlackEventHandler OnFadeToBlackFinished = null;
        public event OnFadeFromBlackEventHandler OnFadeFromBlackStarted = null;
        public event OnFadeFromBlackEventHandler OnFadeFromBlackFinished = null;

        [SerializeField] private Image image = null;
        [SerializeField] private Ease ease = Ease.OutSine;

        private bool IsFaded = false;

        private Coroutine currentCoroutine = null;

        public void FadeToBlack(float duration, float delay = 0f, Action onFadeStarted = null, Action onFadeFinish = null, bool force = false)
        {
            if (currentCoroutine != null)
            {
                if (!force)
                {
                    Debug.LogError($"{nameof(FadeToBlack)} is already in progress. A new fade-to-black operation cannot be started until the current one finishes.");
                    return;
                }
                StopCoroutine(currentCoroutine);
            }

            currentCoroutine = StartCoroutine(FadeToBlackCoroutine(duration, delay, onFadeStarted, onFadeFinish, force));
        }

        private IEnumerator FadeToBlackCoroutine(float duration, float delay, Action onFadeStarted, Action onFadeFinish, bool force = false)
        {
            if (IsFaded && force)
                image.color = new(image.color.r, image.color.g, image.color.b, 0.0f);

            yield return new WaitForSeconds(delay);

            OpenMenu();

            onFadeStarted?.Invoke();
            OnFadeToBlackStarted?.Invoke(this, duration);
            image.raycastTarget = true;

            image.DOFade(1.0f, duration).SetEase(ease).OnComplete(() =>
            {
                IsFaded = true;
                OnFadeToBlackFinished?.Invoke(this, duration);
                onFadeFinish?.Invoke();
            });

            currentCoroutine = null;
        }

        public void FadeFromBlack(float duration, float delay = 0f, Action onFadeStarted = null, Action onFadeFinish = null, bool force = false)
        {
            if (currentCoroutine != null)
            {
                if (!force)
                {
                    Debug.LogError($"{nameof(FadeToBlack)} is already in progress. A new fade-to-black operation cannot be started until the current one finishes.");
                    return;
                }
                StopCoroutine(currentCoroutine);
            }

            currentCoroutine = StartCoroutine(FadeFromBlackCoroutine(duration, delay, onFadeStarted, onFadeFinish, force));
        }

        private IEnumerator FadeFromBlackCoroutine(float duration, float delay, Action onFadeStarted, Action onFadeFinish, bool force = false)
        {
            if (!IsFaded && force)
                image.color = new(image.color.r, image.color.g, image.color.b, 1.0f);

            yield return new WaitForSeconds(delay);

            onFadeStarted?.Invoke();
            OnFadeFromBlackStarted?.Invoke(this, duration);

            image.DOFade(0.0f, duration).SetEase(ease).OnComplete(() =>
            {
                CloseMenu();

                IsFaded = false;
                image.raycastTarget = false;
                OnFadeFromBlackFinished?.Invoke(this, duration);
                onFadeFinish?.Invoke();
            });

            currentCoroutine = null;
        }

        public delegate void OnFadeFromBlackEventHandler(TransitionController sender, float duration);
        public delegate void OnFadeToBlackEventHandler(TransitionController sender, float duration);
    }
}
