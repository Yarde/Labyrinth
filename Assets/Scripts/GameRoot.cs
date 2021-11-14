using System;
using System.Collections.Generic;
using System.Linq;
using GameData;
using Gameplay;
using Labirynth;
using Labirynth.Questions;
using Network;
using Player.Skills;
using UI;
using UnityEngine;
using QuestionTrigger = Labirynth.Questions.QuestionTrigger;
using Random = UnityEngine.Random;

public class GameRoot : MonoBehaviour
{
    public static bool IsPaused = false;
    public static string SessionCode;

    [Header("Server Settings")]
    [SerializeField] private string serverHost = "http://zpi2021.westeurope.cloudapp.azure.com/api/";

    [SerializeField] private Player.Player player;
    [SerializeField] private LabirynthGenerator labirynth;
    [SerializeField] private UserInterface ui;
    [SerializeField] private SkillData[] skills;

    [Header("Object to instantiate")]
    [SerializeField] private QuestionTrigger treasurePrefab;
    [SerializeField] private QuestionTrigger keyPrefab;
    [SerializeField] private QuestionTrigger enemyPrefab;

    private Dictionary<Type, ObjectiveData> _objectives;
    private GeneratorData _generatorData;
    private Skill[] _skills;
    private ConnectionManager _connectionManager;

    private async void Awake()
    {
        _connectionManager = new ConnectionManager(serverHost);
        StartGameResponse startGameResponse = await ui.ShowMenu();
        StartGame(startGameResponse);
    }

    private void StartGame(StartGameResponse response)
    {
        SessionCode = response.SessionCode;

        SetupObjectives(response);
        SetupGeneratorData(response);
        labirynth.Setup(_generatorData);
        player.Setup(_objectives, _generatorData.Dimensions);
        SetupSkills();

        ui.Setup(player, _skills);
        
        if (response.StudentData != null)
        {
            player.Experience = response.StudentData.Experience;
            player.Coins = response.StudentData.Money;
        }
        else
        {
            ui.ShowTutorial();
        }
    }

    private void SetupSkills()
    {
        _skills = new Skill[]
        {
            new UpgradeLight(player, skills.FirstOrDefault(x => x.skillName == "UpgradeLight")),
            new UpgradeVision(player, skills.FirstOrDefault(x => x.skillName == "UpgradeVision")),
            new UpgradeMovement(player, skills.FirstOrDefault(x => x.skillName == "UpgradeMovement"))
        };
    }

    private void SetupObjectives(StartGameResponse response)
    {
        _objectives = new Dictionary<Type, ObjectiveData>
        {
            { typeof(Key), new ObjectiveData { Total = response.QuestionsNumber[0], Prefab = keyPrefab } },
            { typeof(Enemy), new ObjectiveData { Total = response.QuestionsNumber[1], Prefab = enemyPrefab } },
            { typeof(Treasure), new ObjectiveData { Total = response.QuestionsNumber[2], Prefab = treasurePrefab } }
        };
    }

    private void SetupGeneratorData(StartGameResponse response)
    {
        if (response.MazeSetting != null)
        {
            _generatorData = new GeneratorData()
            {
                Dimensions = new Vector2Int(response.MazeSetting.Size, response.MazeSetting.Size),
                Objectives = _objectives,
                Seed = response.MazeSetting.Seed,
            };
        }
        else
        {
            var totalObjectives = response.QuestionsNumber.Sum();
            _generatorData = new GeneratorData()
            {
                Dimensions = new Vector2Int(totalObjectives*2/3, totalObjectives*2/3),
                Objectives = _objectives,
                Seed = Random.Range(0, 100000),
            };
        }
    }
}
