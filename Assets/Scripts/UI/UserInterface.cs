using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Gameplay;
using Google.Protobuf.Collections;
using Menu;
using Player.Skills;
using UI.HUD;
using UI.Windows;
using UI.Windows.Questions;
using UnityEngine;
using Utils;

namespace UI
{
    public class UserInterface : MonoBehaviour
    {
        [Header("Windows Prefabs")] 
        [SerializeField] private GameHud gameHudPrefab;
        [SerializeField] private MenuWindow menuPrefab;
        [SerializeField] private DeadScreen deadScreenPrefab;
        [SerializeField] private PauseScreen pauseScreenPrefab;
        [SerializeField] private WinScreen winScreenPrefab;
        [SerializeField] private TipDisplay tipDisplayPrefab;
        [SerializeField] private RewardPopup rewardPopupPrefab;
        [SerializeField] private TutorialSystem tutorialSystemPrefab;

        [Header("Question Types")] 
        [SerializeField] private QuestionScreenBase singleChoiceQuestionPrefab;
        [SerializeField] private QuestionScreenBase multiChoiceQuestionPrefab;

        private MenuWindow _menu;
        private GameHud _hud;
        private PauseScreen _pauseScreen;
        private WinScreen _winScreen;
        private DeadScreen _deadScreen;
        private TipDisplay _tipDisplay;
        private RewardPopup _rewardPopup;

        private QuestionScreenBase _singleChoiceQuestion;
        private QuestionScreenBase _multiChoiceQuestion;

        private Player.Player _player;

        private void Update()
        {
            if (GameRoot.IsPaused)
            {
                return;
            }

            Cheats();
        }

        public void Setup(Player.Player player, Skill[] skills)
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
            GameRoot.IsPaused = true;
            //DisableInput = true;

            _singleChoiceQuestion.gameObject.SetActive(true);

            // todo change to real data
            var question = GetMockQuestion();

            //select the correct type of window
            var correct = await _singleChoiceQuestion.DisplayQuestion(question);
            _singleChoiceQuestion.gameObject.SetActive(false);

            // pass the reward here
            var result = correct
                ? new QuestionResult(100, 200, 0, 100)
                : new QuestionResult(0, 0, -1, 0);

            await DisplayReward(result);
            
            GameRoot.IsPaused = false;
            //DisableInput = false;
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
        public async UniTask<StartGameResponse> ShowMenu()
        {
            if (_hud)
            {
                Destroy(_hud.gameObject);
            }

            _menu = Instantiate(menuPrefab, transform);
            StartGameResponse startGameResponse = await _menu.ShowMenu();
            Destroy(_menu.gameObject);
            return startGameResponse;
        }
        
        public async void ShowTutorial()
        {
            if (!_hud) 
                return;

            PauseGame();
            
            var tutorial = Instantiate(tutorialSystemPrefab, transform);
            await tutorial.DisplayTutorial();
            Destroy(tutorial.gameObject);

            ResumeGame();
        }
        
        private void HandlePauseScreen()
        {
            if (!_hud) 
                return;
            
            if (GameRoot.IsPaused && _pauseScreen.IsOnTop)
            {
                _pauseScreen.Resume();
                ResumeGame();
            }
            else
            {
                _pauseScreen.Pause();
                PauseGame();
            }
        }

        public void PauseGame()
        {
            _hud.Pause();
            GameRoot.IsPaused = true;
        }
        
        public void ResumeGame()
        {
            _hud.Resume();
            GameRoot.IsPaused = false;
        }
        
        public async UniTask WinScreen(UniTask<Empty> requestTask)
        {
            if (_winScreen == null)
            {
                _winScreen = Instantiate(winScreenPrefab, transform);
            }
            PauseGame();
            _winScreen.Setup(_player).Forget();

            await requestTask;
            _winScreen.OnRequestSent();
        }
        
        public async UniTask LoseScreen(UniTask<Empty> uniTask)
        {
            if (_deadScreen == null)
            {
                _deadScreen = Instantiate(deadScreenPrefab, transform);
            }
            PauseGame();
            _deadScreen.Setup();

            await uniTask;
            _deadScreen.OnRequestSent();
        }
        
        public int GetEndgamePlaytime() => _hud.GetPlaytime();

#region Debug

        private Queue<QuestionResponse> _mockQuestions;
        [SerializeField] private string[] mockQuestionStrings;
        
        private QuestionResponse GetMockQuestion()
        {
            if (_mockQuestions == null)
            {
                var shuffledList = new List<string>(mockQuestionStrings);
                shuffledList.Shuffle();
                _mockQuestions = new Queue<QuestionResponse>();
                foreach (var s in shuffledList)
                {
                    var q = CreateQuestionFromString(s);
                    _mockQuestions.Enqueue(q);
                }
            }

            var question = _mockQuestions.Dequeue();
            _mockQuestions.Enqueue(question);
            return question;
        }

        private static QuestionResponse CreateQuestionFromString(string question)
        {
            var split = question.Split(';');
            var correct = int.Parse(split[split.Length - 1]);
            var answers = new RepeatedField<QuestionResponse.Types.Answer>();
            for (var i = 1; i < split.Length - 1; i++)
            {
                answers.Add(new QuestionResponse.Types.Answer()
                {
                    AnswersID = (uint) (i - 1),
                    Content = split[i],
                    Correct = correct == i
                });
            }

            return new QuestionResponse()
            {
                Content = split[0],
                Answers = {answers}
            };
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