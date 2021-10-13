using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UI.Windows
{
    public abstract class QuestionScreenBase : MonoBehaviour
    {
        public abstract UniTask DisplayQuestion();
    }
}