using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using GameData;
using Gameplay.Skills;
using Labirynth.Questions;
using Network;
using UI;
using UnityEngine;
using Logger = UI.Logger;

namespace Gameplay
{
    [RequireComponent(typeof(PlayerMovement))]
    public class Player : MonoBehaviour
    {
        private const string NOT_ENOUGH_KEYS_TIP_TEXT = "Come back when you collect all the Keys";

        [SerializeField] private SkillData[] skills;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private float baseLightStrength = 0f;
        [SerializeField] private float baseViewDistance = 0f;
        [SerializeField] private float baseMovementSpeed = 2f;

        public int Hearts { get; private set; }
        public int Points { get; set; }
        public int Experience { get; set; }
        public int Level { get; set; }
        public int Playtime { get; private set; }
        public int Coins { get; set; }
        public float LightLevel { get; set; }
        public float FieldOfViewLevel { get; set; }
        public float MovementSpeed { get; set; }
        public Dictionary<Type, ObjectiveData> Objectives { get; set; }

        private UserInterface _ui;
        public Dictionary<SkillTypes, Skill> Skills;
        private bool _isQuestionOpen;

        public void Setup(Dictionary<Type, ObjectiveData> objectives, Vector2Int dimensions, UserInterface ui)
        {
            transform.position = new Vector3(dimensions.x / 2f, 0f, dimensions.y / 2f);

            Objectives = objectives;
            _ui = ui;

            LightLevel = baseLightStrength;
            FieldOfViewLevel = baseViewDistance;
            MovementSpeed = baseMovementSpeed;
            Hearts = 5;
            SetupSkills();
        }

        private void SetupSkills()
        {
            Skills = new Dictionary<SkillTypes, Skill>
            {
                {SkillTypes.Light, new UpgradeLight(this, skills.FirstOrDefault(x => x.skillName == "UpgradeLight"))},
                {SkillTypes.Vision, new UpgradeVision(this, skills.FirstOrDefault(x => x.skillName == "UpgradeVision"))},
                {SkillTypes.Movement, new UpgradeMovement(this, skills.FirstOrDefault(x => x.skillName == "UpgradeMovement"))}
            };
        }

        private async void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.CompareTag("QuestionTrigger"))
            {
                await HandleQuestion(collision);
            }
            else if (collision.collider.CompareTag("Exit"))
            {
                await HandleExit();
            }
            // todo optimize before use
            /*else if (collision.collider.CompareTag("Floor"))
            {
                var trigger = collision.collider.GetComponent<Floor>();
                trigger.MarkVisited();
            }*/
        }

        private async UniTask HandleQuestion(Collision collision)
        {
            var trigger = collision.collider.GetComponent<Labirynth.Questions.QuestionTrigger>();
            if (!trigger.Collected && !_isQuestionOpen)
            {
                _isQuestionOpen = true;
                await OpenQuestion(trigger);
            }
        }

        private async UniTask OpenQuestion(Labirynth.Questions.QuestionTrigger trigger)
        {
            Logger.Log($"Collision entered with {trigger.name}");
            var result = await _ui.HandleQuestion(trigger.GetType());

            Coins += result.Coins;
            Experience += result.Experience;
            Hearts += result.Hearts;
            Points += result.Points;
            await HandleGameLost();

            _isQuestionOpen = false;
            Objectives[trigger.GetType()].Collected++;
            await trigger.Destroy();
        }

        private async UniTask HandleGameLost()
        {
            if (Hearts == 0)
            {
                if (GameRoot.IsDebug)
                {
                    Playtime += _ui.GetEndgamePlaytime();
                    Points = CalculatePoints();
                    await _ui.LoseScreen(UniTask.CompletedTask);
                    return;
                }

                var requestTask = SendEndGameRequest(false);
                await _ui.LoseScreen(requestTask);
            }
        }

        private async UniTask HandleExit()
        {
            var mainObjective = Objectives[typeof(Key)];
            if (mainObjective.Collected == mainObjective.Total)
            {
                if (GameRoot.IsDebug)
                {
                    Playtime += _ui.GetEndgamePlaytime();
                    Points = CalculatePoints();
                    await _ui.WinScreen(UniTask.CompletedTask);
                    return;
                }

                var requestTask = SendEndGameRequest(true);
                await _ui.WinScreen(requestTask);
            }
            else
            {
                _ui.DisplayTip(NOT_ENOUGH_KEYS_TIP_TEXT).Forget();
            }
        }

        private UniTask<Empty> SendEndGameRequest(bool isWon)
        {
            Playtime += _ui.GetEndgamePlaytime();
            Points = CalculatePoints();
            var request = new EndGameRequest
            {
                SessionCode = GameRoot.SessionCode,
                GameplayTime = Playtime,
                ScenarioEnded = isWon,
                StudentEndGameData = new EndGameRequest.Types.StudentEndGameData
                {
                    Experience = Experience,
                    Money = Coins,
                    Light = Skills[SkillTypes.Light].CompletionPercentage,
                    Vision = Skills[SkillTypes.Vision].CompletionPercentage,
                    Speed = Skills[SkillTypes.Movement].CompletionPercentage
                }
            };
            var requestTask = ConnectionManager.Instance.SendMessageAsync<Empty>(request, Endpoints.EndGame);
            return requestTask;
        }

        private int CalculatePoints()
        {
            var toAdd = 0;
            toAdd += Coins;
            toAdd += Hearts * 20;

            return toAdd;
        }

        public enum SkillTypes
        {
            Light = 0,
            Vision = 1,
            Movement = 2
        }
    }
}