using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Player player;
    public float smoothSpeed = 10f;
    public Vector3 offset;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    private void FixedUpdate()
    {
        if (!player)
        {
            return;
        }

        var destination = player.transform.position + offset;
        var smoothed = Vector3.Lerp(transform.position, destination, smoothSpeed * Time.deltaTime);
        transform.position = smoothed;
    }
}