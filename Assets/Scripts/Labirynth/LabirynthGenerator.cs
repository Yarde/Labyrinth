using System.Collections.Generic;
using System.Linq;
using Labirynth.Generators;
using Labirynth.Questions;
using UnityEngine;
using Utils;

namespace Labirynth
{
    public class LabirynthGenerator : MonoBehaviour
    {
        [SerializeField] private Transform wallsSpawnTransform;
        [SerializeField] private Transform triggersSpawnTransform;
        
        [SerializeField] private GameObject wallPrefab;
        [SerializeField] private GameObject floorPrefab;
        [SerializeField] private QuestionTrigger questionTrigger;
        [SerializeField] private float size = 1;

        private Cell[,] _cells;
        private Vector2Int _dimensions = new Vector2Int(20, 20);
        private int _questions = 20;
        private int _seed = 42;

        private void Start()
        {
            // todo get labirynth data from backend, size, questions, goal and so on
            // _dimensions = Reply.dimensions;
            // _seed = Reply.seed;
            // _questions = Reply.questions;
            _seed = Random.Range(0, 1000000);
            
            ProceduralNumberGenerator.Initialize(_seed);
            
            InitializeLabirynth();

            var generationAlgorithm = new HuntAndKillAlgorithm(_cells, _dimensions);
            generationAlgorithm.CreateLabirynth();
            
            AddQuestionTriggers();
        }

        private void AddQuestionTriggers()
        {
            // todo add question triggers
            var cellList = LabirynthToList();
            
            // Debug.Log($"There is {cellList.FindAll(x => x.DeadEnd).Count} dead ends");
            // var s = "";
            // foreach (var cell in cellList)
            // {
            //     s = $"{s}{cell.Floor.name}, {cell.DistanceFromCenter}\n";
            // }
            // Debug.Log($"There are distances to all cells:\n{s}");
            
            var deadEnds = cellList.Where(x => x.DeadEnd).ToList();
            deadEnds.Shuffle();

            if (deadEnds.Count < _questions)
            {
                Debug.LogError($"More questions than dead ends {deadEnds.Count} < {_questions}");
            }

            for (var i = 0; i < _questions; i++)
            {
                AddTrigger(deadEnds, i);
            }
        }

        private void AddTrigger(List<Cell> deadEnds, int i)
        {
            deadEnds[i].Occupied = true;
            var trigger = Instantiate(questionTrigger, triggersSpawnTransform);
            trigger.transform.position = deadEnds[i].Floor.transform.position;
            trigger.name = $"{questionTrigger.name} - {i}";
        }

        private List<Cell> LabirynthToList()
        {
            var labirynthToList = new List<Cell>();
            for (var i = 0; i < _dimensions.x; i++)
            {
                for (var j = 0; j < _dimensions.y; j++)
                {
                    if (_cells[i, j] != null)
                    {
                        labirynthToList.Add(_cells[i, j]);
                    }
                }
            }

            return labirynthToList;
        }

        private void InitializeLabirynth()
        {
            _cells = new Cell[_dimensions.x, _dimensions.y];
            var radius = new Vector2Int(_dimensions.x / 2, _dimensions.y / 2);

            for (var row = 0; row < _dimensions.x; row++)
            {
                for (var col = 0; col < _dimensions.y; col++)
                {
                    _cells[row, col] = AddCell(row, radius, col);
                }
            }
        }

        private Cell AddCell(int row, Vector2Int radius, int col)
        {
            var distanceToCenter = Mathf.Pow(row - _dimensions.x / 2, 2) / Mathf.Pow(radius.x, 2) +
                                   Mathf.Pow(col - _dimensions.y / 2, 2) / Mathf.Pow(radius.y, 2);
            if (distanceToCenter >= 1)
            {
                //outside of circle
                return null;
            }

            var cell = new Cell();
            SetFloor(row, col, cell);
            if (distanceToCenter < 1)
            {
                SetWestWall(row, col, cell);
                SetNorthWall(row, col, cell);
            }
            SetEastWall(row, col, cell);
            SetSouthWall(row, col, cell);
            return cell;
        }

        private void SetFloor(int row, int col, Cell cell)
        {
            var position = new Vector3(row * size, -0.5f, col * size);
            cell.Floor = Instantiate(floorPrefab, position, Quaternion.identity, wallsSpawnTransform);
            cell.Floor.name = $"Floor {row},{col}";
        }

        private void SetSouthWall(int row, int col, Cell cell)
        {
            var position = new Vector3(row * size + size / 2f, 0, col * size);
            cell.South = Instantiate(wallPrefab, position, Quaternion.identity, wallsSpawnTransform);
            cell.South.name = $"South Wall {row},{col}";
            cell.South.transform.Rotate(Vector3.up * 90f);
        }

        private void SetEastWall(int row, int col, Cell cell)
        {
            var position = new Vector3(row * size, 0, col * size + size / 2f);
            cell.East = Instantiate(wallPrefab, position, Quaternion.identity, wallsSpawnTransform);
            cell.East.name = $"East Wall {row},{col}";
        }

        private void SetNorthWall(int row, int col, Cell cell)
        {
            var position = new Vector3(row * size - size / 2f, 0, col * size);
            cell.North = Instantiate(wallPrefab, position, Quaternion.identity, wallsSpawnTransform);
            cell.North.name = $"North Wall {row},{col}";
            cell.North.transform.Rotate(Vector3.up * 90f);
        }

        private void SetWestWall(int row, int col, Cell cell)
        {
            var position = new Vector3(row * size, 0, col * size - size / 2f);
            cell.West = Instantiate(wallPrefab, position, Quaternion.identity, wallsSpawnTransform);
            cell.West.name = $"West Wall {row},{col}";
        }
    }
}