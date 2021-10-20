using Cysharp.Threading.Tasks;
using UnityEngine;
using Utils;

namespace Labirynth.Questions
{
    public class Enemy : QuestionTrigger
    {
        private const int raycastDistance = 200;
        
        [SerializeField] private Rigidbody rigidBody;
        
        private void FixedUpdate()
        {
            rigidBody.
            
            transform.position = transform.position.WithX(transform.position.x + 0.01f);
            
            var origin = transform.position;
            var direction = transform.forward;
            Debug.DrawLine(origin, origin + (direction * raycastDistance), Color.red, 2, false);
        }

        public override async UniTask Destroy()
        {
            Collected = true;
            Destroy(gameObject);
            await UniTask.CompletedTask;
        }
    }
}