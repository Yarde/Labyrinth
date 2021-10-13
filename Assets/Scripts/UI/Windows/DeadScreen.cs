using UnityEngine.UI;

namespace UI.Windows
{
    public class DeadScreen : WindowState
    {
        public Button again;
        public Button menu;

        public override void OnEnter()
        {
            Pause();
            again.onClick.AddListener(PlayAgain);
            menu.onClick.AddListener(MainMenu);
        }

        public override void OnExit()
        {
            
            again.onClick.RemoveListener(PlayAgain);
            menu.onClick.RemoveListener(MainMenu);
        }
    }
}
