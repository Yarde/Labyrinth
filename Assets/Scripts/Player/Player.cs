using System.Linq;
using Cysharp.Threading.Tasks;
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
        // todo get dimension of maze somehow
        transform.position = new Vector3(10f, 0f, 10f);
        
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
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("QuestionTrigger"))
        {
            OpenQuestion(collision.collider);
        }
    }

    private async UniTask OpenQuestion(Component component)
    {
        Debug.Log($"Collision entered with {component.name}");
        await ui.OpenQuestion();
        Destroy(component.gameObject);

        Coins += 100;
        Experience += 200;
    }
}