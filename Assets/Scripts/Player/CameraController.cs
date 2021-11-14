using UnityEngine;
using Utils;

namespace Player
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Player player;
        [SerializeField] private float smoothSpeed = 10f;
        [SerializeField] private Vector3 offset;

        private void FixedUpdate()
        {
            if (GameRoot.IsPaused)
            {
                return;
            }
        
            var destination = player.transform.position + offset;
            var smoothed = Vector3.Lerp(transform.position, destination, smoothSpeed * Time.deltaTime);
            transform.position = smoothed.WithY(smoothed.y + player.FieldOfViewLevel);
        }
    }
}