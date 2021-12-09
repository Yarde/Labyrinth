using Cysharp.Threading.Tasks;
using Gameplay;
using Network;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class MenuWindow : MonoBehaviour
    {
        [SerializeField] private LoginPanel loginPanel;
        [SerializeField] private Button feedbackButton;
        [SerializeField] private Button bugButton;
        [SerializeField] private TMP_InputField newHost;
        [SerializeField] private Button applyHostButton;

        public async UniTask<StartGameResponse> ShowMenu()
        {
            loginPanel.Setup();
            feedbackButton.onClick.AddListener(() =>
            {
                Application.OpenURL("https://forms.gle/Uq4spo1D81nwj6Cw5");
            });
            
            bugButton.onClick.AddListener(() =>
            {
                Application.OpenURL("mailto:zpi.clientapp@gmail.com");
            });
            
            applyHostButton.onClick.AddListener(ChangeHost);
            
            await UniTask.WaitUntil(() => loginPanel.StartGameResponse != null);
            return loginPanel.StartGameResponse;
        }

        private void ChangeHost()
        {
            ConnectionManager.Instance.ChangeHost(newHost.text);
        }
    }
}