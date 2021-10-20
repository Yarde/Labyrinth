using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Utils;

namespace Labirynth.Questions
{
    public class Enemy : QuestionTrigger
    {
        private const int raycastDistance = 5;
        
        [SerializeField] private Rigidbody rigidBody;

        private Vector3 direction = Vector3.left;
        private List<Vector3> possibleDirections = new List<Vector3>
        {
            Vector3.left,
            Vector3.right,
            Vector3.forward,
            Vector3.back
        };

        private void FixedUpdate()
        {
            if (rigidBody.velocity.magnitude < 0.1f)
            {
                var directions = possibleDirections.Where(x => x != direction).ToList();
                direction = ProceduralNumberGenerator.GetRandomDirection(directions);
            }
            
            var origin = transform.position;
            rigidBody.velocity = direction;

            Debug.DrawLine(origin, origin + direction * raycastDistance, Color.red);
        }

        public override async UniTask Destroy()
        {
            Collected = true;
            Destroy(gameObject);
            await UniTask.CompletedTask;
        }
    }
}