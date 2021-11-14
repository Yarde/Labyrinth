using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows
{
    public class PauseScreen : WindowState
    {
        [SerializeField] private  Button resume;
        [SerializeField] private  Button menu;

        public void Setup()
        {
            resume.onClick.AddListener(Resume);
            menu.onClick.AddListener(MainMenu);
        }

        private void OnDestroy()
        {
            resume.onClick.RemoveListener(Resume);
            menu.onClick.RemoveListener(MainMenu);
        }
    }
}
