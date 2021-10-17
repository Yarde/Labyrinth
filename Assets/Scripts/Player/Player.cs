using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameData;
using Labirynth.Questions;
using Skills;
using UI;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerRotation))]
[RequireComponent(typeof(AudioSource))]
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

    public void Setup(Dictionary<Type, ObjectiveData> objectives)
    {
        // todo get dimension of maze somehow
        transform.position = new Vector3(10f, 0f, 10f);

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
            await OpenQuestion(trigger);
        }
    }

    private async UniTask OpenQuestion(QuestionTrigger trigger)
    {
        Debug.Log($"Collision entered with {trigger.name}");
        await ui.OpenQuestion();

        Objectives[trigger.GetType()].Collected++;

        Coins += 100;
        Experience += 200;

        trigger.Destroy();
    }
}