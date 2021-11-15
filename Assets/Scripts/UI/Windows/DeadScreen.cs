using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows
{
    public class DeadScreen : WindowState
    {
        [SerializeField] private Button menu;
        [SerializeField] private GameObject loading;

        public void Setup()
        {
            menu.onClick.AddListener(MainMenu);
            menu.gameObject.SetActive(false);
            loading.gameObject.SetActive(true);
            
            gameObject.SetActive(true);
        }

        private void OnDestroy()
        {
            menu.onClick.RemoveListener(MainMenu);
        }

        public void OnRequestSent()
        {
            menu.gameObject.SetActive(true);
            loading.gameObject.SetActive(false);
        }
    }
}
