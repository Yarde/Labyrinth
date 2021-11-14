using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Gameplay;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows.Questions
{
    public class AnswerButton : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private Image frame;
        
        [Header("Color of answers")]
        [SerializeField] private Color defaultColor;
        [SerializeField] private Color selectedColor;
        [SerializeField] private Color correctColor;
        [SerializeField] private Color wrongColor;

        public bool IsSelected { get; private set; }
        public uint AnswerId { get; private set; }
        private QuestionResponse.Types.Answer _answer;
        private Action _onClick;

        public void Setup(QuestionResponse.Types.Answer answer, Action onClick)
        {
            _answer = answer;
            AnswerId = answer.AnswersID;
            
            button.interactable = true;
            text.text = answer.Content;
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(Select);
            _onClick = onClick;

            Unselect();
        }

        private void Select()
        {
            button.onClick.RemoveListener(Select);
            button.onClick.AddListener(UnselectOnClick);
            
            frame.color = selectedColor;
            IsSelected = true;
            _onClick.Invoke();
        }

        private void UnselectOnClick()
        {
            Unselect();
            _onClick.Invoke();
        }
        
        public void Unselect()
        {
            button.onClick.RemoveListener(UnselectOnClick);
            button.onClick.AddListener(Select);
            
            IsSelected = false;
            frame.color = defaultColor;
        }

        public async UniTask<bool> ResolveQuestion()
        {
            button.interactable = false;

            if (!IsSelected && !_answer.Correct) 
                return true;
            
            if (_answer.Correct)
            {
                await transform.DOShakeScale(0.5f, 0.3f);
                frame.color = correctColor;
                return IsSelected;
            }
            
            frame.color = wrongColor;
            return false;
        }
    }
}