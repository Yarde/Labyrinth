using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Labirynth.Questions
{
    public abstract class QuestionTrigger : MonoBehaviour
    {
        public Cell Cell { get; set; }
        public Sprite Icon;
        public Color Color;
        public bool Collected { get; protected set; }
        public abstract UniTask Destroy();
    }
}