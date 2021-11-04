using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Gameplay;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows
{
    public abstract class ClosedQuestion : QuestionScreenBase
    {
        [SerializeField] private Timer timer;
        [SerializeField] private AnswerButton answerButtonPrefab;
        [SerializeField] private Transform answerButtonHolder;
        [SerializeField] protected Button confirmButton;
        [SerializeField] private TextMeshProUGUI questionText;

        [SerializeField] private RewardPopup rewardPopupPrefab;
        
        private bool finished;
        protected List<AnswerButton> _answers;
        private RewardPopup _rewardPopup;

        private bool correct;
        
        public override async UniTask<bool> DisplayQuestion(Question question)
        {
            timer.StartTimer();

            finished = false;
            correct = true;
            confirmButton.onClick.AddListener(ConfirmChoice);
            confirmButton.interactable = false;

            questionText.text = question.Content;

            if (_answers != null && _answers.Count > 0)
            {
                UpdateAnswers(question);
            }
            else
            {
                SpawnAnswers(question);
            }

            await UniTask.WaitUntil(() => finished);

            return correct;
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
        
        private void UpdateAnswers(Question question)
        {
            // todo use pool instead to handle changing answer count
            for (var i = 0; i < question.Answers.Count; i++)
            {
                var answer = question.Answers[i];
                var button = _answers[i];
                button.Setup(answer, () => OnAnswerClicked(answer.AnswerID));
            }
        }

        protected abstract void OnAnswerClicked(uint clickedId);

        private async void ConfirmChoice()
        {
            timer.StopTimer();
            confirmButton.onClick.RemoveListener(ConfirmChoice);
            foreach (var button in _answers)
            {
                ResolveButton(button);
            }

            await UniTask.Delay(400);
            PresentResult().Forget();
        }

        private async Task ResolveButton(AnswerButton button)
        {
            var isCorrect = await button.ResolveQuestion();

            if (!isCorrect)
            {
                correct = false;
            }
        }

        private async UniTask PresentResult()
        {
            // todo animate reward or heart lost
            await UniTask.Delay(2000);

            finished = true;
        }
    }
}