using Cysharp.Threading.Tasks;

namespace Labirynth.Questions
{
    public class Key : QuestionTrigger
    {

        public override async UniTask Destroy()
        {

            Destroy(gameObject);
        }
    }
}