using UnityEngine;
using Utils;

namespace Labirynth
{
    public class HuntAndKillAlgorithm : GenerationAlgorithm
    {
        private int _currentRow;
        private int _currentColumn;

        private bool _courseComplete = false;

        public HuntAndKillAlgorithm(Cell[,] cells, Vector2Int dimensions) : base(cells, dimensions)
        {
        }

        public override void CreateMaze()
        {
            HuntAndKill();
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

            // Clears the start area
            var size = new Vector2Int(Dimensions.x/10, Dimensions.y/10);
            GenerateStartingArea(size);
        }

        private void GenerateStartingArea(Vector2Int size)
        {
            for (var i = Dimensions.x / 2 - size.x; i < Dimensions.x / 2 + size.x; i++)
            {
                for (var j = Dimensions.y / 2 - size.y; j < Dimensions.y / 2 + size.y; j++)
                {
                    DestroyWallIfItExists(Cells[i, j].North);
                    DestroyWallIfItExists(Cells[i, j].South);
                    DestroyWallIfItExists(Cells[i, j].East);
                    DestroyWallIfItExists(Cells[i, j].West);
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
                        DestroyWallIfItExists(Cells[_currentRow, _currentColumn].North);
                        DestroyWallIfItExists(Cells[_currentRow - 1, _currentColumn].South);
                        _currentRow--;
                        break;
                    case 2 when CellIsAvailable(_currentRow + 1, _currentColumn):
                        // South
                        DestroyWallIfItExists(Cells[_currentRow, _currentColumn].South);
                        DestroyWallIfItExists(Cells[_currentRow + 1, _currentColumn].North);
                        _currentRow++;
                        break;
                    case 3 when CellIsAvailable(_currentRow, _currentColumn + 1):
                        // East
                        DestroyWallIfItExists(Cells[_currentRow, _currentColumn].East);
                        DestroyWallIfItExists(Cells[_currentRow, _currentColumn + 1].West);
                        _currentColumn++;
                        break;
                    case 4 when CellIsAvailable(_currentRow, _currentColumn - 1):
                        // West
                        DestroyWallIfItExists(Cells[_currentRow, _currentColumn].West);
                        DestroyWallIfItExists(Cells[_currentRow, _currentColumn - 1].East);
                        _currentColumn--;
                        break;
                }

                Cells[_currentRow, _currentColumn].Visited = true;
            }
        }

        private void Hunt()
        {
            _courseComplete = true;

            for (var r = 0; r < Dimensions.x; r++)
            {
                for (var c = 0; c < Dimensions.y; c++)
                {
                    if (Cells[r, c] == null) continue;

                    if (Cells[r, c].Visited || !CellHasAnAdjacentVisitedCell(r, c)) continue;

                    _courseComplete = false;
                    _currentRow = r;
                    _currentColumn = c;
                    DestroyAdjacentWall(_currentRow, _currentColumn);
                    Cells[_currentRow, _currentColumn].Visited = true;
                    return;
                }
            }
        }


        private bool RouteStillAvailable(int row, int column)
        {
            var availableRoutes = 0;

            if (row > 0 && Cells[row - 1, column] != null && !Cells[row - 1, column].Visited)
            {
                availableRoutes++;
            }

            if (row < Dimensions.x - 1 && Cells[row + 1, column] != null && !Cells[row + 1, column].Visited)
            {
                availableRoutes++;
            }

            if (column > 0 && Cells[row, column - 1] != null && !Cells[row, column - 1].Visited)
            {
                availableRoutes++;
            }

            if (column < Dimensions.y - 1 && Cells[row, column + 1] != null && !Cells[row, column + 1].Visited)
            {
                availableRoutes++;
            }

            return availableRoutes > 0;
        }

        private bool CellIsAvailable(int row, int column)
        {
            return row >= 0 && row < Dimensions.x && column >= 0 && column < Dimensions.y &&
                   Cells[row, column] != null && !Cells[row, column].Visited;
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
            if (row > 0 && Cells[row - 1, column] != null && Cells[row - 1, column].Visited)
            {
                visitedCells++;
            }

            // Look one row down (south) if we're the second-to-last row (or less)
            if (row < (Dimensions.x - 2) && Cells[row + 1, column] != null && Cells[row + 1, column].Visited)
            {
                visitedCells++;
            }

            // Look one row left (west) if we're column 1 or greater
            if (column > 0 && Cells[row, column - 1] != null && Cells[row, column - 1].Visited)
            {
                visitedCells++;
            }

            // Look one row right (east) if we're the second-to-last column (or less)
            if (column < (Dimensions.y - 2) && Cells[row, column + 1] != null && Cells[row, column + 1].Visited)
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
                    case 1 when row > 0 && Cells[row - 1, column] != null && Cells[row - 1, column].Visited:
                        DestroyWallIfItExists(Cells[row, column].North);
                        DestroyWallIfItExists(Cells[row - 1, column].South);
                        wallDestroyed = true;
                        break;
                    case 2 when row < (Dimensions.x - 2) && Cells[row + 1, column] != null &&
                                Cells[row + 1, column].Visited:
                        DestroyWallIfItExists(Cells[row, column].South);
                        DestroyWallIfItExists(Cells[row + 1, column].North);
                        wallDestroyed = true;
                        break;
                    case 3 when column > 0 && Cells[row, column - 1] != null && Cells[row, column - 1].Visited:
                        DestroyWallIfItExists(Cells[row, column].West);
                        DestroyWallIfItExists(Cells[row, column - 1].East);
                        wallDestroyed = true;
                        break;
                    case 4 when column < (Dimensions.y - 2) && Cells[row, column + 1] != null &&
                                Cells[row, column + 1].Visited:
                        DestroyWallIfItExists(Cells[row, column].East);
                        DestroyWallIfItExists(Cells[row, column + 1].West);
                        wallDestroyed = true;
                        break;
                }
            }
        }
    }
}