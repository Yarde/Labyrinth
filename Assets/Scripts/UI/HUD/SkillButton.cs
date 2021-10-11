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
        [SerializeField] private string skillTextPattern = "{0} Level";

        //todo pass player to button
        private Player _player;
        private Skill _skill;

        private void Update()
        {
            if (_skill == null || !_player || _skill.Data.Level == _skill.Data.MaxLevel)
            {
                //not initialized
                return;
            }

            if (_skill.Data.Cost > _player.Coins)
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
            skillLevelText.text = string.Format(skillTextPattern, skill.Data.Level);
            
            skillButton.interactable = false;
            skillButton.onClick.AddListener(_skill.Upgrade);
            skillButton.onClick.AddListener(UpdateSkill);
        }

        private void UpdateSkill()
        {
            skillLevelText.text = string.Format(skillTextPattern, _skill.Data.Level);

            if (_skill.Data.Level == _skill.Data.MaxLevel)
            {
                skillButton.interactable = false;
                // todo block button and change sprite color to gold or something like that
            }
        }
    }
}