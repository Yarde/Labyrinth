using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Windows
{
    public abstract class WindowState : MonoBehaviour
    {
        public void Pause()
        {
            gameObject.SetActive(true);
        }
        
        public void Resume()
        {
            gameObject.SetActive(false);
        }

        protected void MainMenu()
        {
            GameRoot.IsPaused = false;
            SceneManager.LoadScene("Scene");
        }
    }
}