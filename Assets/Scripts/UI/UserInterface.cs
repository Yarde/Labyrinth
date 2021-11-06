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
        [SerializeField] private RewardPopup rewardPopupPrefab;
        [SerializeField] private TutorialSystem tutorialSystemPrefab;

        [Header("Question Types")] 
        [SerializeField] private QuestionScreenBase singleChoiceQuestionPrefab;
        [SerializeField] private QuestionScreenBase multiChoiceQuestionPrefab;

        private MenuWindow _menu;
        private GameHud _hud;
        private WindowState _pauseScreen;
        private WindowState _deadScreen;
        private TipDisplay _tipDisplay;
        private RewardPopup _rewardPopup;

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

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                HandlePauseScreen();
            }
        }

        public void Setup(Player player, Skill[] skills)
        {
            _player = player;

            _hud = Instantiate(gameHudPrefab, transform);
            _hud.Setup(player, skills);
            _hud.SetListeners(HandlePauseScreen, ShowTutorial);

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
            var correct = await _singleChoiceQuestion.DisplayQuestion(question);
            _singleChoiceQuestion.gameObject.SetActive(false);

            // pass the reward here
            var result = correct
                ? new QuestionResult(100, 200, 0)
                : new QuestionResult(0, 0, -1);

            await DisplayReward(result);
            
            DisableInput = false;
            _hud.Resume();
            
            return result;
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

        private async UniTask DisplayReward(QuestionResult result)
        {
            if (_rewardPopup == null)
            {
                _rewardPopup = Instantiate(rewardPopupPrefab, transform);
                _rewardPopup.SetupRewardPopup();
            }
            else
            {
                _rewardPopup.gameObject.SetActive(true);
            }

            await _rewardPopup.DisplayRewards(result);
            _rewardPopup.gameObject.SetActive(false);
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
        
        private void ShowTutorial()
        {
            var tutorial = Instantiate(tutorialSystemPrefab, transform);
        }
        
        private void HandlePauseScreen()
        {
            if (!_hud) 
                return;
            
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

        public void PauseGame()
        {
            DisableInput = true;
            _hud.Pause();
            GameRoot.IsPaused = true;
        }
        
        public void LoseScreen()
        {
            if (_deadScreen == null)
            {
                _deadScreen = Instantiate(deadScreenPrefab, transform);
                _deadScreen.Setup();
            }
            _deadScreen.OnEnter();
            
            DisableInput = true;
            _hud.Pause();
            GameRoot.IsPaused = true;
        }
        
        #region Debug
        private static Question MockQuestion()
        {
            var question = new Question
            {
                Content = "Who was the first president of United States of America",
                QuestionType = Question.Types.QuestionType.Abcd,
                Answers =
                {
                    new Question.Types.Answer
                    {
                        AnswerID = 1,
                        Content = "George Washington",
                        Correct = true
                    },
                    new Question.Types.Answer
                    {
                        AnswerID = 2,
                        Content = "Thomas Jefferson",
                        Correct = false
                    },
                    new Question.Types.Answer
                    {
                        AnswerID = 3,
                        Content = "Abraham Lincoln",
                        Correct = false
                    },
                    new Question.Types.Answer
                    {
                        AnswerID = 4,
                        Content = "Benjamin Franklin",
                        Correct = false
                    },
                }
            };
            return question;
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
        #endregion
    }
}