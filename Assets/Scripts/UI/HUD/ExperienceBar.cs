using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.HUD
{
    public class ExperienceBar : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        [SerializeField] private TextMeshProUGUI experienceText;

        private const float EXP_MULTIPLIER = 0.1f;

        private Player.Player _player;
        
        private int currentExperience;
        private int level;
        private int experienceLeft;
        private int experienceNeeded;

        private void Update()
        {
            if (currentExperience < _player.Experience)
            {
                // todo animate experience gain
                UpdateExperience();
            }
        }
        
        public void SetupBar(Player.Player player)
        {
            _player = player;
            UpdateExperience();
        }
        
        private void UpdateExperience()
        {
            currentExperience = _player.Experience;
            CalculateLevel();
            experienceText.text = $"Level {level} - {experienceLeft}/{experienceNeeded} exp";
            slider.value = experienceLeft / (float) experienceNeeded;
        }

        private void CalculateLevel()
        {
            level = (int) (Mathf.Floor(EXP_MULTIPLIER * Mathf.Sqrt(currentExperience)) + 1);
            var experienceToNextLevel = (int) Mathf.Pow(level / EXP_MULTIPLIER, 2) - currentExperience;
            experienceNeeded = (int) Mathf.Pow(level / EXP_MULTIPLIER, 2) -
                               (int) Mathf.Pow((level - 1) / EXP_MULTIPLIER, 2);
            experienceLeft = experienceNeeded - experienceToNextLevel;
        }
    }
}