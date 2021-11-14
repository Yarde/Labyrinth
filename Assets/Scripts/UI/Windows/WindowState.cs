using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Windows
{
    public abstract class WindowState : MonoBehaviour
    {
        public bool IsOnTop { get; set; }
        
        public void Pause()
        {
            IsOnTop = true;
            gameObject.SetActive(true);
            GameRoot.IsPaused = true;
        }
        
        public void Resume()
        {
            IsOnTop = false;
            gameObject.SetActive(false);
            GameRoot.IsPaused = false;
        }
        
        public void PlayAgain()
        {
            GameRoot.IsPaused = false;
            SceneManager.LoadScene("Scene");
        }

        public void MainMenu()
        {
            GameRoot.IsPaused = false;
            SceneManager.LoadScene("Scene");
        }
    }
}