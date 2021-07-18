using UnityEngine;

namespace Labirynth
{
    public class Cell
    {
        public bool Visited = false;
        public GameObject North, South, East, West;
    }
}