﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] float crisisDuration;
    private float timeRemaining;
    [SerializeField] TextMeshProUGUI timer;
    [SerializeField] GameObject pauseMenu;
    public bool isPaused;
    private bool timerEnabled = true;
    [SerializeField] GameObject endGameScreen;
    [SerializeField] GameObject tentacles;
    [SerializeField] TextMeshProUGUI endTimer;
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] GameObject fade;


    public static Action OnVictory;
    public static Action OnDeath;

    public float TimeRemaining
    {
        get => timeRemaining;
        set
        {
            timeRemaining = Mathf.Clamp(value, 0f, crisisDuration);
            timer.text = ((int)timeRemaining/60).ToString() + ":" + ((int)timeRemaining%60).ToString().PadLeft(2, '0');

            if (timeRemaining == 0)
            {
                timerEnabled = false;
                OvercameCrisis();
            }
        }
    }

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this.gameObject);

        TimeRemaining = crisisDuration;
        endGameScreen.SetActive(false);
        pauseMenu.SetActive(false);
        tentacles.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }

        if (timerEnabled)
            TimeRemaining -= Time.deltaTime;
    }

    public void Pause()
    {
        if (isPaused)
            Time.timeScale = 1f;
        else 
            Time.timeScale = 0f;

        isPaused = !isPaused;
        pauseMenu.SetActive(isPaused);
    }

    public void GameOver()
    {
        timerEnabled = false;
        endTimer.text = timer.text;
        tentacles.SetActive(true);
        title.text = "Voce nao resistiu a crise";
        endGameScreen.SetActive(true);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Vote()
    {
        Application.OpenURL("https://rebrand.ly/innerdemons");
    }

    private void OvercameCrisis()
    {
        // endTimer.text = timer.text;
        // title.text = "Voce resistiu a crise";
        tentacles.SetActive(false);
        // endGameScreen.SetActive(true);
        OnVictory?.Invoke();
        fade.SetActive(true);
        Invoke(nameof(End), 4f);
    }

    public void End()
    {
        SceneManager.LoadScene(2);
    }
}