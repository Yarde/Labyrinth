using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils;

namespace UI.Windows
{
    public class WinScreen : WindowState
    {
        [SerializeField] private Button menu;

        [SerializeField] private AnimatedText points;
        [SerializeField] private AnimatedText time;
        [SerializeField] private AnimatedText exp;
        [SerializeField] private AnimatedText objectives;
        [SerializeField] private AnimatedText skills;
        
        [SerializeField] private string pointsFormat = "Points: {0}";
        [SerializeField] private string timeFormat = "Time: {0} seconds";
        [SerializeField] private string expFormat = "Experience: {0}";
        [SerializeField] private string objectivesFormat = "Objectives: {0}";
        [SerializeField] private string skillsFormat = "Skills: {0}";
        
        [SerializeField] private float duration = 0.5f;
        [SerializeField] private float strength = 0f;

        public async UniTask Setup(Player.Player player)
        {
            gameObject.SetActive(true);

            int objectivesCollected = player.Objectives.Sum(x => x.Value.Collected);
            int objectivesTotal = player.Objectives.Sum(x => x.Value.Total);

            await points.AnimateNewValue(player.Points, pointsFormat, duration, strength);
            await time.AnimateNewValue(player.Playtime, timeFormat, duration, strength);
            await exp.AnimateNewValue(player.Experience, expFormat, duration, strength);
            await objectives.AnimateNewValue(objectivesCollected, $"{objectivesFormat}/{objectivesTotal}", duration, strength);
            await skills.AnimateNewValue(10, skillsFormat, duration, strength);
            
            menu.onClick.AddListener(MainMenu);
        }

        private void OnDestroy()
        {
            menu.onClick.RemoveListener(MainMenu);
        }

        private void MainMenu()
        {
            GameRoot.IsPaused = false;
            SceneManager.LoadScene("Scene");
        }
    }
}
