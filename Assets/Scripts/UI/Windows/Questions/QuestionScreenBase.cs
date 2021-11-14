using Cysharp.Threading.Tasks;
using Gameplay;
using UnityEngine;

namespace UI.Windows.Questions
{
    public abstract class QuestionScreenBase : MonoBehaviour
    {
        public abstract UniTask<bool> DisplayQuestion(QuestionResponse question);
    }
}