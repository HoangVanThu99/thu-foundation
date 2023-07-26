﻿#pragma warning disable 649
using System;
using System.Collections.Generic;
using System.Globalization;
using Pancake.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Pancake.Loader
{
    [RequireComponent(typeof(CanvasGroup))]
    [AddComponentMenu("")]
    public sealed class LoadingComponent : MonoBehaviour
    {
        internal static LoadingComponent instance;

        #region spinner

        public Color spinnerColor = Color.white;
        public SpinnerItem spinnerItem;
        public Transform spinnerParent;
        public int spinnerIndex = 0;

        #endregion

        #region background

        public Image background;
        public Sprite singleBackgroundSprite;
        public List<Sprite> backgroundCollection = new List<Sprite>();
        public bool isAutoChangeBg;
        public float timeAutoChangeBg = 5;
        [Range(0.1f, 5)] public float backgroundFadingSpeed = 1;
        public Animator backgroundAnimator;
        private bool _enableFading;
        private float _currentTimeChangeBg;

        #endregion

        #region status label

        public bool enableStatusLabel;
        public TextMeshProUGUI txtStatus;
        public TMP_FontAsset statusFont;
        public float statusSize = 24;
        public Color statusColor = Color.white;
        public string statusSchema = "Loading {0}%";

        #endregion

        #region hints

        public bool isHints;
        public TextMeshProUGUI txtHints;
        public TMP_FontAsset hintsFont;
        public Color hintsColor = Color.white;
        public float hintsFontSize;
        public bool isChangeHintsWithTimer;
        public float hintsLifeTime = 5; // this value will used when isChangeHintsWithTimer equal true
        [TextArea] public List<string> hintsCollection;
        private float _currentHintsTime;

        #endregion

        #region other

        public CanvasGroup canvasGroup;
        public Slider progressBar;
        public Animator mainLoadingAnimator;

        [Tooltip("Second(s)")] public float virtualLoadTime = 2;
        private float _currentTimeLoading;

        public bool enableRandomBackground = true;

        [Range(0.1f, 10)] public float fadingAnimationSpeed = 2.0f;
        [Range(0.1f, 10)] public float timeDelayDestroy = 1.5f;

        public UnityEvent onBeginEvents;
        public UnityEvent onFinishEvents;

        private static bool processLoading;
        public bool updateHelper = false;
        private bool _onFinishInvoked;
        private AsyncOperation _loadingOperation;
        private AsyncOperation _loadingOperationSubScene;
        private Func<bool> _waitingComplete;
        private Func<bool> _funcWaitBeforeLoadScene;
        private Action _prepareActiveScene;
        private bool _isWaitingPrepareActiveScene;
        private UnityEvent _onFinishAction;

        #endregion

        private void Awake()
        {
            if (canvasGroup == null) canvasGroup = gameObject.GetComponent<CanvasGroup>();

            canvasGroup.alpha = 0f;
        }

        private void Start()
        {
            if (isHints)
            {
                txtHints.text = hintsCollection.PickRandom();
            }

            if (enableRandomBackground)
            {
                background.sprite = backgroundCollection.PickRandom();
            }
            else
            {
                backgroundAnimator.enabled = false;
                background.sprite = singleBackgroundSprite;
            }

            backgroundAnimator.speed = backgroundFadingSpeed;
            progressBar.value = 0;
        }

        public void LoadScene(
            string sceneName,
            Func<bool> funcWaiting = null,
            Func<bool> funcWaitBeforeLoadScene = null,
            Action prepareActiveScene = null,
            UnityEvent onBeginEvent = null,
            UnityEvent onFinishEvent = null)
        {
            processLoading = true;
            DontDestroyOnLoad(gameObject);
            _waitingComplete = funcWaiting;
            _prepareActiveScene = prepareActiveScene;
            _funcWaitBeforeLoadScene = funcWaitBeforeLoadScene;
            _onFinishAction = onFinishEvent;
            _isWaitingPrepareActiveScene = false;
            gameObject.SetActive(true);

            onBeginEvent?.Invoke();
            onBeginEvents.Invoke();
            WaitDoneBeforeLoadSceneAsync(sceneName);
        }

        private async void WaitDoneBeforeLoadSceneAsync(string sceneName)
        {
            if (_funcWaitBeforeLoadScene != null) await UniTask.WaitUntil(_funcWaitBeforeLoadScene);

            _loadingOperation = SceneManager.LoadSceneAsync(sceneName);
            _loadingOperation.allowSceneActivation = false;
        }

        public void LoadScene(
            string sceneName,
            string subScene,
            Func<bool> funcWaiting = null,
            Func<bool> funcWaitBeforeLoadScene = null,
            Action prepareActiveScene = null,
            UnityEvent onBeginEvent = null,
            UnityEvent onFinishEvent = null)
        {
            processLoading = true;
            DontDestroyOnLoad(gameObject);
            _waitingComplete = funcWaiting;
            _prepareActiveScene = prepareActiveScene;
            _funcWaitBeforeLoadScene = funcWaitBeforeLoadScene;
            _onFinishAction = onFinishEvent;
            _isWaitingPrepareActiveScene = false;
            gameObject.SetActive(true);

            onBeginEvent?.Invoke();
            onBeginEvents.Invoke();
            WaitDoneBeforeLoadSceneAsyncSub(sceneName, subScene);
        }

        private async void WaitDoneBeforeLoadSceneAsyncSub(string sceneName, string subScene)
        {
            if (_funcWaitBeforeLoadScene != null) await UniTask.WaitUntil(_funcWaitBeforeLoadScene);

            _loadingOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            _loadingOperationSubScene = SceneManager.LoadSceneAsync(subScene, LoadSceneMode.Additive);
            _loadingOperation.allowSceneActivation = false;
            _loadingOperationSubScene.allowSceneActivation = false;
        }

        private void Update()
        {
            if (processLoading)
            {
                #region virtual loading

                if (progressBar.value < 0.4f)
                {
                    progressBar.value += 1 / virtualLoadTime / 3 * Time.deltaTime;
                    _currentTimeLoading += Time.deltaTime / 3f;
                }
                else
                {
                    progressBar.value += 1 / virtualLoadTime * Time.deltaTime;
                    _currentTimeLoading += Time.deltaTime;
                }

                txtStatus.text = string.Format(statusSchema, Mathf.Round(progressBar.value * 100).ToString(CultureInfo.InvariantCulture));

                if (_currentTimeLoading >= virtualLoadTime)
                {
                    if (_prepareActiveScene != null && !_isWaitingPrepareActiveScene)
                    {
                        _isWaitingPrepareActiveScene = true;
                        _prepareActiveScene.Invoke();
                    }

                    if (_waitingComplete != null && !_waitingComplete.Invoke()) return;

                    if (_loadingOperation != null)
                    {
                        _loadingOperation.allowSceneActivation = true;
                        if (_loadingOperationSubScene != null) _loadingOperationSubScene.allowSceneActivation = true;
                        canvasGroup.alpha -= fadingAnimationSpeed * Time.deltaTime;

                        if (canvasGroup.alpha <= 0) Destroy(gameObject);

                        if (_onFinishInvoked == false)
                        {
                            onFinishEvents.Invoke();
                            _onFinishAction?.Invoke();
                            _onFinishInvoked = true;
                        }
                    }
                }
                else
                {
                    if (canvasGroup.alpha < 1) canvasGroup.alpha += fadingAnimationSpeed * Time.deltaTime;
                }

                #endregion

                if (enableRandomBackground)
                {
                    if (isAutoChangeBg)
                    {
                        _currentTimeChangeBg += Time.deltaTime;

                        if (_currentTimeChangeBg >= timeAutoChangeBg && backgroundAnimator.GetCurrentAnimatorStateInfo(0).IsName("bg_fadein"))
                            backgroundAnimator.Play("bg_fadeout");

                        else if (_currentTimeChangeBg >= timeAutoChangeBg && backgroundAnimator.GetCurrentAnimatorStateInfo(0).IsName("bg_wait"))
                        {
                            var cloneHelper = background.sprite;
                            var imageChose = backgroundCollection.PickRandom();

                            if (imageChose == cloneHelper) imageChose = backgroundCollection.PickRandom();

                            background.sprite = imageChose;
                            _currentTimeChangeBg = 0.0f;
                        }
                    }

                    else
                    {
                        background.sprite = backgroundCollection.PickRandom();

                        if (background.color != new Color32(255, 255, 255, 255))
                            background.color = new Color32(255, 255, 255, 255);

                        backgroundAnimator.enabled = false;
                        enableRandomBackground = false;
                    }
                }

                if (isHints && isChangeHintsWithTimer)
                {
                    _currentHintsTime += Time.deltaTime;

                    if (_currentHintsTime >= hintsLifeTime)
                    {
                        var tempHints = txtHints.text;
                        var nextHints = hintsCollection.PickRandom();
                        if (tempHints.Equals(nextHints)) nextHints = hintsCollection.PickRandom();
                        txtHints.text = nextHints;
                        _currentHintsTime = 0.0f;
                    }
                }
            }
        }

        private IEnumerator<float> DestroyLoadingScreen()
        {
            yield return Timing.WaitForSeconds(timeDelayDestroy);
            Destroy(gameObject);
        }
    }
}