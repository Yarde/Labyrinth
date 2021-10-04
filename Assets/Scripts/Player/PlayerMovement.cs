using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    [SerializeField] private Player _player;
    [SerializeField] private Vector3 _movement;
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody _rigidBody;

    private void FixedUpdate()
    {
        _movement.x = Input.GetAxis("Horizontal");
        _movement.z = Input.GetAxis("Vertical");
        
        //_animator.SetFloat(Speed, _movement.sqrMagnitude);

        _rigidBody.velocity = _movement.normalized * _player.MovementSpeed;
    }
}