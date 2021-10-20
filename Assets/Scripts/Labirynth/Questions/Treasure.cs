using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Labirynth.Questions
{
    public class Treasure : QuestionTrigger
    {
        [SerializeField] private Animator animator;
        [SerializeField] private GameObject idleBox;
        [SerializeField] private GameObject destroyBox;
        [SerializeField] private Collider collider;
        private static readonly int DestroyTrigger = Animator.StringToHash("Destroy");

        public override async UniTask Destroy()
        {
            Collected = true;
            idleBox.SetActive(false);
            destroyBox.SetActive(true);
            
            animator.SetTrigger(DestroyTrigger);

            collider.enabled = false;
            var animationLength = animator.GetCurrentAnimatorStateInfo(0).length;
            await UniTask.Delay((int)(animationLength * 1000));
            
            Destroy(gameObject);
        }
    }
}