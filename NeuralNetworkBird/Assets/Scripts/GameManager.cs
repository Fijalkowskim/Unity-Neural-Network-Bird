using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    float timeScale = 1;
    public bool gamePaused{ get; private set; }
    public UnityEvent<bool> onPauseToggle{ get; private set; }

private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            Init();
        }    
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) TogglePause();
    }
    void Init()
    {
        gamePaused = false;
        timeScale = 1;
    }
    public void TogglePause()
    {
        if (gamePaused)
        {
            Time.timeScale = timeScale;
            gamePaused = false;
        }
        else
        {
            Time.timeScale = 0;
            gamePaused = true;
        }

        onPauseToggle?.Invoke(gamePaused);
    }
    public void setTimeScale(float newValue)
    {
        timeScale = newValue;
        Time.timeScale = newValue;
    }
}
