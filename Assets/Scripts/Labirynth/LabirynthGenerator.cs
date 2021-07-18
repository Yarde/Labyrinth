using System;
using UnityEngine;

namespace Labirynth
{
    public class LabirynthGenerator : MonoBehaviour
    {
        [SerializeField] private GameObject wallPrefab;
        [SerializeField] private Vector2Int _dimentions = new Vector2Int(10, 10);
        [SerializeField] private float size = 1;

        private Cell[,] _cells;

        void Start()
        {
            InitializeLabirynth();

            GenerationAlgorithm ma = new HuntAndKillAlgorithm(_cells);
            ma.CreateMaze();
        }

        private void InitializeLabirynth()
        {
            _cells = new Cell[_dimentions.x, _dimentions.y];
            var radius = new Vector2Int(_dimentions.x / 2, _dimentions.y / 2);

            for (var row = 0; row < _dimentions.x; row++)
            {
                for (var col = 0; col < _dimentions.y; col++)
                {
                    if (Math.Pow(row - _dimentions.x / 2, 2) / Math.Pow(radius.x, 2) + Math.Pow(col - _dimentions.y / 2, 2) / Math.Pow(radius.y, 2) > 1)
                    {
                        continue;
                    }

                    var cell = new Cell();

                    if (Math.Abs(Math.Pow(row - _dimentions.x / 2, 2) / Math.Pow(radius.x, 2) + Math.Pow(col - _dimentions.y / 2, 2) / Math.Pow(radius.y, 2) - 1) < 1) //col == 0)
                    {
                        cell.West = Instantiate(wallPrefab, new Vector3(row * size, 0, (col * size) - (size / 2f)),
                            Quaternion.identity, transform);
                        cell.West.name = "West Wall " + row + "," + col;

                        cell.North = Instantiate(wallPrefab, new Vector3((row * size) - (size / 2f), 0, col * size),
                            Quaternion.identity, transform);
                        cell.North.name = "North Wall " + row + "," + col;
                        cell.North.transform.Rotate(Vector3.up * 90f);
                    }

                    cell.East = Instantiate(wallPrefab, new Vector3(row * size, 0, (col * size) + (size / 2f)),
                        Quaternion.identity, transform);
                    cell.East.name = "East Wall " + row + "," + col;

                    // if (row == 0)
                    // {
                    //     cell.North = Instantiate(wallPrefab, new Vector3((row * size) - (size / 2f), 0, col * size),
                    //         Quaternion.identity, transform);
                    //     cell.North.name = "North Wall " + row + "," + col;
                    //     cell.North.transform.Rotate(Vector3.up * 90f);
                    // }

                    cell.South = Instantiate(wallPrefab, new Vector3((row * size) + (size / 2f), 0, col * size),
                        Quaternion.identity, transform);
                    cell.South.name = "South Wall " + row + "," + col;
                    cell.South.transform.Rotate(Vector3.up * 90f);

                    _cells[row, col] = cell;
                }
            }
        }
    }
}