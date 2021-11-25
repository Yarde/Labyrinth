using System.Linq;
using Gameplay;
using UnityEngine;

namespace UI.Windows.Questions
{
    public class QuestionWindowManager : MonoBehaviour
    {
        [Header("Question Types")] 
        [SerializeField] private QuestionScreenBase singleChoiceQuestionPrefab;
        [SerializeField] private QuestionScreenBase multiChoiceQuestionPrefab;

        private QuestionScreenBase _singleChoiceFourAnswers;
        private QuestionScreenBase SingleChoiceFourAnswers
        {
            get
            {
                if (!_singleChoiceFourAnswers)
                {
                    _singleChoiceFourAnswers = Instantiate(singleChoiceQuestionPrefab, transform);
                    _singleChoiceFourAnswers.gameObject.SetActive(false);
                }
            
                return _singleChoiceFourAnswers;
            }
        }
        
        private QuestionScreenBase _multiChoiceQuestion;
        private QuestionScreenBase MultiChoiceQuestion
        {
            get
            {
                if (!_multiChoiceQuestion)
                {
                    _multiChoiceQuestion = Instantiate(multiChoiceQuestionPrefab, transform);
                    _multiChoiceQuestion.gameObject.SetActive(false);
                }
            
                return _multiChoiceQuestion;
            }
        }

        public QuestionScreenBase GetWindow(QuestionResponse response)
        {
            var answers = response.Answers.Count;
            var correctAnswers = response.Answers.Sum(x => x.Correct ? 1 : 0);

            if (correctAnswers == 1)
            {
                //single choice
                return SingleChoiceFourAnswers;
            }
            if (correctAnswers > 1)
            {
                //single choice
                return MultiChoiceQuestion;
            }

            Logger.LogError($"Window not found for {answers} answers with {correctAnswers} correct answers");
            return SingleChoiceFourAnswers;
        }
    }
}