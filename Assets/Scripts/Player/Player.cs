using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameData;
using Labirynth.Questions;
using Skills;
using UI;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class Player : MonoBehaviour 
{
    [SerializeField] private UserInterface ui;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float baseLightStrength = 5f;
    [SerializeField] private float baseViewDistance = 0.1f;
    [SerializeField] private float baseMovementSpeed = 2f;

    public int Hearts { get; private set; }
    public int Points { get; private set; }
    public int Experience { get; private set; }
    public int Playtime { get; private set; }
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
        // todo you can collide with already answered question, fix it
        if (collision.collider.CompareTag("QuestionTrigger"))
        {
            var trigger = collision.collider.GetComponent<QuestionTrigger>();
            if (!trigger.Collected)
            {
                await OpenQuestion(trigger);
            }
        }
        // todo optimize before use
        /*else if (collision.collider.CompareTag("Floor"))
        {
            var trigger = collision.collider.GetComponent<Floor>();
            trigger.MarkVisited();
        }*/
    }

    private async UniTask OpenQuestion(QuestionTrigger trigger)
    {
        Debug.Log($"Collision entered with {trigger.name}");
        await ui.OpenQuestion();

        Objectives[trigger.GetType()].Collected++;

        Coins += 100;
        Experience += 200;

        await trigger.Destroy();
    }
}