using UnityEngine;

namespace Labirynth
{
    public class HuntAndKillAlgorithm : GenerationAlgorithm
    {
        private int _currentRow;
        private int _currentColumn;

        private bool _courseComplete = false;

        public HuntAndKillAlgorithm(Cell[,] mazeCells) : base(mazeCells)
        {
        }

        public override void CreateMaze()
        {
            HuntAndKill();
        }

        private void HuntAndKill()
        {
            _currentRow = _dimentions.x / 2;
            _currentColumn = _dimentions.y / 2;

            _cells[_currentRow, _currentColumn].Visited = true;

            while (!_courseComplete)
            {
                Kill(); // Will run until it hits a dead end.
                Hunt(); // Finds the next unvisited cell with an adjacent visited cell. If it can't find any, it sets courseComplete to true.
            }

            for (var i = _dimentions.x / 2 - _dimentions.x / 10; i < _dimentions.x / 2 + _dimentions.x / 10; i++)
            {
                for (var j = _dimentions.y / 2 - _dimentions.y / 10; j < _dimentions.y / 2 + _dimentions.y / 10; j++)
                {
                    DestroyWallIfItExists(_cells[i, j].North);
                    DestroyWallIfItExists(_cells[i, j].South);
                    DestroyWallIfItExists(_cells[i, j].East);
                    DestroyWallIfItExists(_cells[i, j].West);
                }
            }
        }

        private void Kill()
        {
            while (RouteStillAvailable(_currentRow, _currentColumn))
            {
                //var direction = Random.Range(1, 5);
                var direction = ProceduralNumberGenerator.GetNextNumber();

                switch (direction)
                {
                    case 1 when CellIsAvailable(_currentRow - 1, _currentColumn):
                        // North
                        DestroyWallIfItExists(_cells[_currentRow, _currentColumn].North);
                        DestroyWallIfItExists(_cells[_currentRow - 1, _currentColumn].South);
                        _currentRow--;
                        break;
                    case 2 when CellIsAvailable(_currentRow + 1, _currentColumn):
                        // South
                        DestroyWallIfItExists(_cells[_currentRow, _currentColumn].South);
                        DestroyWallIfItExists(_cells[_currentRow + 1, _currentColumn].North);
                        _currentRow++;
                        break;
                    case 3 when CellIsAvailable(_currentRow, _currentColumn + 1):
                        // East
                        DestroyWallIfItExists(_cells[_currentRow, _currentColumn].East);
                        DestroyWallIfItExists(_cells[_currentRow, _currentColumn + 1].West);
                        _currentColumn++;
                        break;
                    case 4 when CellIsAvailable(_currentRow, _currentColumn - 1):
                        // West
                        DestroyWallIfItExists(_cells[_currentRow, _currentColumn].West);
                        DestroyWallIfItExists(_cells[_currentRow, _currentColumn - 1].East);
                        _currentColumn--;
                        break;
                }

                _cells[_currentRow, _currentColumn].Visited = true;
            }
        }

        private void Hunt()
        {
            _courseComplete = true;

            for (var r = 0; r < _dimentions.x; r++)
            {
                for (var c = 0; c < _dimentions.y; c++)
                {
                    if (_cells[r, c] == null) continue;
                
                    if (_cells[r, c].Visited || !CellHasAnAdjacentVisitedCell(r, c)) continue;

                    _courseComplete = false;
                    _currentRow = r;
                    _currentColumn = c;
                    DestroyAdjacentWall(_currentRow, _currentColumn);
                    _cells[_currentRow, _currentColumn].Visited = true;
                    return;
                }
            }
        }


        private bool RouteStillAvailable(int row, int column)
        {
            var availableRoutes = 0;

            if (row > 0 && _cells[row - 1, column] != null && !_cells[row - 1, column].Visited)
            {
                availableRoutes++;
            }

            if (row < _dimentions.x - 1 && _cells[row + 1, column] != null && !_cells[row + 1, column].Visited)
            {
                availableRoutes++;
            }

            if (column > 0 && _cells[row, column - 1] != null && !_cells[row, column - 1].Visited)
            {
                availableRoutes++;
            }

            if (column < _dimentions.y - 1 && _cells[row, column + 1] != null && !_cells[row, column + 1].Visited)
            {
                availableRoutes++;
            }

            return availableRoutes > 0;
        }

        private bool CellIsAvailable(int row, int column)
        {
            return row >= 0 && row < _dimentions.x && column >= 0 && column < _dimentions.y && _cells[row, column] != null && !_cells[row, column].Visited;
        }

        private void DestroyWallIfItExists(Object wall)
        {
            if (wall != null)
            {
                Object.Destroy(wall);
            }
        }

        private bool CellHasAnAdjacentVisitedCell(int row, int column)
        {
            var visitedCells = 0;

            // Look 1 row up (north) if we're on row 1 or greater
            if (row > 0 && _cells[row - 1, column] != null && _cells[row - 1, column].Visited)
            {
                visitedCells++;
            }

            // Look one row down (south) if we're the second-to-last row (or less)
            if (row < (_dimentions.x - 2) && _cells[row + 1, column] != null && _cells[row + 1, column].Visited)
            {
                visitedCells++;
            }

            // Look one row left (west) if we're column 1 or greater
            if (column > 0 && _cells[row, column - 1] != null && _cells[row, column - 1].Visited)
            {
                visitedCells++;
            }

            // Look one row right (east) if we're the second-to-last column (or less)
            if (column < (_dimentions.y - 2) && _cells[row, column + 1] != null && _cells[row, column + 1].Visited)
            {
                visitedCells++;
            }

            // return true if there are any adjacent visited _cells to this one
            return visitedCells > 0;
        }

        private void DestroyAdjacentWall(int row, int column)
        {
            var wallDestroyed = false;

            while (!wallDestroyed)
            {
                // int direction = Random.Range (1, 5);
                var direction = ProceduralNumberGenerator.GetNextNumber();

                switch (direction)
                {
                    case 1 when row > 0 && _cells[row - 1, column] != null && _cells[row - 1, column].Visited:
                        DestroyWallIfItExists(_cells[row, column].North);
                        DestroyWallIfItExists(_cells[row - 1, column].South);
                        wallDestroyed = true;
                        break;
                    case 2 when row < (_dimentions.x - 2) && _cells[row + 1, column] != null && _cells[row + 1, column].Visited:
                        DestroyWallIfItExists(_cells[row, column].South);
                        DestroyWallIfItExists(_cells[row + 1, column].North);
                        wallDestroyed = true;
                        break;
                    case 3 when column > 0 && _cells[row, column - 1] != null && _cells[row, column - 1].Visited:
                        DestroyWallIfItExists(_cells[row, column].West);
                        DestroyWallIfItExists(_cells[row, column - 1].East);
                        wallDestroyed = true;
                        break;
                    case 4 when column < (_dimentions.y - 2) && _cells[row, column + 1] != null && _cells[row, column + 1].Visited:
                        DestroyWallIfItExists(_cells[row, column].East);
                        DestroyWallIfItExists(_cells[row, column + 1].West);
                        wallDestroyed = true;
                        break;
                }
            }
        }
    }
}