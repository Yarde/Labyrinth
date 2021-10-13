using UnityEngine.UI;

namespace UI.Windows
{
    public class PauseScreen : WindowState
    {
        public Button resume;
        public Button menu;

        public override void OnEnter()
        {
            Pause();
            resume.onClick.AddListener(Resume);
            menu.onClick.AddListener(MainMenu);
        }

        public override void OnExit()
        {
            Resume();
            resume.onClick.RemoveListener(Resume);
            menu.onClick.RemoveListener(MainMenu);
        }
    }
}
