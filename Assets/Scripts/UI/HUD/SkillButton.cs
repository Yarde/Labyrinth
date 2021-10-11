using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SkillButton : MonoBehaviour
    {
        [SerializeField] private Button skillButton;
        [SerializeField] private Image skillImage;
        [SerializeField] private TextMeshPro skillLevelText;
        [SerializeField] private TextMeshPro skillCostText;
        [SerializeField] private string skillTextPattern = "{0} Level";

        //todo pass player to button
        private Player _player;
        private Skill _skill;
        private string _name;
        private int _level;
        private int _maxLevel;
        private int _cost;

        private void Update()
        {
            if (!_skill || _level == _maxLevel)
            {
                //not initialized
                return;
            }

            if (_cost > _player.Coins)
            {
                //not enough coins
                skillButton.interactable = false;
            }
            else
            {
                skillButton.interactable = true;
            }
        }

        public void SetupSkill(Skill connectedSkill, Sprite skillSprite, string skillName, int level, int maxLevel, int cost)
        {
            _name = skillName;
            _level = level;
            _skill = connectedSkill;
            _maxLevel = maxLevel;
            _cost = cost;
            
            skillImage.sprite = skillSprite;
            skillLevelText.text = string.Format(skillTextPattern, level);
            
            skillButton.interactable = false;
            skillButton.onClick.AddListener(_skill.Upgrade);
        }

        public void UpdateSkill(int level)
        {
            skillLevelText.text = string.Format(skillTextPattern, level);
            _level = level;

            if (level == _maxLevel)
            {
                // todo block button and change sprite color to gold or something like that
            }
        }
    }
}