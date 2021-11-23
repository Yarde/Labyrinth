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
        [SerializeField] private Light enemyLight;

        private const float MoveTime = 1f;
        private const float MaxLightRange = 0.5f;
        private const float IncrementOfRange = 0.02f;

        private CancellationTokenSource _cancellationToken;
        private bool _isMoving;
        private Cell _previousCell;

        private void Start()
        {
            _cancellationToken = new CancellationTokenSource();
            FlashLight().WithCancellation(_cancellationToken.Token);
        }

        private void OnDestroy()
        {
            _cancellationToken.Cancel();
            _cancellationToken.Dispose();
            _cancellationToken = null;
        }

        private async UniTask FlashLight()
        {
            var sign = 1;
            while (!_cancellationToken.IsCancellationRequested)
            {
                enemyLight.range += IncrementOfRange * sign;
                await UniTask.Delay(100);

                if (enemyLight.range <= 0 || enemyLight.range >= MaxLightRange)
                {
                    sign *= -1;
                }
            }
        }

        private void FixedUpdate()
        {
            if (GameRoot.IsPaused)
            {
                return;
            }
            
            if (!_isMoving)
            {
                MoveToNextCell().WithCancellation(_cancellationToken.Token);;
            }
        }

        private async UniTask MoveToNextCell()
        {
            _isMoving = true;
            
            Cell nextCell;
            if (Cell.AdjacentCells.Count > 1 && _previousCell != null)
            {
                var directions = Cell.AdjacentCells.Where(x => x != _previousCell).ToList();
                nextCell = ProceduralNumberGenerator.GetRandomCell(directions);
            }
            else
            {
                nextCell = ProceduralNumberGenerator.GetRandomCell(Cell.AdjacentCells);
            }

            _previousCell = Cell;
            Cell = nextCell;

            if (Cell.Floor)
            {
                var nextCellPosition = nextCell.Floor.transform.position;
                await transform.DOMove(nextCellPosition, MoveTime);
            }

            _isMoving = false;
        }

        public override async UniTask Destroy()
        {
            Collected = true;
            Destroy(gameObject);
            await UniTask.CompletedTask;
        }
    }
}