using System;
using System.Collections.Generic;
using System.Linq;
using GameData;
using Gameplay;
using Labirynth;
using Labirynth.Questions;
using Network;
using UI;
using UnityEngine;
using QuestionTrigger = Labirynth.Questions.QuestionTrigger;
using Random = UnityEngine.Random;

public class GameRoot : MonoBehaviour
{
    public static bool IsPaused = false;
    public static string SessionCode;
    public static bool IsDebug = false;
    public static bool CheatsAllowed = false;

        [Header("Server Settings")]
    [SerializeField] private string serverHost = "http://zpi2021.westeurope.cloudapp.azure.com/api/";

    [Header("Bindings")]
    [SerializeField] private Player player;
    [SerializeField] private LabirynthGenerator labirynth;
    [SerializeField] private UserInterface ui;

    [Header("Object to instantiate")]
    [SerializeField] private QuestionTrigger treasurePrefab;
    [SerializeField] private QuestionTrigger keyPrefab;
    [SerializeField] private QuestionTrigger enemyPrefab;

    private Dictionary<Type, ObjectiveData> _objectives;
    private GeneratorData _generatorData;
    private ConnectionManager _connectionManager;
        
    private int _currentCheatSequence;

    private async void Awake()
    {
        _connectionManager = new ConnectionManager(serverHost, ui);

        var startGameResponse = await ui.ShowMenu();
        StartGame(startGameResponse);
        //ui.PauseGame();
    }

    private void Update()
    {
        if (!CheatsAllowed)
        {
            TryEnableCheats();
        }
    }

    private void StartGame(StartGameResponse response)
    {
        SessionCode = response.SessionCode;

        SetupObjectives(response);
        SetupGeneratorData(response);
        labirynth.Setup(_generatorData);
        player.Setup(_objectives, _generatorData.Dimensions, ui);
        ui.Setup(player);
        
        if (response.StudentData.Experience == 0)
        {
            ui.ShowTutorial();
        }
        else
        {
            player.Experience = response.StudentData.Experience;
            player.Coins = response.StudentData.Money;
        }
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
        var totalObjectives = response.QuestionsNumber.Sum();
        var size = response.MazeSetting.Size != 0 ? response.MazeSetting.Size : totalObjectives * 2 / 3;
        var seed = response.MazeSetting.Seed != 0 ? response.MazeSetting.Seed : Random.Range(0, 100000);
        
        Debug.Log($"Seed: {seed}, Size: {size}");
        
        _generatorData = new GeneratorData
        {
            Dimensions = new Vector2Int(size, size),
            Objectives = _objectives,
            Seed = seed
        };
    }
    
    private void TryEnableCheats()
    {
        if (Input.GetKey(KeyCode.L) && _currentCheatSequence == 0)
        {
            _currentCheatSequence++;
        }
        else if (Input.GetKey(KeyCode.O) && _currentCheatSequence == 1)
        {
            _currentCheatSequence++;
        }
        else if (Input.GetKey(KeyCode.L) && _currentCheatSequence == 2)
        {
            CheatsAllowed = true;
        }
    }
}
