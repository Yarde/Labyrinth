using System;
using System.Collections.Generic;
using System.Linq;
using GameData;
using Labirynth;
using Labirynth.Questions;
using Skills;
using UI;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameRoot : MonoBehaviour
{
    public static bool IsPaused = false;
    
    [SerializeField] private Player player;
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

    private async void Awake()
    {
        var data = await ui.ShowMenu();
        StartGame(null);
    }

    private void StartGame(object data)
    {
        SetupObjectives();
        SetupGeneratorData();
        labirynth.Setup(_generatorData);
        player.Setup(_objectives, _generatorData.Dimensions);
        
        SetupSkills();
        
        ui.Setup(player, _skills);
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
    
    private void SetupObjectives()
    {
        _objectives = new Dictionary<Type, ObjectiveData>
        {
            {typeof(Treasure), new ObjectiveData {Collected = 0, Total = 10, Prefab = treasurePrefab}},
            {typeof(Key), new ObjectiveData {Collected = 0, Total = 10, Prefab = keyPrefab}},
            {typeof(Enemy), new ObjectiveData {Collected = 0, Total = 10, Prefab = enemyPrefab}}
        };
    }

    private void SetupGeneratorData()
    {
        _generatorData = new GeneratorData()
        {
            Dimensions = new Vector2Int(20, 20),
            Objectives = _objectives,
            Seed = Random.Range(0, 100000),
        };
    }
}