using Cysharp.Threading.Tasks;
using Gameplay;
using UnityEngine;

namespace UI.Windows
{
    public abstract class QuestionScreenBase : MonoBehaviour
    {
        public abstract UniTask DisplayQuestion(Question question);
        public abstract UniTask DisplayReward(QuestionResult result);
    }
}