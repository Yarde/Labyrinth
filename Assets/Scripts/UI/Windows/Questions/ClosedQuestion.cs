using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Gameplay;
using TMPro;
using UI.Elements;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows.Questions
{
    public abstract class ClosedQuestion : QuestionScreenBase
    {
        [SerializeField] private Timer timer;
        [SerializeField] private AnswerButton answerButtonPrefab;
        [SerializeField] private LayoutGroup answerButtonHolder;
        [SerializeField] protected Button confirmButton;
        [SerializeField] private TextMeshProUGUI questionText;
        [SerializeField] protected List<AnswerButton> answers;
        
        private bool finished;
        private RewardPopup _rewardPopup;

        private float _correct;
        private int _correctCount;

        public override async UniTask<StudentAnswerRequest> DisplayQuestion(QuestionResponse question)
        {
            gameObject.SetActive(true);
            timer.StartTimer();

            finished = false;
            _correct = 1.0f;
            confirmButton.onClick.AddListener(ConfirmChoice);
            confirmButton.interactable = false;

            questionText.text = question.Content;

            if (answers != null && answers.Count > 0)
            {
                UpdateAnswers(question);
            }
            else
            {
                SpawnAnswers(question);
            }

            _correctCount = question.Answers.Sum(x => x.Correct ? 1 : 0); 

            await UniTask.WaitUntil(() => finished);
            gameObject.SetActive(false);
            
            var request = new StudentAnswerRequest
                          {
                              QuestionType = QuestionTrigger.Enemy,
                              SessionCode = GameRoot.SessionCode,
                              AnswersID = { GetSelectedAnswers() },
                              TimeToAnswer = timer.ElapsedSeconds,
                              QuestionCorrectnes = _correct
                          };

            return request;
        }

        private IEnumerable<int> GetSelectedAnswers()
        {
            var answers = this.answers.FindAll(x => x.IsSelected).Select(y => y.AnswerId);
            return answers;
        }

        private void SpawnAnswers(QuestionResponse question)
        {
            answers = new List<AnswerButton>();
            answerButtonHolder.enabled = true;

            foreach (var answer in question.Answers)
            {
                var newAnswer = Instantiate(answerButtonPrefab, answerButtonHolder.transform);
                newAnswer.Setup(answer, () => OnAnswerClicked(answer.AnswersID));
                answers.Add(newAnswer);
            }
        }
        
        private void UpdateAnswers(QuestionResponse question)
        {
            for (var i = 0; i < question.Answers.Count; i++)
            {
                var answer = question.Answers[i];
                var button = answers[i];
                button.Setup(answer, () => OnAnswerClicked(answer.AnswersID));
            }
        }

        protected abstract void OnAnswerClicked(int clickedId);

        private async void ConfirmChoice()
        {
            timer.StopTimer();
            confirmButton.onClick.RemoveListener(ConfirmChoice);
            foreach (var button in answers)
            {
                _ = ResolveButton(button);
            }

            await UniTask.Delay(400);
            PresentResult().Forget();
        }

        private async Task ResolveButton(AnswerButton button)
        {
            var isCorrect = await button.ResolveQuestion();

            if (!isCorrect)
            {
                _correct -= 1f/_correctCount;
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