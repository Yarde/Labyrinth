using TMPro;
using UnityEngine;

namespace UI
{
    public class GameHud : MonoBehaviour
    {
        [SerializeField] private HealthBar healthBar;
        [SerializeField] private ExperienceBar experienceBar;
        [SerializeField] private TextMeshPro coinCounter;
        [SerializeField] private Timer timer;
        [SerializeField] private SkillButton[] skillButtons;

    }
}