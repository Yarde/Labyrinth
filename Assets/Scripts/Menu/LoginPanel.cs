using Cysharp.Threading.Tasks;
using Gameplay;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;
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
        
        // todo change to BE data
        public StartGameResponse Data { get; set; }

        public void Setup()
        {
            start.onClick.AddListener(RunGame);
        }

        private async void RunGame()
        {
            if (ValidateTexts())
            {
                StartGameResponse response = await SendStartGameRequest();

                if (response.Error)
                {
                    serverError.gameObject.SetActive(true);
                    serverError.text = response.ErrorMsg;
                }
                else
                {
                    Data = response;
                }
            }
        }
        private async UniTask<StartGameResponse> SendStartGameRequest()
        {
            var request = new StartGameRequest
            {
                Email = email.text,
                Code = code.text
            };
            var response = await ConnectionManager.Instance.GetMessageAsync<StartGameResponse>(request, "dawid/sth");
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