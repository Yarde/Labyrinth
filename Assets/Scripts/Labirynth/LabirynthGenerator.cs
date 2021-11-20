using System;
using System.Collections.Generic;
using System.Linq;
using GameData;
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
        [SerializeField] private GameObject exitPrefab;
        [SerializeField] private float size = 1;

        private Cell[,] _cells;
        private Vector2Int _dimensions;
        private int _seed;
        private Dictionary<Type, ObjectiveData> _objectives;

        public void Setup(GeneratorData generatorData)
        {
            _dimensions = generatorData.Dimensions;
            _objectives = generatorData.Objectives;
            _seed = generatorData.Seed;
            
            ProceduralNumberGenerator.Initialize(_seed);
            
            InitializeLabirynth();

            var generationAlgorithm = new HuntAndKillAlgorithm(_cells, _dimensions);
            generationAlgorithm.CreateLabirynth();
            
            SpawnGameElements();
        }

        private void SpawnGameElements()
        {
            var cellList = LabirynthToList();
            var deadEnds = cellList.Where(x => x.DeadEnd).ToList();
            deadEnds.Shuffle();
            
            var extraPoints = cellList.Where(x => !x.DeadEnd && x.DistanceFromCenter > _dimensions.x/2).ToList();
            extraPoints.Shuffle();

            var possibleSpawnPoints = deadEnds.Concat(extraPoints).ToList();

            AddExitTrigger(possibleSpawnPoints[0]);
            AddQuestionTriggers(possibleSpawnPoints);
        }
        
        private void AddExitTrigger(Cell cell)
        {
            var exit = Instantiate(exitPrefab, triggersSpawnTransform);
            exit.transform.position = cell.Floor.transform.position;
            Destroy(cell.Floor.gameObject);
        }
        
        private void AddQuestionTriggers(List<Cell> possibleSpawnPoints)
        {
            // first cell is always taken by the exit so we start at index 1;
            var cellOffset = 1;
            cellOffset = AddTriggers(possibleSpawnPoints, typeof(Key), cellOffset);
            cellOffset = AddTriggers(possibleSpawnPoints, typeof(Treasure), cellOffset);
            AddTriggers(possibleSpawnPoints, typeof(Enemy), cellOffset);
        }

        private int AddTriggers(List<Cell> cellList, Type type, int startIndex)
        {
            var triggerData = _objectives[type];

            for (var i = startIndex; i < triggerData.Total + startIndex; i++)
            {
                var selectedCell = cellList[i];
                var trigger = Instantiate(triggerData.Prefab, triggersSpawnTransform);
                trigger.transform.position = selectedCell.Floor.transform.position.WithY(-0.4f);
                trigger.name = $"{triggerData.Prefab.name} - {i}";
                trigger.Cell = selectedCell;
            }

            return triggerData.Total + startIndex;
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
            var position = new Vector3(row * size, -0.6f, col * size);
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