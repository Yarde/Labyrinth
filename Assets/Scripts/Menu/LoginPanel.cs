using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Menu
{
    public class LoginPanel : MonoBehaviour
    {
        [SerializeField] private TMP_InputField email;
        [SerializeField] private TMP_InputField code;
        
        [SerializeField] private TextMeshProUGUI emailError;
        [SerializeField] private TextMeshProUGUI codeError;
        
        
        [SerializeField] private Button start;

        private void Start()
        {
            start.onClick.AddListener(RunGame);
        }

        private void RunGame()
        {
            if (ValidateTexts())
            {
                 // todo send some request to authenticate and some shit
                 SceneManager.LoadScene(1);
            }
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