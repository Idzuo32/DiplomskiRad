using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Utilities;

namespace Managers
{
    public class UIManager : Singleton<UIManager>
    {
        #region Initialization

        public void Start()
        {
            InitializeEventSystem();
            SetupPanels();
        }

        #endregion

        #region Panel Management

        [SerializeField] bool startWithFirstPanel;
        [SerializeField] GameObject firstSelectedButton; // Default UI focus
        readonly Dictionary<string, GameObject> _panels = new Dictionary<string, GameObject>();
        readonly Stack<GameObject> _panelStack = new Stack<GameObject>(); // For back navigation

        [SerializeField] List<UIPanel> registeredPanels = new List<UIPanel>();

        [SerializeField] CanvasGroup screenFader;
        [SerializeField] float fadeDuration = 1f;
        [SerializeField] GameObject loadingScreen;
        [SerializeField] UnityEngine.UI.Slider loadingProgressBar;
        [System.Serializable]
        public class UIPanel
        {
            public string panelID;
            public GameObject panelObject;
        }

        void SetupPanels()
        {
            foreach (var panel in registeredPanels)
            {
                if (panel.panelObject != null)
                {
                    _panels.Add(panel.panelID, panel.panelObject);
                    panel.panelObject.SetActive(false);
                }
            }
            if (startWithFirstPanel && registeredPanels.Count > 0)
            {
                ShowPanel(registeredPanels[0].panelID);
            }
        }

        public void ShowPanel(string panelID, bool addToStack = true)
        {
            if (_panels.TryGetValue(panelID, out var panel))
            {
                if (addToStack && _panelStack.Count > 0)
                {
                    _panelStack.Peek().SetActive(false);
                }

                panel.SetActive(true);

                if (addToStack)
                {
                    _panelStack.Push(panel);
                }

                UpdateEventSystemFocus(panel);
            }
            else
            {
                Debug.LogWarning($"Panel {panelID} not found!");
            }
        }

        void HidePanel(string panelID)
        {
            if (_panels.TryGetValue(panelID, out var panel))
            {
                panel.SetActive(false);
            }
        }

        public void BackToPrevious()
        {
            if (_panelStack.Count <= 1) return;

            var currentPanel = _panelStack.Pop();
            currentPanel.SetActive(false);

            var previousPanel = _panelStack.Peek();
            previousPanel.SetActive(true);

            UpdateEventSystemFocus(previousPanel);
        }

        public void HideAllPanels()
        {
            foreach (var panel in _panels.Values)
            {
                panel.SetActive(false);
            }

            _panelStack.Clear();
        }

        #endregion

        #region Event System Management

        void InitializeEventSystem()
        {
            if (FindFirstObjectByType<EventSystem>()) return;
            var eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();
        }

        void UpdateEventSystemFocus(GameObject panel)
        {
            EventSystem.current.SetSelectedGameObject(null);
            var selectable = panel.GetComponentInChildren<UnityEngine.UI.Selectable>();
            EventSystem.current.SetSelectedGameObject(selectable != null ? selectable.gameObject : firstSelectedButton);
        }

        #endregion

        #region Screen Transitions   
        public void FadeScreen(bool fadeToBlack, UnityAction onComplete = null)
        {
            StartCoroutine(FadeRoutine(fadeToBlack ? 1 : 0, onComplete));
        }

        IEnumerator FadeRoutine(float targetAlpha, UnityAction onComplete)
        {
            screenFader.blocksRaycasts = true;
            var startAlpha = screenFader.alpha;
            var elapsed = 0f;

            while (elapsed < fadeDuration)
            {
                screenFader.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / fadeDuration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            screenFader.alpha = targetAlpha;
            screenFader.blocksRaycasts = Mathf.Approximately(targetAlpha, 1);
            onComplete?.Invoke();
        }

        #endregion

        #region Scene Loading

        public void LoadSceneWithLoadingScreen(string sceneName)
        {
            StartCoroutine(LoadSceneAsync(sceneName));
        }

        IEnumerator LoadSceneAsync(string sceneName)
        {
            ShowPanel(loadingScreen.name, false);
            var operation = SceneManager.LoadSceneAsync(sceneName);

            while (operation is { isDone: false })
            {
                var progress = Mathf.Clamp01(operation.progress / 0.9f);
                loadingProgressBar.value = progress;
                yield return null;
            }

            HidePanel(loadingScreen.name);
        }

        #endregion

        #region Device Detection

        bool _usingGamepad;

        void Update()
        {
            CheckInputDevice();
        }

        void CheckInputDevice()
        {
            var gamepadInput = Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f ||
                               Mathf.Abs(Input.GetAxis("Vertical")) > 0.1f;

            var mouseInput = Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0;

            if (mouseInput && _usingGamepad)
            {
                _usingGamepad = false;
                EventSystem.current.SetSelectedGameObject(null);
                Cursor.visible = true;
            }
            else if (gamepadInput && !_usingGamepad)
            {
                _usingGamepad = true;
                Cursor.visible = false;
                if (_panelStack.Count > 0)
                {
                    UpdateEventSystemFocus(_panelStack.Peek());
                }
            }
        }

        #endregion
    }
}