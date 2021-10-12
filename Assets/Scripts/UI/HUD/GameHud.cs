﻿using Skills;
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
        [SerializeField] private SkillPanel skillPanel;

        private Player _player;
        
        public void Setup(Player player, Skill[] skills)
        {
            _player = player;
            skillPanel.Setup(player, skills);
            healthBar.SetupBar(player);
            experienceBar.SetupBar(player);
            timer.Setup();
        }

        private void Update()
        {
            coinCounter.text = _player.Coins.ToString();
        }
    }
}