using System.Linq;
using Skills;
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
    
    [SerializeField] private SkillData[] skills;

    public int Hearts { get; private set; }
    public int Points { get; private set; }
    public int Experience { get; private set; }
    public int Playtime { get; private set; }
    public int Coins { get; set; }
    public float LightLevel { get; set; }
    public float FieldOfViewLevel { get; set; }
    public float MovementSpeed { get; set; }
    
    private Skill[] _skills;

    private void Start()
    {
        LightLevel = baseLightStrength;
        FieldOfViewLevel = baseViewDistance;
        MovementSpeed = baseMovementSpeed;
        Experience = 0;
        Hearts = 5;
        SetupSkills();
        ui.Setup(this, _skills);
    }
    
    private void SetupSkills()
    {
        _skills = new Skill[]
        {
            new UpgradeLight(this, skills.FirstOrDefault(x => x.skillName == "UpgradeLight")),
            new UpgradeVision(this, skills.FirstOrDefault(x => x.skillName == "UpgradeVision")),
            new UpgradeMovement(this, skills.FirstOrDefault(x => x.skillName == "UpgradeMovement"))
        };
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Coins += 11;
            Experience += 12;
        }
    }
}