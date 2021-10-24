using Gameplay;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows
{
    public class AnswerButton : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private Image frame;

        public bool IsSelected { get; set; }

        public void Setup(Question.Types.Answer answer)
        {
            button.interactable = true;
            text.text = answer.Content;
            button.onClick.AddListener(Select);

            Unselect();
        }

        public void Select()
        {
            frame.color = Color.blue;
            IsSelected = true;
        }

        public void Unselect()
        {
            IsSelected = false;
            frame.color = Color.white;
        }

        public void BlockButton()
        {
            button.interactable = false;
        }

        public void MarkCorrect()
        {
            frame.color = Color.white;
        }
        
        public void MarkWrong()
        {
            frame.color = Color.white;
        }
        
        public void MarkMissed()
        {
            frame.color = Color.white;
        }
    }
}