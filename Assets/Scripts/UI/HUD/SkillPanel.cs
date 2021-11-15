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
            _skillButtons = new SkillButton[player.Skills.Length];
            
            for (var index = 0; index < player.Skills.Length; index++)
            {
                _skillButtons[index] = Instantiate(buttonPrefab, transform);
                
                var skill = player.Skills[index];
                _skillButtons[index].SetupSkill(skill, player);
            }
        }
    }
}