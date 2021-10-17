using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Labirynth.Questions
{
    public abstract class QuestionTrigger : MonoBehaviour
    {
        public Sprite Icon;
        public bool Collected { get; protected set; }
        public abstract UniTask Destroy();
    }
}