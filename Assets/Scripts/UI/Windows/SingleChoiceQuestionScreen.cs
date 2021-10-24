using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Gameplay;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows
{
    public class SingleChoiceQuestionScreen : QuestionScreenBase
    {
        [SerializeField] private TextMeshProUGUI timer;
        [SerializeField] private AnswerButton answerButtonPrefab;
        [SerializeField] private Transform answerButtonHolder;
        [SerializeField] private Button confirmButton;
        [SerializeField] private TextMeshProUGUI questionText;
        
        private float startTime;
        private bool finished;

        private List<AnswerButton> _answers;
        
        public override async UniTask DisplayQuestion(Question question)
        {
            startTime = Time.realtimeSinceStartup;
            finished = false;
            confirmButton.onClick.AddListener(ConfirmChoice);
            confirmButton.interactable = false;

            questionText.text = question.Content;

            _answers = new List<AnswerButton>();
            foreach (var answer in question.Answers)
            {
                var newAnswer = Instantiate(answerButtonPrefab, answerButtonHolder);
                newAnswer.Setup(answer, () => OnAnswerClicked(answer.AnswerID));
                _answers.Add(newAnswer);
            }

            await UniTask.WaitUntil(() => finished);
        }

        private void OnAnswerClicked(uint clickedId)
        {
            var clicked = _answers.Where(x => x.IsSelected).ToList();
            confirmButton.interactable = clicked.Count > 0;

            // todo this is only for single choice question,
            // make separate implementation for single and multiple choice questions
            if (clicked.Count > 0)
            {
                foreach (var button in clicked)
                {
                    if (button.AnswerId != clickedId)
                    {
                        button.Unselect();
                    }
                }
            }
        }

        private void ConfirmChoice()
        {
            foreach (var button in _answers)
            {
                button.ResolveQuestion();
            }
            
            // todo animate reward or heart lost
            
            
            
            finished = true;
        }

        private void Update()
        {
            var timePassed = Mathf.Round((Time.realtimeSinceStartup - startTime) * 10f) / 10f;
            timer.text = $"Time passed: {timePassed}sec";
        }
    }
}