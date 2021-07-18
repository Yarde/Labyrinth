using UnityEngine;

namespace Labirynth
{
    public abstract class GenerationAlgorithm
    {
        protected Cell[,] _cells;
        protected Vector2Int _dimentions = new Vector2Int(10, 10);

        protected GenerationAlgorithm(Cell[,] cells) : base() {
            _cells = cells;
            _dimentions.x = _cells.GetLength(0);
            _dimentions.y = _cells.GetLength(1);
        }

        public abstract void CreateMaze ();
    }
}