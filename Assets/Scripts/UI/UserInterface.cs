using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Gameplay;
using Google.Protobuf.Collections;
using Labirynth.Questions;
using Menu;
using Network;
using UI.HUD;
using UI.Windows;
using UI.Windows.Questions;
using UnityEngine;
using Utils;
using QuestionTrigger = Gameplay.QuestionTrigger;

namespace UI
{
    public class UserInterface : MonoBehaviour
    {
        private const string FIRST_OBJECTIVE_FOUND_TEXT = "You found your first {0}, only {1} more to go!";
        private const string LAST_OBJECTIVE_FOUND_TEXT = "You found all Keys! Now go to Exit";
        
        [Header("Windows Prefabs")] [SerializeField]
        private GameHud gameHudPrefab;

        [SerializeField] private MenuWindow menuPrefab;
        [SerializeField] private DeadScreen deadScreenPrefab;
        [SerializeField] private PauseScreen pauseScreenPrefab;
        [SerializeField] private WinScreen winScreenPrefab;
        [SerializeField] private TipDisplay tipDisplayPrefab;
        [SerializeField] private RewardPopup rewardPopupPrefab;
        [SerializeField] private TutorialSystem tutorialSystemPrefab;
        [SerializeField] private LoadingScreen loadingScreenPrefab;
        [SerializeField] private QuestionWindowManager questionWindowManager;

        private MenuWindow _menu;
        private GameHud _hud;
        private PauseScreen _pauseScreen;
        private WinScreen _winScreen;
        private DeadScreen _deadScreen;
        private TipDisplay _tipDisplay;
        private RewardPopup _rewardPopup;
        private LoadingScreen _loadingScreen;

        private Player _player;

        private void Update()
        {
            if (GameRoot.IsPaused)
            {
                return;
            }

            if (_hud && GameRoot.CheatsAllowed)
            {
                Cheats();
            }
        }

        public void Setup(Player player)
        {
            _player = player;

            _hud = Instantiate(gameHudPrefab, transform);
            _hud.Setup(player);
            _hud.SetListeners(HandlePauseScreen, ShowTutorial);

            if (!_pauseScreen)
            {
                _pauseScreen = Instantiate(pauseScreenPrefab, transform);
                _pauseScreen.Setup(ResumeGame);
                _pauseScreen.gameObject.SetActive(false);
            }
            
            if (!_loadingScreen)
            {
                _loadingScreen = Instantiate(loadingScreenPrefab, transform);
                _loadingScreen.gameObject.SetActive(false);
            }
        }

        public async UniTask<QuestionResult> HandleQuestion(Type type)
        {
            PauseGame();
            var questionType = GetQuestionType(type);
            QuestionResponse questionResponse;
            if (GameRoot.IsDebug)
            {
                questionResponse = GetMockQuestion();
            }
            else
            {
                questionResponse = await SendQuestionRequest(questionType);
            }
            
            var answer = await ShowQuestionWindow(questionResponse, questionType);
            
            var result = GetQuestionResult(questionResponse, answer.QuestionCorrectnes);
            await DisplayResult(result);
            await EncouragementTip(type);
            ResumeGame();
            
            return result;
        }

        private async UniTask EncouragementTip(Type type)
        {
            var objective = _player.Objectives[type];
            if (type == typeof(Key))
            {
                if (objective.Collected == 0)
                {
                    await DisplayTip(string.Format(FIRST_OBJECTIVE_FOUND_TEXT, "Key", objective.Total - 1));
                }
                if (objective.Collected == objective.Total - 1)
                {
                    await DisplayTip(LAST_OBJECTIVE_FOUND_TEXT);
                }
            }
            
        }

        private async UniTask<StudentAnswerRequest> ShowQuestionWindow(QuestionResponse response, QuestionTrigger questionType)
        {
            var questionWindow = questionWindowManager.GetWindow(response);
            var answer = await questionWindow.DisplayQuestion(response);
            
            answer.QuestionType = questionType;
            if (!GameRoot.IsDebug)
            {
                SendQuestionAnswer(answer);
            }

            return answer;
        }

        private async UniTask<QuestionResponse> SendQuestionRequest(QuestionTrigger type)
        {
            var request = new QuestionRequest
            {
                SessionCode = GameRoot.SessionCode,
                QuestionType = type
            };
            var question = await ConnectionManager.Instance.SendMessageAsync<QuestionResponse>(request, Endpoints.Question, true);
            return question;
        }
        
        private void SendQuestionAnswer(StudentAnswerRequest answer)
        {
            ConnectionManager.Instance.SendMessageAsync<Empty>(answer, Endpoints.Answer).Forget();
        }

        private static QuestionResult GetQuestionResult(QuestionResponse question, float correctness)
        {
            var correctnessClamped = Mathf.Clamp(correctness, 0f, 1f);
            var coins = (int) (question.QuestionReward.Money * correctnessClamped);
            var exp = (int) (question.QuestionReward.Experience * correctnessClamped);
            var points = (int) (100 * correctnessClamped);
            var result = new QuestionResult(coins, exp, correctnessClamped <= 0f ? -1 : 0, points);
            return result;
        }

        private QuestionTrigger GetQuestionType(Type type)
        {
            return type == typeof(Key) ? QuestionTrigger.Key :
                type == typeof(Enemy) ? QuestionTrigger.Enemy : QuestionTrigger.Object;
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

        private async UniTask DisplayResult(QuestionResult result)
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

        public async UniTask<StartGameResponse> ShowMenu()
        {
            if (_hud)
            {
                Destroy(_hud.gameObject);
            }

            _menu = Instantiate(menuPrefab, transform);
            var startGameResponse = await _menu.ShowMenu();
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

            if (GameRoot.IsPaused)
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

        private void ResumeGame()
        {
            _hud.Resume();
            GameRoot.IsPaused = false;
        }

        public void SetLoadingActive(bool active)
        {
            _loadingScreen.gameObject.SetActive(active);
        }

        public async UniTask WinScreen(UniTask requestTask)
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

        public async UniTask LoseScreen(UniTask uniTask)
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
                answers.Add(new QuestionResponse.Types.Answer
                {
                    AnswersID = i - 1,
                    Content = split[i],
                    Correct = correct == i
                });
            }

            return new QuestionResponse
            {
                Content = split[0],
                Answers = {answers},
                QuestionReward = new QuestionResponse.Types.QuestionReward
                {
                    Experience = 100,
                    Money = 100
                }
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