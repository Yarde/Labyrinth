﻿using Skills;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SkillButton : MonoBehaviour
    {
        [SerializeField] private Button skillButton;
        [SerializeField] private Image skillImage;
        [SerializeField] private TextMeshProUGUI skillLevelText;
        [SerializeField] private TextMeshProUGUI skillCostText;
        private const string SkillTextPattern = "Level {0}\nCost {1}";

        //todo pass player to button
        private Player _player;
        private Skill _skill;

        private void Update()
        {
            if (_skill == null || !_player || _skill.Level == _skill.Data.maxLevel)
            {
                //not initialized
                return;
            }

            if (_skill.Data.cost > _player.Coins)
            {
                //not enough coins
                skillButton.interactable = false;
            }
            else
            {
                skillButton.interactable = true;
            }
        }

        public void SetupSkill(Skill skill, Sprite skillSprite, Player player)
        {
            _skill = skill;
            _player = player;
           
            skillImage.sprite = skillSprite;
            skillLevelText.text = string.Format(SkillTextPattern, skill.Level, _skill.Data.cost);
            skillCostText.text = _skill.Data.displayName;
            
            skillButton.interactable = false;
            skillButton.onClick.AddListener(_skill.Upgrade);
            skillButton.onClick.AddListener(UpdateSkill);
        }

        private void UpdateSkill()
        {
            skillLevelText.text = string.Format(SkillTextPattern, _skill.Level, _skill.Data.cost);
            skillCostText.text = _skill.Data.displayName;

            if (_skill.Level == _skill.Data.maxLevel)
            {
                skillButton.interactable = false;
                // todo block button and change sprite color to gold or something like that
            }
        }
    }
}