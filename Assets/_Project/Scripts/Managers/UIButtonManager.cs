using Utilities;

namespace Managers
{
    public class UIButtonManager : Singleton<UIButtonManager>
    {
        public void OnPlayAgainButtonClicked()
        {
            UIManager.Instance.HideAllPanels();
            UIManager.Instance.LoadSceneWithLoadingScreen("MainLevel");
        }

        public void OnAudioButtonClicked()
        {
            UIManager.Instance.ShowPanel("AudioPanel");
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
