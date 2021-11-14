using UnityEngine;

namespace UI.Windows
{
    [CreateAssetMenu(fileName = "Data", menuName = "Tutorial", order = 1)]
    public class TutorialTipData : ScriptableObject
    {
        public string tipText;
        public Vector3 tipPosition;
        public Vector3 cutoutPosition;
        public Vector2 cutoutSize;
        public bool showArrow = true;
        public float arrowRotationZ;
    }
}