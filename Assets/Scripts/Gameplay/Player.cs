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

namespace Gameplay
{
    [RequireComponent(typeof(PlayerMovement))]
    public class Player : MonoBehaviour 
    {
        private const string NOT_ENOUGH_KEYS_TIP_TEXT = "Come back where you collected all the Keys";

        [SerializeField] private SkillData[] skills;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private float baseLightStrength = 5f;
        [SerializeField] private float baseViewDistance = 0.1f;
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
        public Skill[] Skills;

        public void Setup(Dictionary<Type, ObjectiveData> objectives, Vector2Int dimensions, UserInterface ui)
        {
            transform.position = new Vector3(dimensions.x/2f, 0f, dimensions.y/2f);

            Objectives = objectives;
            _ui = ui;
        
            LightLevel = baseLightStrength;
            FieldOfViewLevel = baseViewDistance;
            MovementSpeed = baseMovementSpeed;
            Experience = 0;
            Hearts = 1;
            SetupSkills();
        }
        
        private void SetupSkills()
        {
            Skills = new Skill[]
            {
                new UpgradeLight(this, skills.FirstOrDefault(x => x.skillName == "UpgradeLight")),
                new UpgradeVision(this, skills.FirstOrDefault(x => x.skillName == "UpgradeVision")),
                new UpgradeMovement(this, skills.FirstOrDefault(x => x.skillName == "UpgradeMovement"))
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
            if (!trigger.Collected)
            {
                await OpenQuestion(trigger);
            }
        }

        private async UniTask OpenQuestion(Labirynth.Questions.QuestionTrigger trigger)
        {
            Debug.Log($"Collision entered with {trigger.name}");
            var result = await _ui.HandleQuestion(trigger.GetType());
        
            Coins += result.Coins;
            Experience += result.Experience;
            Hearts += result.Hearts;
            Points += result.Points;
            await HandleGameLost();
        
            Objectives[trigger.GetType()].Collected++;
            await trigger.Destroy();
        }

        private async UniTask HandleGameLost()
        {
            if (Hearts == 0)
            {
                Playtime += _ui.GetEndgamePlaytime();
                var request = new EndGameRequest
                {
                    SessionCode = GameRoot.SessionCode,
                    GameplayTime = Playtime,
                    ScenarioEnded = false,
                    StudentEndGameData = new EndGameRequest.Types.StudentEndGameData
                    {
                        Experience = Experience,
                        Money = Coins,
                        Skills = {Skills.Select(x => x.CompletionPercentage)}
                    }
                };
                var task = ConnectionManager.Instance.SendMessageAsync<Empty>(request, Endpoints.EndGame);
                await _ui.LoseScreen(task);
            }
        }
        
        private async UniTask HandleExit()
        {
            var mainObjective = Objectives[typeof(Key)];
            if (mainObjective.Collected == mainObjective.Total)
            {
                Playtime += _ui.GetEndgamePlaytime();
                Points = CalculatePoints();
                var request = new EndGameRequest
                {
                    SessionCode = GameRoot.SessionCode,
                    GameplayTime = Playtime,
                    ScenarioEnded = true,
                    StudentEndGameData = new EndGameRequest.Types.StudentEndGameData
                    {
                        Experience = Experience,
                        Money = Coins
                    }
                };
                var requestTask = ConnectionManager.Instance.SendMessageAsync<Empty>(request, Endpoints.EndGame);
                await _ui.WinScreen(requestTask);
            }
            else
            {
                _ui.DisplayTip(NOT_ENOUGH_KEYS_TIP_TEXT).Forget();
            }
        }
        
        private int CalculatePoints()
        {
            var toAdd = 0;
            toAdd += Coins;
            toAdd += Hearts * 20;

            return toAdd;
        }
    }
}