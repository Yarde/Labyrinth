using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.Windows
{
    public class PauseScreen : WindowState
    {
        [SerializeField] private  Button resume;
        [SerializeField] private  Button menu;

        public void Setup(UnityAction resumeGame)
        {
            resume.onClick.AddListener(() =>
            {
                resumeGame();
                Resume();
            });
            menu.onClick.AddListener(MainMenu);
        }
        
        private void OnDestroy()
        {
            menu.onClick.RemoveListener(MainMenu);
            resume.onClick.RemoveAllListeners();
        }
    }
}
