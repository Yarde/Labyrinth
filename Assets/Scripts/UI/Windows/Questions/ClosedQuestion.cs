using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Gameplay;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows
{
    public abstract class ClosedQuestion : QuestionScreenBase
    {
        [SerializeField] private TextMeshProUGUI timer;
        [SerializeField] private AnswerButton answerButtonPrefab;
        [SerializeField] private Transform answerButtonHolder;
        [SerializeField] protected Button confirmButton;
        [SerializeField] private TextMeshProUGUI questionText;
        
        private float startTime;
        private bool finished;

        protected List<AnswerButton> _answers;
        
        public override async UniTask DisplayQuestion(Question question)
        {
            startTime = Time.realtimeSinceStartup;
            finished = false;
            confirmButton.onClick.AddListener(ConfirmChoice);
            confirmButton.interactable = false;

            questionText.text = question.Content;

            SpawnAnswers(question);

            await UniTask.WaitUntil(() => finished);
        }

        private void SpawnAnswers(Question question)
        {
            _answers = new List<AnswerButton>();
            foreach (var answer in question.Answers)
            {
                var newAnswer = Instantiate(answerButtonPrefab, answerButtonHolder);
                newAnswer.Setup(answer, () => OnAnswerClicked(answer.AnswerID));
                _answers.Add(newAnswer);
            }
        }

        protected abstract void OnAnswerClicked(uint clickedId);

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