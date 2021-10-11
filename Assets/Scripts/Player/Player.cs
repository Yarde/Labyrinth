using UI;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerRotation))]
[RequireComponent(typeof(AudioSource))]
public class Player : MonoBehaviour 
{
    [SerializeField] private UserInterface ui;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float baseLightStrength = 5f;
    [SerializeField] private float baseViewDistance = 0.1f;
    [SerializeField] private float baseMovementSpeed = 2f;

    public int Hearts { get; private set; }
    public int Points { get; private set; }
    public int Experience { get; private set; }
    public int Playtime { get; private set; }
    public int Coins { get; private set; }
    public float LightLevel { get; private set; }
    public float FieldOfViewLevel { get; private set; }
    public float MovementSpeed { get; private set; }

    private void Start()
    {
        LightLevel = baseLightStrength;
        FieldOfViewLevel = baseViewDistance;
        MovementSpeed = baseMovementSpeed;
    }

    //debug only
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LightLevel++;
        }
    }
}