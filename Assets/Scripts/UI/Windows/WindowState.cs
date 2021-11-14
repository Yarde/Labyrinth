using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Windows
{
    public abstract class WindowState : MonoBehaviour
    {
        public abstract void Setup();
        public abstract void OnEnter();
        public abstract void OnExit();
        
        public bool IsOnTop { get; set; }
        
        protected void Pause()
        {
            IsOnTop = true;
            gameObject.SetActive(true);
            GameRoot.IsPaused = true;
        }
        
        protected void Resume()
        {
            IsOnTop = false;
            gameObject.SetActive(false);
            GameRoot.IsPaused = false;
        }
        
        protected void PlayAgain()
        {
            GameRoot.IsPaused = false;
            SceneManager.LoadScene("Scene");
        }

        protected void MainMenu()
        {
            GameRoot.IsPaused = false;
            SceneManager.LoadScene("Scene");
        }
    }
}