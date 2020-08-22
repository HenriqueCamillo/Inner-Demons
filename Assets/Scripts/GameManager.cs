using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] float crysisDuration;
    private float timeRemaining;
    [SerializeField] TextMeshProUGUI timer;
    [SerializeField] GameObject pauseMenu;
    public bool isPaused;

    public float TimeRemaining
    {
        get => timeRemaining;
        set
        {
            timeRemaining = Mathf.Clamp(value, 0f, crysisDuration);
            timer.text = ((int)timeRemaining/60).ToString() + ":" + ((int)timeRemaining%60).ToString().PadLeft(2, '0');

            if (timeRemaining == 0)
                GameOver();
        }
    }

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this.gameObject);

        TimeRemaining = crysisDuration;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }

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
        
        Debug.Log("Game over");
        // SceneManager.LoadScene(0);
    }
}