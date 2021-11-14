using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using GameData;
using Labirynth.Questions;
using Player.Skills;
using UI;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(PlayerMovement))]
    public class Player : MonoBehaviour 
    {
        private const string NOT_ENOUGH_KEYS_TIP_TEXT = "Come back where you collected all the Keys";
        private const string GAME_WON_TIP_TEXT = "You won! Congratulations";

        [SerializeField] private UserInterface ui;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private float baseLightStrength = 5f;
        [SerializeField] private float baseViewDistance = 0.1f;
        [SerializeField] private float baseMovementSpeed = 2f;

        public int Hearts { get; private set; }
        public int Points { get; private set; }
        public int Experience { get; set; }
        public int Playtime { get; set; }
        public int Coins { get; set; }
        public float LightLevel { get; set; }
        public float FieldOfViewLevel { get; set; }
        public float MovementSpeed { get; set; }
        public Dictionary<Type, ObjectiveData> Objectives { get; set; }
        private Skill[] _skills;

        public void Setup(Dictionary<Type, ObjectiveData> objectives, Vector2Int dimensions)
        {
            transform.position = new Vector3(dimensions.x/2f, 0f, dimensions.y/2f);

            Objectives = objectives;
        
            LightLevel = baseLightStrength;
            FieldOfViewLevel = baseViewDistance;
            MovementSpeed = baseMovementSpeed;
            Experience = 0;
            Hearts = 5;
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
                // todo game won, do something
                ui.PauseGame();
                await ui.DisplayTip(GAME_WON_TIP_TEXT);
            }
            else
            {
                ui.DisplayTip(NOT_ENOUGH_KEYS_TIP_TEXT).Forget();
            }
        }

        private async UniTask OpenQuestion(QuestionTrigger trigger)
        {
            Debug.Log($"Collision entered with {trigger.name}");
            var result = await ui.OpenQuestion();
        
            Coins += result.Coins;
            Experience += result.Experience;
            Hearts += result.Hearts;

            if (Hearts == 0)
            {
                ui.LoseScreen();
                // todo send notification that player lost
            }
        
            Objectives[trigger.GetType()].Collected++;
            await trigger.Destroy();
        }
    }
}