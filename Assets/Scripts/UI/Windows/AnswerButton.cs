using System;
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

        public bool IsSelected { get; private set; }
        public uint AnswerId { get; private set; }
        private Question.Types.Answer _answer;
        private Action _onClick;

        public void Setup(Question.Types.Answer answer, Action onClick)
        {
            _answer = answer;
            AnswerId = answer.AnswerID;
            
            button.interactable = true;
            text.text = answer.Content;
            button.onClick.AddListener(Select);
            _onClick = onClick;

            Unselect();
        }

        private void Select()
        {
            button.onClick.RemoveListener(Select);
            button.onClick.AddListener(Unselect);
            
            frame.color = Color.blue;
            IsSelected = true;
            _onClick.Invoke();
        }

        public void Unselect()
        {
            button.onClick.RemoveListener(Unselect);
            button.onClick.AddListener(Select);
            
            IsSelected = false;
            frame.color = Color.white;
            
            _onClick.Invoke();
        }

        public void ResolveQuestion()
        {
            button.interactable = false;
            if (_answer.Correct)
            {
                if (IsSelected)
                {
                    frame.color = Color.green;
                }
                else
                {
                    frame.color = Color.yellow;
                }
            }
            else
            {
                if (IsSelected)
                {
                    frame.color = Color.red;
                }
            }
        }
    }
}