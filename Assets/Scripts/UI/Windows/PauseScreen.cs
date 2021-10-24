using UnityEngine.UI;

namespace UI.Windows
{
    public class PauseScreen : WindowState
    {
        public Button resume;
        public Button menu;

        public override void Setup()
        {
            resume.onClick.AddListener(Resume);
            menu.onClick.AddListener(MainMenu);
        }

        private void OnDestroy()
        {
            resume.onClick.RemoveListener(Resume);
            menu.onClick.RemoveListener(MainMenu);
        }

        public override void OnEnter()
        {
            Pause();
        }

        public override void OnExit()
        {
            Resume();
        }
    }
}
