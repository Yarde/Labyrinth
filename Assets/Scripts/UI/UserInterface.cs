using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        [Header("Question Types")] [SerializeField]
        private QuestionScreenBase singleChoiceQuestionPrefab;

        [SerializeField] private QuestionScreenBase multiChoiceQuestionPrefab;

        private MenuWindow _menu;
        private GameHud _hud;
        private PauseScreen _pauseScreen;
        private WinScreen _winScreen;
        private DeadScreen _deadScreen;
        private TipDisplay _tipDisplay;
        private RewardPopup _rewardPopup;
        private LoadingScreen _loadingScreen;

        private QuestionScreenBase _singleChoiceQuestion;
        private QuestionScreenBase _multiChoiceQuestion;

        private Player _player;

        private void Update()
        {
            if (GameRoot.IsPaused)
            {
                return;
            }

            if (_hud)
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

            if (!_singleChoiceQuestion)
            {
                _singleChoiceQuestion = Instantiate(singleChoiceQuestionPrefab, transform);
                _singleChoiceQuestion.gameObject.SetActive(false);
            }
        }

        public async UniTask<QuestionResult> HandleQuestion(Type type)
        {
            PauseGame();
            var question = await SendQuestionRequest(type);
            var result = await OpenQuestionWindow(question);
            await DisplayReward(result);
            ResumeGame();

            return result;
        }

        private async UniTask<QuestionResponse> SendQuestionRequest(Type type)
        {
            var request = new QuestionRequest
            {
                SessionCode = GameRoot.SessionCode,
                QuestionType = GetQuestionType(type)
            };
            var question = await ConnectionManager.Instance.SendMessageAsync<QuestionResponse>(request, "next-question", true);
            return question;
        }
        
        private async UniTask<QuestionResult> OpenQuestionWindow(QuestionResponse question)
        {
            var answer = await _singleChoiceQuestion.DisplayQuestion(question);
            ConnectionManager.Instance.SendMessageAsync<Empty>(answer, "answer").Forget();

            var result = GetQuestionResult(question, answer);
            return result;
        }

        private static QuestionResult GetQuestionResult(QuestionResponse question, StudentAnswerRequest answer)
        {
            var correctness = Mathf.Max(answer.QuestionCorrectnes, 0f);
            var coins = (int) (question.QuestionReward.Money * correctness);
            var exp = (int) (question.QuestionReward.Experience * correctness);
            var points = (int) (100 * correctness);
            var result = new QuestionResult(coins, exp, correctness <= 0f ? -1 : 0, points);
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

        private void PauseGame()
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
                    AnswersID = i - 1,
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