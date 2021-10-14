using UnityEngine;
using Utils;

[RequireComponent(typeof(Light))]
public class LightController : MonoBehaviour {
    [SerializeField] private Player player;
    [SerializeField] private Light lightSource;
    [SerializeField] private float smoothSpeed = 10f;
    [SerializeField] private Vector3 offset;

    private void FixedUpdate() 
    {
        lightSource.range = player.LightLevel;

        var destination = player.transform.position + offset;
        var smoothed = Vector3.Lerp(transform.position, destination, smoothSpeed * Time.deltaTime);
        transform.position = smoothed.WithY(smoothed.y + player.LightLevel * 0.01f);
    }

}