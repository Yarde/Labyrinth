using Skills;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI
{
    public class SkillButton : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private Image icon;
        [SerializeField] private Image frame;
        [SerializeField] private Image frameFill;
        [SerializeField] private Image cloud;
        [SerializeField] private Image coinIcon;
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private TextMeshProUGUI costText;
        private const string SkillTextPattern = "Level {0}\nCost {1}";
        private static readonly Color lightGray = new Color(0.7f, 0.7f, 0.7f, 0.7f);

        private Player _player;
        private Skill _skill;

        private void Update()
        {
            if (_skill == null || !_player || _skill.Level == _skill.Data.maxLevel)
            {
                //not initialized
                return;
            }

            if (_skill.Cost > _player.Coins)
            {
                //not enough coins
                button.interactable = false;
                costText.color = lightGray;
                frame.color = lightGray.WithA(0.4f);
                frameFill.color = lightGray;
                icon.color = lightGray;
                coinIcon.color = lightGray;
            }
            else
            {
                button.interactable = true;
                costText.color = Color.white;
                frame.color = Color.white.WithA(0.4f);
                frameFill.color = Color.white;
                icon.color = Color.white;
                coinIcon.color = Color.white;
            }
        }

        public void SetupSkill(Skill skill, Player player)
        {
            _skill = skill;
            _player = player;
           
            icon.sprite = skill.Data.icon;
            frame.sprite = skill.Data.frame;
            frameFill.sprite = skill.Data.frame;
            cloud.color = skill.Data.color;
            SetTexts();
            
            button.interactable = false;
            button.onClick.AddListener(_skill.Upgrade);
            button.onClick.AddListener(UpdateSkill);
        }

        private void UpdateSkill()
        {
            SetTexts();
            frameFill.fillAmount = _skill.Level / (float) _skill.Data.maxLevel;

            if (_skill.Level == _skill.Data.maxLevel)
            {
                button.interactable = false;
                // todo block button and change sprite color to gold or something like that
                costText.color = Color.yellow;
            }
        }

        private void SetTexts()
        {
            levelText.text = string.Format(SkillTextPattern, _skill.Level, _skill.Cost);
            costText.text = _skill.Cost.ToString();
        }
    }
}