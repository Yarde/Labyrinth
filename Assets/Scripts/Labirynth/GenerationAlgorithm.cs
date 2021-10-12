using UnityEngine;

namespace Labirynth
{
    public abstract class GenerationAlgorithm
    {
        protected readonly Cell[,] Cells;
        protected Vector2Int Dimensions;

        protected GenerationAlgorithm(Cell[,] cells, Vector2Int dimensions)
        {
            Cells = cells;
            Dimensions = dimensions;
        }

        public abstract void CreateMaze();
    }
}