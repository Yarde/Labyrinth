using Cysharp.Threading.Tasks;

namespace Labirynth.Questions
{
    public class Enemy : QuestionTrigger
    {

        public override async UniTask Destroy()
        {
            Collected = true;
            Destroy(gameObject);
            await UniTask.CompletedTask;
        }
    }
}