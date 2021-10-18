using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public abstract class WindowState : MonoBehaviour
    {
        public abstract void OnEnter();
        public abstract void OnExit();
        
        public bool IsOnTop { get; set; }
        
        protected void Pause()
        {
            IsOnTop = true;
            gameObject.SetActive(true);
            Time.timeScale = 0;
        }
        
        protected void Resume()
        {
            IsOnTop = false;
            gameObject.SetActive(false);
            Time.timeScale = 1;
        }
        
        protected void PlayAgain()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("Scene");
        }

        protected void MainMenu()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("Scene");
        }
    }
}