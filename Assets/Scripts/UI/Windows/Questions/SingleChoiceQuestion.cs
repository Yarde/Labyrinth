﻿using System.Linq;
using Cysharp.Threading.Tasks;

namespace UI.Windows
{
    public class SingleChoiceQuestion : ClosedQuestion
    {
        protected override void OnAnswerClicked(uint clickedId)
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