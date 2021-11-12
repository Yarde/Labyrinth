using Cysharp.Threading.Tasks;
using Gameplay;
using UnityEngine;

namespace UI.Windows
{
    public abstract class QuestionScreenBase : MonoBehaviour
    {
        public abstract UniTask<bool> DisplayQuestion(QuestionResponse question);
    }
}