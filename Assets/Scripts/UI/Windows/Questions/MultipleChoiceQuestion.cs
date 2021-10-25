using System.Collections.Generic;
using System.Linq;
using Gameplay;

namespace UI.Windows
{
    public class MultipleChoiceQuestion : ClosedQuestion
    {
        protected override void OnAnswerClicked(uint clickedId)
        {
            var clicked = _answers.Where(x => x.IsSelected).ToList();
            confirmButton.interactable = clicked.Count > 0;
        }
    }
}