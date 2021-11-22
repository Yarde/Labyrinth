using Gameplay;
using UnityEngine;

namespace UI.HUD
{
    public class SkillPanel : MonoBehaviour
    {
        [SerializeField] private SkillButton buttonPrefab;
        
        private SkillButton[] _skillButtons;

        public void Setup(Player player)
        {
            _skillButtons = new SkillButton[player.Skills.Count];

            var index = 0;
            foreach(var skill in player.Skills)
            {
                _skillButtons[index] = Instantiate(buttonPrefab, transform);
                _skillButtons[index].SetupSkill(skill.Value, player);
                index++;
            }
        }
    }
}