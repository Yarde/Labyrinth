using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Labirynth.Questions
{
    public abstract class QuestionTrigger : MonoBehaviour
    {
        public abstract UniTask Destroy();
    }
}