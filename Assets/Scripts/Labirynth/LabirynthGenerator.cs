using Labirynth.Generators;
using UnityEngine;

namespace Labirynth
{
    public class LabirynthGenerator : MonoBehaviour
    {
        [SerializeField] private GameObject wallPrefab;
        [SerializeField] private GameObject floorPrefab;
        [SerializeField] private Vector2Int dimensions = new Vector2Int(10, 10);
        [SerializeField] private float size = 1;

        private Cell[,] _cells;

        private void Start()
        {
            InitializeLabirynth();

            var generationAlgorithm = new HuntAndKillAlgorithm(_cells, dimensions);
            generationAlgorithm.CreateLabirynth();
            
            // todo add question triggers
        }

        private void InitializeLabirynth()
        {
            _cells = new Cell[dimensions.x, dimensions.y];
            var radius = new Vector2Int(dimensions.x / 2, dimensions.y / 2);

            for (var row = 0; row < dimensions.x; row++)
            {
                for (var col = 0; col < dimensions.y; col++)
                {
                    _cells[row, col] = AddCell(row, radius, col);
                }
            }
        }

        private Cell AddCell(int row, Vector2Int radius, int col)
        {
            var distanceToCenter = Mathf.Pow(row - dimensions.x / 2, 2) / Mathf.Pow(radius.x, 2) +
                                   Mathf.Pow(col - dimensions.y / 2, 2) / Mathf.Pow(radius.y, 2);
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
            cell.Floor = Instantiate(floorPrefab, position, Quaternion.identity, transform);
            cell.Floor.name = $"Floor {row},{col}";
        }

        private void SetSouthWall(int row, int col, Cell cell)
        {
            var position = new Vector3(row * size + size / 2f, 0, col * size);
            cell.South = Instantiate(wallPrefab, position, Quaternion.identity, transform);
            cell.South.name = $"South Wall {row},{col}";
            cell.South.transform.Rotate(Vector3.up * 90f);
        }

        private void SetEastWall(int row, int col, Cell cell)
        {
            var position = new Vector3(row * size, 0, col * size + size / 2f);
            cell.East = Instantiate(wallPrefab, position, Quaternion.identity, transform);
            cell.East.name = $"East Wall {row},{col}";
        }

        private void SetNorthWall(int row, int col, Cell cell)
        {
            var position = new Vector3(row * size - size / 2f, 0, col * size);
            cell.North = Instantiate(wallPrefab, position, Quaternion.identity, transform);
            cell.North.name = $"North Wall {row},{col}";
            cell.North.transform.Rotate(Vector3.up * 90f);
        }

        private void SetWestWall(int row, int col, Cell cell)
        {
            var position = new Vector3(row * size, 0, col * size - size / 2f);
            cell.West = Instantiate(wallPrefab, position, Quaternion.identity, transform);
            cell.West.name = $"West Wall {row},{col}";
        }
    }
}