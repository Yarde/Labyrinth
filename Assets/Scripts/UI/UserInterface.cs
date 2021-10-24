using Cysharp.Threading.Tasks;
using Menu;
using Skills;
using UI.Windows;
using UnityEngine;

namespace UI
{
    public class UserInterface : MonoBehaviour
    {
        [Header("Windows Prefabs")]
        [SerializeField] private GameHud gameHudPrefab;
        [SerializeField] private MenuWindow menuPrefab;
        [SerializeField] private QuestionScreenBase singleChoiceQuestionPrefab;
        [SerializeField] private WindowState deadScreenPrefab;
        [SerializeField] private WindowState pauseScreenPrefab;
        [SerializeField] private TipDisplay tipDisplayPrefab;

        private MenuWindow _menu;
        private GameHud _hud;
        private WindowState _pauseScreen;
        private QuestionScreenBase _singleChoiceQuestion;
        private TipDisplay _tipDisplay;

        private bool DisableInput { get; set; }

        private Player _player;
        
        private void Update()
        {
            if (DisableInput)
            {
                return;
            }
            
            Cheats();
            HandlePauseScreen();
        }

        private void Cheats()
        {
            if (Input.GetKey(KeyCode.Space))
            {
                _player.Coins += 100;
            }
        }

        private void HandlePauseScreen()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && _hud)
            {
                if (Time.timeScale == 0 && _pauseScreen.IsOnTop)
                {
                    _hud.Resume();
                    _pauseScreen.OnExit();
                }
                else
                {
                    _hud.Pause();
                    _pauseScreen.OnEnter();
                }
            }
        }

        public void Setup(Player player, Skill[] skills)
        {
            _player = player;
            
            _hud = Instantiate(gameHudPrefab, transform);
            _hud.Setup(player, skills);

            if (!_pauseScreen)
            {
                _pauseScreen = Instantiate(pauseScreenPrefab, transform);
                _pauseScreen.Setup();
                _pauseScreen.gameObject.SetActive(false);
            }
            if (!_singleChoiceQuestion)
            {
                _singleChoiceQuestion = Instantiate(singleChoiceQuestionPrefab, transform);
                _singleChoiceQuestion.gameObject.SetActive(false);
            }
        }

        public async UniTask OpenQuestion()
        {
            _hud.Pause();
            DisableInput = true;
            
            _singleChoiceQuestion.gameObject.SetActive(true);
            await _singleChoiceQuestion.DisplayQuestion();
            _singleChoiceQuestion.gameObject.SetActive(false);
            
            DisableInput = false;
            _hud.Resume();
        }

        public async UniTask DisplayTip(string text)
        {
            if (!_tipDisplay)
            {
                _tipDisplay = Instantiate(tipDisplayPrefab, transform);
                _tipDisplay.gameObject.SetActive(false);
            }

            await _tipDisplay.DisplayTip(text);
        }

            
            // todo change `object` to real data
        public async UniTask<object> ShowMenu()
        {
            if (_hud)
            {
                Destroy(_hud.gameObject);
            }
            
            _menu = Instantiate(menuPrefab, transform);
            var data = await _menu.ShowMenu();
            Destroy(_menu.gameObject);
            return data;
        }

        public void PauseGame()
        {
            DisableInput = true;
            _hud.Pause();
            Time.timeScale = 0;
        }
    }
}
