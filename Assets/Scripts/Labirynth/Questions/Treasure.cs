using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Labirynth.Questions
{
    public class Treasure : QuestionTrigger
    {
        [SerializeField] private Animator animator;
        [SerializeField] private GameObject idleBox;
        [SerializeField] private GameObject destroyBox;
        private static readonly int DestroyTrigger = Animator.StringToHash("Destroy");

        public override async UniTask Destroy()
        {
            idleBox.SetActive(false);
            destroyBox.SetActive(true);
            
            animator.SetTrigger(DestroyTrigger);
            
            var animationLength = animator.GetCurrentAnimatorStateInfo(0).length;
            await UniTask.Delay((int)(animationLength * 1000));
            
            Destroy(gameObject);
        }
    }
}