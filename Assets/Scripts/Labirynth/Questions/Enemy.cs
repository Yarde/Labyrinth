using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Utils;

namespace Labirynth.Questions
{
    public class Enemy : QuestionTrigger
    {
        private const int RaycastDistance = 10;
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

        private bool _isMoving = false;
        private Cell _previousCell;

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
                light.range += 0.1f * sign;
                await UniTask.Delay(100);

                if (light.range <= 0 || light.range >= 1)
                {
                    sign *= -1;
                }
            }
        }

        private void FixedUpdate()
        {
            if (!_isMoving)
            {
                MoveToNextCell().WithCancellation(_cancellationToken.Token);;
            }
            
            /*if (rigidBody.velocity.magnitude < 0.1f)
            {
                var directions = _possibleDirections.Where(x => x != _direction).ToList();
                _direction = ProceduralNumberGenerator.GetRandomDirection(directions);
            }
            
            var origin = transform.position;
            rigidBody.velocity = _direction;
            
            Debug.DrawLine(origin, origin + _direction * RaycastDistance, Color.green);*/
        }

        private async UniTask MoveToNextCell()
        {
            _isMoving = true;
            Cell nextCell;

            if (Cell.AdjacentCells.Count > 1 && _previousCell != null)
            {
                var directions = Cell.AdjacentCells.Where(x => x != _previousCell).ToList();
                nextCell = ProceduralNumberGenerator.GetRandomCell(directions);
                //Debug.LogError($"Dead End or Bug for {name}!");
            }
            else
            {
                nextCell = ProceduralNumberGenerator.GetRandomCell(Cell.AdjacentCells);
            }

            if (nextCell == Cell)
            {
                Debug.LogError($"New cell the same as current cell for {name}!");
            }
            
            _previousCell = Cell;
            Cell = nextCell;
            var nextCellPosition = nextCell.Floor.transform.position;
            transform.DOMove(nextCellPosition, 1f);
            await UniTask.Delay(1000);
            
            _isMoving = false;
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