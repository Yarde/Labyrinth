using System;
using UnityEngine;
using System.Collections;

public class LightController : MonoBehaviour {
    private Player player;
    private Light light;
    public float smoothSpeed = 10f;
    public Vector3 offset;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        light = GetComponent<Light>();
    }

    private void FixedUpdate() {
        if (!player) {
            return;
        }

        light.range = player.lightStrength;

        var destination = player.transform.position + offset;
        var smoothed = Vector3.Lerp(transform.position, destination, smoothSpeed * Time.deltaTime);
        transform.position = destination;
    }

}