using Player.Skills;
using UnityEngine;

namespace UI.HUD
{
    public class SkillPanel : MonoBehaviour
    {
        [SerializeField] private SkillButton buttonPrefab;
        
        private SkillButton[] _skillButtons;

        public void Setup(Player.Player player, Skill[] skills)
        {
            _skillButtons = new SkillButton[skills.Length];
            
            for (var index = 0; index < skills.Length; index++)
            {
                _skillButtons[index] = Instantiate(buttonPrefab, transform);
                
                var skill = skills[index];
                _skillButtons[index].SetupSkill(skill, player);
            }
        }
    }
}