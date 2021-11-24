using UnityEngine;

namespace Gameplay
{
    public class PlayerMovement : MonoBehaviour {
        [SerializeField] private Player player;
        [SerializeField] private Vector3 movement;
        [SerializeField] private Rigidbody rigidBody;
        
        private void FixedUpdate()
        {
            if (GameRoot.IsPaused)
            {
                return;
            }
            
            movement.x = Input.GetAxis("Horizontal");
            movement.z = Input.GetAxis("Vertical");

            if (player.Joystick && movement.magnitude <= 0)
            {
                movement.x = player.Joystick.Horizontal;
                movement.z = player.Joystick.Vertical;
            }
            
            rigidBody.velocity = movement.normalized * player.MovementSpeed;
        }
    }
}