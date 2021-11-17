using System.Linq;

namespace UI.Windows.Questions
{
    public class MultipleChoiceQuestion : ClosedQuestion
    {
        protected override void OnAnswerClicked(int clickedId)
        {
            var clicked = answers.Where(x => x.IsSelected).ToList();
            confirmButton.interactable = clicked.Count > 0;
        }
    }
}