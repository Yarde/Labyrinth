using UI;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerRotation))]
[RequireComponent(typeof(AudioSource))]
public class Player : MonoBehaviour 
{
    [SerializeField] private UserInterface ui;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float baseLightStrength = 5;

    private float _lightStrength;
    
    public float LightLevel => _lightStrength;
    public float FieldOfViewLevel => _lightStrength/40;
    public float MovementSpeed => 2 + (_lightStrength - baseLightStrength)/5;

    private void Start()
    {
        _lightStrength = baseLightStrength;
    }

    //debug only
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _lightStrength++;
        }
    }
}