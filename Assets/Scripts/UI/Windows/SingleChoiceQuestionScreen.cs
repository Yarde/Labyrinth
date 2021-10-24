using System.Collections.Generic;
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
                newAnswer.Setup(answer);
                _answers.Add(newAnswer);
            }

            await UniTask.WaitUntil(() => finished);
        }

        private void ConfirmChoice()
        {
            finished = true;
        }

        private void Update()
        {
            var timePassed = Mathf.Round((Time.realtimeSinceStartup - startTime) * 10f) / 10f;
            timer.text = $"Time passed: {timePassed}sec";
        }
    }
}