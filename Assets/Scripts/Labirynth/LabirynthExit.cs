using UnityEngine;

namespace Labirynth
{
    public class LabirynthExit : MonoBehaviour
    {
        [SerializeField] private GameObject[] lights;

        private bool _isLightSelected;
        public void DisableLights(Cell cell)
        {
            SelectOneSide(cell.West, 0);
            SelectOneSide(cell.East, 1);
            SelectOneSide(cell.South, 2);
            SelectOneSide(cell.North, 3);
        }

        private void SelectOneSide(Object side, int i)
        {
            if (side == null || _isLightSelected)
            {
                Destroy(lights[i]);
            }
            else if (side != null)
            {
                _isLightSelected = true;
            }
        }
    }
}