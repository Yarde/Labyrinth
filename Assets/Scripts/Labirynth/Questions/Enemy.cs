using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Utils;

namespace Labirynth.Questions
{
    public class Enemy : QuestionTrigger
    {
        private const int RaycastDistance = 5;
        private const int LightRange = 5;
        
        [SerializeField] private Rigidbody rigidBody;
        [SerializeField] private Light light;

        private CancellationTokenSource _cancellationToken;
        private Vector3 _direction = Vector3.left;
        private readonly List<Vector3> _possibleDirections = new List<Vector3>
        {
            Vector3.left,
            Vector3.right,
            Vector3.forward,
            Vector3.back
        };

        private void Start()
        {
            _cancellationToken = new CancellationTokenSource();
            FlashLight().WithCancellation(_cancellationToken.Token);
        }

        private async UniTask FlashLight()
        {
            var sign = 1;
            while (!_cancellationToken.IsCancellationRequested)
            {
                light.range += 0.01f * sign;
                await UniTask.Delay(10);

                if (light.range <= 0)
                {
                    sign *= -1;
                    await UniTask.Delay(500);
                }
                if (light.range >= 1)
                {
                    sign *= -1;
                }
            }
        }

        private void FixedUpdate()
        {
            if (rigidBody.velocity.magnitude < 0.1f)
            {
                var directions = _possibleDirections.Where(x => x != _direction).ToList();
                _direction = ProceduralNumberGenerator.GetRandomDirection(directions);
            }
            
            var origin = transform.position;
            rigidBody.velocity = _direction;
            
            Debug.DrawLine(origin, origin + _direction * RaycastDistance, Color.red);
        }

        public override async UniTask Destroy()
        {
            _cancellationToken.Cancel();
            _cancellationToken.Dispose();
            _cancellationToken = null;
            Collected = true;
            Destroy(gameObject);
            await UniTask.CompletedTask;
        }
    }
}