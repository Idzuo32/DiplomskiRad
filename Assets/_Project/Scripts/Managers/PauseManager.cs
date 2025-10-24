using System;
using Player;
using UnityEngine;
using Utilities;

namespace Managers
{
    public class PauseManager : MonoBehaviour
    {
        bool _isPaused;

        void OnEnable()
        {
            InputManager.Instance.MenuOpenCloseEvent += HandlePause;
        }

        void OnDisable()
        {
            if (InputManager.Instance != null)
            {
                InputManager.Instance.MenuOpenCloseEvent -= HandlePause;
            }
        }

        public void Start()
        {
            Unpause();
        }

        public void HandlePause()
        {
            if (_isPaused)
            {
                Unpause();
            }
            else
            {
                Pause();
            }
        }

        void Pause()
        {
            _isPaused = true;
            Time.timeScale = 0;
            UIManager.Instance.ShowPanel("PausePanel");
            UIManager.Instance.FadeScreen(true);
        }

        void Unpause()
        {
            _isPaused = false;
            Time.timeScale = 1;
            UIManager.Instance.HideAllPanels();
            UIManager.Instance.ShowPanel("GamePanel");
        }

        public void OnResumeButtonClick()
        {
            Unpause();
        }
    }
}