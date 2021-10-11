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
            new UpgradeLight(this, new Skill.SkillData
            {
                SkillName = "Upgrade Light",
                BonusPerLevel = 1f,
                Cost = 10,
                Level = 0,
                MaxLevel = 5
            }),
            new UpgradeVision(this, new Skill.SkillData
            {
                SkillName = "Upgrade Vision",
                BonusPerLevel = 0.02f,
                Cost = 10,
                Level = 0,
                MaxLevel = 5
            }),
            new UpgradeMovement(this, new Skill.SkillData
            {
                SkillName = "Upgrade Movement",
                BonusPerLevel = 0.3f,
                Cost = 10,
                Level = 0,
                MaxLevel = 5
            })
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