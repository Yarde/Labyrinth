using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows
{
    public class SingleChoiceQuestionScreen : QuestionScreenBase
    {
        [SerializeField] private TextMeshProUGUI timer;
        [SerializeField] private Button confirmButton;

        private float startTime;
        private bool finished;
        
        public override async UniTask DisplayQuestion()
        {
            startTime = Time.realtimeSinceStartup;
            finished = false;
            confirmButton.onClick.AddListener(ConfirmChoice);

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