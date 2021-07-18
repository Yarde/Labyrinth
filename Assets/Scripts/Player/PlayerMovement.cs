using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public float thresholdSpeed = 5f;
    
    private Player _player;
    private Vector3 _movement;
    private Animator _animator;
    private Rigidbody _rigidBody;
    private static readonly int Speed = Animator.StringToHash("Speed");

    private void Start()
    {
        //_animator = GetComponent<Animator>();
        _rigidBody = GetComponent<Rigidbody>();
        _player = GetComponent<Player>();
    }

    private void FixedUpdate()
    {
        _movement.x = Input.GetAxis("Horizontal");
        _movement.z = Input.GetAxis("Vertical");
        
        //_animator.SetFloat(Speed, _movement.sqrMagnitude);

        _rigidBody.velocity = _movement.normalized * thresholdSpeed;
    }
}