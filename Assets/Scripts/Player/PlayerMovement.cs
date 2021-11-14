using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour {
        [SerializeField] private Player player;
        [SerializeField] private Vector3 movement;
        [SerializeField] private Animator animator;
        [SerializeField] private Rigidbody rigidBody;

        private void FixedUpdate()
        {
            if (GameRoot.IsPaused)
            {
                return;
            }
        
            movement.x = Input.GetAxis("Horizontal");
            movement.z = Input.GetAxis("Vertical");
        
            //_animator.SetFloat(Speed, _movement.sqrMagnitude);

            rigidBody.velocity = movement.normalized * player.MovementSpeed;
        }
    }
}