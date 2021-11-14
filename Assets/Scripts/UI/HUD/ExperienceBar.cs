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
        
        private int _currentExperience;
        private int _experienceLeft;
        private int _experienceNeeded;

        private void Update()
        {
            if (_currentExperience < _player.Experience)
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
            _currentExperience = _player.Experience;
            CalculateLevel();
            experienceText.text = $"Level {_player.Level} - {_experienceLeft}/{_experienceNeeded} exp";
            slider.value = _experienceLeft / (float) _experienceNeeded;
        }

        private void CalculateLevel()
        {
            _player.Level = (int) (Mathf.Floor(EXP_MULTIPLIER * Mathf.Sqrt(_currentExperience)) + 1);
            var experienceToNextLevel = (int) Mathf.Pow(_player.Level / EXP_MULTIPLIER, 2) - _currentExperience;
            _experienceNeeded = (int) Mathf.Pow(_player.Level / EXP_MULTIPLIER, 2) -
                               (int) Mathf.Pow((_player.Level - 1) / EXP_MULTIPLIER, 2);
            _experienceLeft = _experienceNeeded - experienceToNextLevel;
        }
    }
}