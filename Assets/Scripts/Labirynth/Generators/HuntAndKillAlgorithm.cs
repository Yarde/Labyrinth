using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Labirynth.Generators
{
    public class HuntAndKillAlgorithm : GenerationAlgorithm
    { 
        private int _currentRow;
        private int _currentColumn;

        private bool _courseComplete = false;

        public HuntAndKillAlgorithm(Cell[,] cells, Vector2Int dimensions) : base(cells, dimensions)
        {
        }

        public override void CreateLabirynth()
        {
            HuntAndKill();

            // Clears the start area
            var size = new Vector2Int(Dimensions.x / 20, Dimensions.y / 20);
            GenerateStartingArea(size);
        }

        private void HuntAndKill()
        {
            _currentRow = Dimensions.x / 2;
            _currentColumn = Dimensions.y / 2;

            Cells[_currentRow, _currentColumn].Visited = true;

            while (!_courseComplete)
            {
                Kill(); // Will run until it hits a dead end.
                Hunt(); // Finds the next unvisited cell with an adjacent visited cell. If it can't find any, it sets courseComplete to true.
            }
        }

        private void Kill()
        {
            while (RouteAvailable(_currentRow, _currentColumn))
            {
                KillNextCell();
            }
        }

        private void KillNextCell()
        {
            var direction = ProceduralNumberGenerator.GetNextNumber();
            switch (direction)
            {
                case 1 when CellAvailable(_currentRow - 1, _currentColumn):
                    GoNorth(_currentRow, _currentColumn);
                    _currentRow--;
                    break;
                case 2 when CellAvailable(_currentRow + 1, _currentColumn):
                    GoSouth(_currentRow, _currentColumn);
                    _currentRow++;
                    break;
                case 3 when CellAvailable(_currentRow, _currentColumn - 1):
                    GoEast(_currentRow, _currentColumn);
                    _currentColumn--;
                    break;
                case 4 when CellAvailable(_currentRow, _currentColumn + 1):
                    GoWest(_currentRow, _currentColumn);
                    _currentColumn++;
                    break;
            }

            Cells[_currentRow, _currentColumn].Visited = true;
        }

        private void Hunt()
        {
            _courseComplete = true;

            for (var row = 0; row < Dimensions.x; row++)
            {
                for (var column = 0; column < Dimensions.y; column++)
                {
                    if (HuntCell(row, column))
                        return;
                }
            }
        }

        private bool HuntCell(int row, int column)
        {
            if (Cells[row, column] == null) return false;
            if (Cells[row, column].Visited || !HasAdjacentVisitedCell(row, column)) return false;

            _courseComplete = false;
            _currentRow = row;
            _currentColumn = column;
            DestroyAdjacentWall(row, column);
            Cells[row, column].Visited = true;
            return true;
        }

        private void DestroyAdjacentWall(int row, int column)
        {
            var availableDirections = GetAvailableDirections(row, column);
            var direction = ProceduralNumberGenerator.GetNextNumber();
            while (!availableDirections.Contains(direction))
            {
                direction = ProceduralNumberGenerator.GetNextNumber();
            }

            switch (direction)
            {
                case 1:
                    GoNorth(row, column);
                    break;
                case 2:
                    GoSouth(row, column);
                    break;
                case 3:
                    GoEast(row, column);
                    break;
                case 4:
                    GoWest(row, column);
                    break;
            }
        }

        private void GenerateStartingArea(Vector2Int size)
        {
            for (var i = Dimensions.x / 2 - size.x; i <= Dimensions.x / 2 + size.x; i++)
            {
                for (var j = Dimensions.y / 2 - size.y; j <= Dimensions.y / 2 + size.y; j++)
                {
                    Cells[i, j].North.DestroyIfExist();
                    Cells[i, j].South.DestroyIfExist();
                    Cells[i, j].East.DestroyIfExist();
                    Cells[i, j].West.DestroyIfExist();
                }
            }
        }

        #region Helper Functions

        private bool RouteAvailable(int row, int column, int offset = 1)
        {
            return row > 0 && Cells[row - 1, column] != null && !Cells[row - 1, column].Visited ||
                   row < Dimensions.x - offset && Cells[row + 1, column] != null && !Cells[row + 1, column].Visited ||
                   column > 0 && Cells[row, column - 1] != null && !Cells[row, column - 1].Visited ||
                   column < Dimensions.y - offset && Cells[row, column + 1] != null && !Cells[row, column + 1].Visited;
        }

        private bool CellAvailable(int row, int column)
        {
            return row >= 0 && row < Dimensions.x && column >= 0 && column < Dimensions.y &&
                   Cells[row, column] != null && !Cells[row, column].Visited;
        }

        private bool HasAdjacentVisitedCell(int row, int column)
        {
            return CanGoNorth(row, column) ||
                   CanGoSouth(row, column) ||
                   CanGoEast(row, column) ||
                   CanGoWest(row, column);
        }

        private bool CanGoNorth(int row, int column)
        {
            return row > 0 && Cells[row - 1, column] != null && Cells[row - 1, column].Visited;
        }

        private bool CanGoSouth(int row, int column)
        {
            return row < (Dimensions.x - 2) && Cells[row + 1, column] != null && Cells[row + 1, column].Visited;
        }

        private bool CanGoEast(int row, int column)
        {
            return column > 0 && Cells[row, column - 1] != null && Cells[row, column - 1].Visited;
        }

        private bool CanGoWest(int row, int column)
        {
            return column < (Dimensions.y - 2) && Cells[row, column + 1] != null && Cells[row, column + 1].Visited;
        }

        private void GoNorth(int row, int column)
        {
            Cells[row, column].North.DestroyIfExist();
            Cells[row - 1, column].South.DestroyIfExist();
        }

        private void GoSouth(int row, int column)
        {
            Cells[row, column].South.DestroyIfExist();
            Cells[row + 1, column].North.DestroyIfExist();
        }

        private void GoEast(int row, int column)
        {
            Cells[row, column].West.DestroyIfExist();
            Cells[row, column - 1].East.DestroyIfExist();
        }

        private void GoWest(int row, int column)
        {
            Cells[row, column].East.DestroyIfExist();
            Cells[row, column + 1].West.DestroyIfExist();
        }

        private List<int> GetAvailableDirections(int row, int column)
        {
            var availableDirections = new List<int>();
            if (CanGoNorth(row, column))
            {
                availableDirections.Add(1);
            }

            if (CanGoSouth(row, column))
            {
                availableDirections.Add(2);
            }

            if (CanGoEast(row, column))
            {
                availableDirections.Add(3);
            }

            if (CanGoWest(row, column))
            {
                availableDirections.Add(4);
            }

            return availableDirections;
        }

        #endregion
    }
}