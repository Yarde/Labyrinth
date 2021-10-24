using UnityEngine.UI;

namespace UI.Windows
{
    public class DeadScreen : WindowState
    {
        public Button again;
        public Button menu;

        public override void Setup()
        {
            again.onClick.AddListener(PlayAgain);
            menu.onClick.AddListener(MainMenu);
        }

        private void OnDestroy()
        {
            again.onClick.RemoveListener(PlayAgain);
            menu.onClick.RemoveListener(MainMenu);
        }
        
        public override void OnEnter()
        {
            Pause();
        }

        public override void OnExit()
        {
        }
    }
}
