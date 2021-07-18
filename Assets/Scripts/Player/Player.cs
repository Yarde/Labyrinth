using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerRotation))]
[RequireComponent(typeof(AudioSource))]
public class Player : MonoBehaviour {
    private UserInterface _ui;
    private AudioSource _audioSource;
    public float lightStrength = 5;

    private void Start()
    {
        _ui = GetComponentInChildren<UserInterface>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 0)
                ResumeGame();
            else
                PauseGame();
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            lightStrength++;
        }
    }
    
    void PauseGame()
    {
        _ui.PauseScreen();
    }

    public void ResumeGame()
    {
        _ui.Resume();
    }
}