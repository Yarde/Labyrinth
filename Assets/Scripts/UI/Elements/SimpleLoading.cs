using UnityEngine;

namespace UI.Elements
{
    public class SimpleLoading : MonoBehaviour {

        private RectTransform rectComponent;
        [SerializeField] private float rotateSpeed = 200f;

        private void Start () {
            rectComponent = GetComponent<RectTransform>();
        }
    
        private void Update () {
            rectComponent.Rotate(0f, 0f, -(rotateSpeed * Time.deltaTime));
        }
    }
}
