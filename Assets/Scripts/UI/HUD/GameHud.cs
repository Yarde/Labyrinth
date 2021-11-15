using DG.Tweening;
using Gameplay;
using TMPro;
using UI.Elements;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.HUD
{
    public class GameHud : MonoBehaviour
    {
        [SerializeField] private HealthBar healthBar;
        [SerializeField] private ExperienceBar experienceBar;
        [SerializeField] private TextMeshProUGUI coinCounter;
        [SerializeField] private Timer timer;
        [SerializeField] private SkillPanel skillPanel;
        [SerializeField] private ProgressPanel progressPanel;
        
        [SerializeField] private Button menuButton;
        [SerializeField] private Button tutorialButton;
        
        [SerializeField] private Image blendCloud;

        private Player _player;
        
        public void Setup(Player player)
        {
            _player = player;
            skillPanel.Setup(player);
            healthBar.SetupBar(player);
            experienceBar.SetupBar(player);
            progressPanel.Setup(player);

            blendCloud.transform.DORotate(new Vector3(0f, 0f, 360f), 80f, RotateMode.FastBeyond360)
                .SetLoops(-1).SetEase(Ease.Linear);

            Resume();
        }
        
        public void SetListeners(UnityAction onMenuClick, UnityAction onTutorialClick)
        {
            menuButton.onClick.AddListener(onMenuClick);
            tutorialButton.onClick.AddListener(onTutorialClick);
        }
        
        public void Resume()
        {
            timer.ResumeTimer();
        }

        public void Pause()
        {
            timer.StopTimer();
        }

        private void Update()
        {
            coinCounter.text = _player.Coins.ToString();

            var cloudScale = blendCloud.transform.localScale;
            var newScale = Vector3.one * (1f + _player.FieldOfViewLevel);
            if (cloudScale != newScale)
            {
                blendCloud.transform.DOScale(newScale, 0.5f);
            }
        }
        public int GetPlaytime() => timer.ElapsedSeconds;
    }
}