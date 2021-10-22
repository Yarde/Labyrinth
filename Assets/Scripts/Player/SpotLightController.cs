using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

[RequireComponent(typeof(SpotLight))]
public class SpotLightController : MonoBehaviour {
    [SerializeField] private Player player;
    [SerializeField] private Light lightSource;
    [SerializeField] private float smoothSpeed = 10f;
    [SerializeField] private Vector3 offset;

    private void FixedUpdate() 
    {
        lightSource.range = player.LightLevel;

        var destination = player.transform.position + offset;
        var smoothed = Vector3.Lerp(transform.position, destination, smoothSpeed * Time.deltaTime);
        transform.position = smoothed;//.WithY(smoothed.y + player.LightLevel * 0.02f);
        lightSource.spotAngle = 40 + player.LightLevel * 4f;
    }

}