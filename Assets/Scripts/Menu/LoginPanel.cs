using Cysharp.Threading.Tasks;
using Gameplay;
using Network;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class LoginPanel : MonoBehaviour
    {
        [SerializeField] private TMP_InputField email;
        [SerializeField] private TMP_InputField code;
        
        [SerializeField] private TextMeshProUGUI emailError;
        [SerializeField] private TextMeshProUGUI codeError;
        [SerializeField] private TextMeshProUGUI serverError;
        
        [SerializeField] private Button start;
        [SerializeField] private Button debugStart;
        [SerializeField] private GameObject loadingIcon;
        
        [SerializeField] private string onConnectionFailed = "Unable to connect to the server. Please try again later.";
        
        public StartGameResponse StartGameResponse { get; set; }

        public void Setup()
        {
            start.onClick.AddListener(RunGame);
            debugStart.onClick.AddListener(DebugLogin);
        }

        private void DebugLogin()
        {
            var request = new StartGameRequest
            {
                Email = "test",
                Code = "test"
            };
            _ = SendDebugRequest(request);
        }

        private async UniTask SendDebugRequest(StartGameRequest request)
        {
            var response = await ConnectionManager.Instance.SendMessageAsync<StartGameResponse>(request, Endpoints.StartGame);
            StartGameResponse = response;
        }

        private async void RunGame()
        {
            if (ValidateTexts())
            {
                start.interactable = false;
                serverError.gameObject.SetActive(false);
                loadingIcon.SetActive(true);
                var response = await SendStartGameRequest();
                loadingIcon.SetActive(false);

                if (response.Error)
                {
                    serverError.gameObject.SetActive(true);
                    serverError.text = response.ErrorMsg;
                } else if (string.IsNullOrEmpty(response.SessionCode))
                {
                    serverError.gameObject.SetActive(true);
                    serverError.text = onConnectionFailed;
                }
                else
                {
                    StartGameResponse = response;
                }
                start.interactable = true;
            }
        }
        private async UniTask<StartGameResponse> SendStartGameRequest()
        {
            var request = new StartGameRequest
            {
                Email = email.text,
                Code = code.text
            };
            var response = await ConnectionManager.Instance.SendMessageAsync<StartGameResponse>(request, Endpoints.StartGame);
            return response;
        }

        private bool ValidateTexts()
        {
            var emailCheck = ValidateEmail();
            var codeCheck = ValidateCode();
            return emailCheck && codeCheck;
        }
        
        private bool ValidateEmail()
        {
            if (string.IsNullOrWhiteSpace(email.text))
            {
                emailError.gameObject.SetActive(true);
                emailError.text = "Email cannot be empty";
                return false;
            }
            emailError.gameObject.SetActive(false);
            return true;
        }
        
        private bool ValidateCode()
        {
            if (string.IsNullOrWhiteSpace(code.text))
            {
                codeError.gameObject.SetActive(true);
                codeError.text = "Code cannot be empty";
                return false;
            }
            codeError.gameObject.SetActive(false);
            return true;
        }
    }
}