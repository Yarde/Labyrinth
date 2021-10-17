using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Labirynth.Questions
{
    public abstract class QuestionTrigger : MonoBehaviour
    {
        public Sprite Icon;
        public abstract UniTask Destroy();
    }
}