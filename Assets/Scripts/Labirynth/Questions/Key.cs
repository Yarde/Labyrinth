using Cysharp.Threading.Tasks;

namespace Labirynth.Questions
{
    public class Key : QuestionTrigger
    {

        public override async UniTask Destroy()
        {
            Collected = true;
            Destroy(gameObject);
        }
    }
}