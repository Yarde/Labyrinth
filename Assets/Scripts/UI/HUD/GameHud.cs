using System;
using TMPro;
using UnityEngine;

namespace UI
{
    public class GameHud : MonoBehaviour
    {
        [SerializeField] private HealthBar healthBar;
        [SerializeField] private ExperienceBar experienceBar;
        [SerializeField] private TextMeshProUGUI coinCounter;
        [SerializeField] private Timer timer;
        [SerializeField] private SkillButton[] skillButtons;

        private Player _player;
        
        public void Setup(Player player, Skill[] skills)
        {
            _player = player;
            
            for (var index = 0; index < skills.Length; index++)
            {
                var skill = skills[index];
                skillButtons[index].SetupSkill(skill, null, player);
            }
        }

        private void Update()
        {
            coinCounter.text = _player.Coins.ToString();
        }
    }
}