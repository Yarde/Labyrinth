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
        [SerializeField] private Timer timer;
        [SerializeField] private AnswerButton answerButtonPrefab;
        [SerializeField] private Transform answerButtonHolder;
        [SerializeField] protected Button confirmButton;
        [SerializeField] private TextMeshProUGUI questionText;

        [SerializeField] private RewardPopup rewardPopupPrefab;
        
        private bool finished;
        protected List<AnswerButton> _answers;
        private RewardPopup _rewardPopup;
        
        public override async UniTask DisplayQuestion(Question question)
        {
            timer.StartTimer();

            finished = false;
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
        }
        
        public override async UniTask DisplayReward(QuestionResult result)
        {
            if (_rewardPopup == null)
            {
                _rewardPopup = Instantiate(rewardPopupPrefab, transform);
            }

            await _rewardPopup.DisplayReward(result);
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

        private void ConfirmChoice()
        {
            timer.StopTimer();
            confirmButton.onClick.RemoveListener(ConfirmChoice);
            foreach (var button in _answers)
            {
                button.ResolveQuestion();
            }
            
            PresentResult().Forget();
        }

        private async UniTask PresentResult()
        {
            // todo animate reward or heart lost
            await UniTask.Delay(2000);

            finished = true;
        }
    }
}