using UnityEngine;

namespace Labirynth
{
    public class Cell
    {
        public bool Visited = false;
        public bool Occupied = false;
        public bool DeadEnd = false;
        public int DistanceFromCenter = 0;
        public GameObject North, South, East, West, Floor;
    }
}