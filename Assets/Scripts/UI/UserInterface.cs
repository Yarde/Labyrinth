using Cysharp.Threading.Tasks;
using Gameplay;
using Menu;
using Skills;
using UI.Windows;
using UnityEngine;
using Utils;

namespace UI
{
    public class UserInterface : MonoBehaviour
    {
        [Header("Windows Prefabs")]
        [SerializeField] private GameHud gameHudPrefab;
        [SerializeField] private MenuWindow menuPrefab;
        [SerializeField] private WindowState deadScreenPrefab;
        [SerializeField] private WindowState pauseScreenPrefab;
        [SerializeField] private TipDisplay tipDisplayPrefab;
        
        [Header("Question Types")]
        [SerializeField] private QuestionScreenBase singleChoiceQuestionPrefab;
        [SerializeField] private QuestionScreenBase multiChoiceQuestionPrefab;

        private MenuWindow _menu;
        private GameHud _hud;
        private WindowState _pauseScreen;
        private TipDisplay _tipDisplay;
        
        private QuestionScreenBase _singleChoiceQuestion;
        private QuestionScreenBase _multiChoiceQuestion;

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
            if (Input.GetKeyDown(KeyCode.Y))
            {
                _player.Coins += 100;
            }
            if (Input.GetKeyDown(KeyCode.T))
            {
                _player.transform.position = _player.transform.position.WithZ(_player.transform.position.z + 1f);
            }
            if (Input.GetKeyDown(KeyCode.G))
            {
                _player.transform.position = _player.transform.position.WithZ(_player.transform.position.z - 1f);
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                _player.transform.position = _player.transform.position.WithX(_player.transform.position.x - 1f);
            }
            if (Input.GetKeyDown(KeyCode.H))
            {
                _player.transform.position = _player.transform.position.WithX(_player.transform.position.x + 1f);
            }
        }

        private void HandlePauseScreen()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && _hud)
            {
                if (GameRoot.IsPaused && _pauseScreen.IsOnTop)
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

        public async UniTask<QuestionResult> OpenQuestion()
        {
            _hud.Pause();
            DisableInput = true;
            
            _singleChoiceQuestion.gameObject.SetActive(true);
            
            // todo change to real data
            var question = MockQuestion();

            //select the correct type of window
            await _singleChoiceQuestion.DisplayQuestion(question);
            _singleChoiceQuestion.gameObject.SetActive(false);
            
            DisableInput = false;
            _hud.Resume();

            // pass the reward here
            return new QuestionResult();
        }

        private static Question MockQuestion()
        {
            var question = new Question()
            {
                Content = "Who was the first president of United States of America",
                QuestionType = Question.Types.QuestionType.Abcd,
                Answers =
                {
                    new Question.Types.Answer()
                    {
                        AnswerID = 1,
                        Content = "George Washington",
                        Correct = true
                    },
                    new Question.Types.Answer()
                    {
                        AnswerID = 2,
                        Content = "Thomas Jefferson",
                        Correct = false
                    },
                    new Question.Types.Answer()
                    {
                        AnswerID = 3,
                        Content = "Abraham Lincoln",
                        Correct = false
                    },
                    new Question.Types.Answer()
                    {
                        AnswerID = 4,
                        Content = "Benjamin Franklin",
                        Correct = false
                    },
                }
            };
            return question;
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
            GameRoot.IsPaused = true;
        }
    }

    public class QuestionResult
    {
    }
}
