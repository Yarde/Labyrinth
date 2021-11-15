using System.Linq;

namespace UI.Windows.Questions
{
    public class SingleChoiceQuestion : ClosedQuestion
    {
        protected override void OnAnswerClicked(int clickedId)
        {
            var clicked = _answers.Where(x => x.IsSelected).ToList();
            confirmButton.interactable = clicked.Count > 0;
            
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
    }
}