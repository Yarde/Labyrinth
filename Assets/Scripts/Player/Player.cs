using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using GameData;
using Gameplay;
using Labirynth.Questions;
using Network;
using UI;
using UI.Windows.Questions;
using UnityEngine;
using QuestionTrigger = Labirynth.Questions.QuestionTrigger;

namespace Player
{
    [RequireComponent(typeof(PlayerMovement))]
    public class Player : MonoBehaviour 
    {
        private const string NOT_ENOUGH_KEYS_TIP_TEXT = "Come back where you collected all the Keys";

        [SerializeField] private UserInterface ui;
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

        public void Setup(Dictionary<Type, ObjectiveData> objectives, Vector2Int dimensions)
        {
            transform.position = new Vector3(dimensions.x/2f, 0f, dimensions.y/2f);

            Objectives = objectives;
        
            LightLevel = baseLightStrength;
            FieldOfViewLevel = baseViewDistance;
            MovementSpeed = baseMovementSpeed;
            Experience = 0;
            Hearts = 1;
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
            var trigger = collision.collider.GetComponent<QuestionTrigger>();
            if (!trigger.Collected)
            {
                await OpenQuestion(trigger);
            }
        }
    
        private async Task HandleExit()
        {
            var mainObjective = Objectives[typeof(Key)];
            if (mainObjective.Collected == mainObjective.Total)
            {
                Playtime += ui.GetEndgamePlaytime();
                Points = CalculatePoints();
                await ui.WinScreen();
            }
            else
            {
                ui.DisplayTip(NOT_ENOUGH_KEYS_TIP_TEXT).Forget();
            }
        }
        private int CalculatePoints()
        {
            var toAdd = 0;
            toAdd += Coins;
            toAdd += Hearts * 20;

            return toAdd;
        }

        private async UniTask OpenQuestion(QuestionTrigger trigger)
        {
            Debug.Log($"Collision entered with {trigger.name}");
            QuestionResult result = await ui.OpenQuestion();
        
            Coins += result.Coins;
            Experience += result.Experience;
            Hearts += result.Hearts;
            Points += result.Points;

            if (Hearts == 0)
            {
                var request = new EndGameRequest
                {
                    SessionCode = GameRoot.SessionCode,
                    GameplayTime = (uint) Playtime,
                    ScenarioEnded = false,
                    StudentEndGameData = new EndGameRequest.Types.StudentEndGameData
                    {
                        Experience = Experience,
                        Money = Coins
                    }
                };
                UniTask<Empty> task = ConnectionManager.Instance.SendMessageAsync<Empty>(request, "dawid/end");
                await ui.LoseScreen(task);
            }
        
            Objectives[trigger.GetType()].Collected++;
            await trigger.Destroy();
        }
    }
}