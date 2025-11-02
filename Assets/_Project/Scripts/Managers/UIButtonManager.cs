using UnityEngine;
using Utilities;

namespace Managers
{
    public class UIButtonManager : Singleton<UIButtonManager>
    {
        public void OnPlayAgainButtonClicked()
        {
            Time.timeScale = 1f;
            UIManager.Instance.HideAllPanels();
            UIManager.Instance.LoadSceneWithLoadingScreen("MainLevel");
        }

        public void OnFinishButtonClicked()
        {
            UIManager.Instance.HideAllPanels();
            UIManager.Instance.ShowPanel("MainMenu");
        }

        public void OnTutorial_1_ButtonClicked()
        {
            UIManager.Instance.HideAllPanels();
            UIManager.Instance.ShowPanel("Tutorial_Menu_1");
        }

        public void OnTutorial_2_ButtonClicked()
        {
            UIManager.Instance.HideAllPanels();
            UIManager.Instance.ShowPanel("Tutorial_Menu_2");
        }
        
        public void OnTutorial_3_ButtonClicked()
        {
            UIManager.Instance.HideAllPanels();
            UIManager.Instance.ShowPanel("Tutorial_Menu_3");
        }
        
        public void OnTutorial_4_ButtonClicked()
        {
            UIManager.Instance.HideAllPanels();
            UIManager.Instance.ShowPanel("Tutorial_Menu_4");
        }
        
        public void OnTutorial_5_ButtonClicked()
        {
            UIManager.Instance.HideAllPanels();
            UIManager.Instance.ShowPanel("Tutorial_Menu_5");
        }
        
        public void OnTutorial_6_ButtonClicked()
        {
            UIManager.Instance.HideAllPanels();
            UIManager.Instance.ShowPanel("Tutorial_Menu_6");
        }

        public void OnQuitButtonClicked()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        public void OnBackButtonClicked()
        {
            UIManager.Instance.BackToPrevious();
        }
    }
}