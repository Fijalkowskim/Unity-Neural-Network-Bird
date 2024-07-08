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
    public bool generationSummary{ get; private set; }
    public UnityEvent<bool> onPauseToggle{ get; private set; }
    public UnityEvent<GenerationSummaryData> onGenerationSummary{ get; private set; }
    public UnityEvent onGenerationSummaryEnd{ get; private set; }

    [SerializeField] bool skipSummary = false;
    [SerializeField] GenerationSummaryUI generationSummaryUI;

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
        onPauseToggle = new UnityEvent<bool>();
        onGenerationSummary = new UnityEvent<GenerationSummaryData>();
        onGenerationSummaryEnd = new UnityEvent();
        gamePaused = false;
        timeScale = 1;
        generationSummary = false;
    }
    public void TogglePause()
    {
        if (generationSummary) return;
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
    public void StartGenerationSummary(GenerationSummaryData generationSummaryData)
    {
        Time.timeScale = 0;
        gamePaused = true;
        generationSummary = true;
        onPauseToggle?.Invoke(true);
        onGenerationSummary?.Invoke(generationSummaryData);
        if (skipSummary)
        {
            generationSummaryUI.StoptGenerationSummary();
        }
    }
    public void StopGenerationSummary()
    {
        Time.timeScale = timeScale;
        gamePaused = false;
        generationSummary = false;
        onGenerationSummaryEnd?.Invoke();
        onPauseToggle?.Invoke(false);
    }
}
