using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class UIStatsDislay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI fitnessText, distanceText, timeText, avgSpeedText, populationText, simulationText, timeScaleText, pauseButtonText;
    [SerializeField] BirdController birdController;
    [SerializeField] GeneticAlgorithm geneticAlgorithm;
    [SerializeField] Slider timeScaleSlider;

    float lastTimeScale; 
    void Awake()
    {
        fitnessText.text = 0.ToString();
        distanceText.text = 0.ToString();
        timeText.text = 0.ToString();
        avgSpeedText.text = 0.ToString();
        timeScaleText.text = 1.ToString();
        pauseButtonText.text = "Pause";

        timeScaleSlider.value = 0;
        timeScaleSlider.onValueChanged.AddListener(onTimeScaleSliderChange);
    }
    private void OnDestroy()
    {
        timeScaleSlider.onValueChanged.RemoveListener(onTimeScaleSliderChange);
    }

    void LateUpdate()
    {
        if (birdController == null || geneticAlgorithm == null) return;
        fitnessText.text = birdController.fitness.ToString("F2");
        distanceText.text = birdController.totalDistance.ToString("F2");
        timeText.text = birdController.timeSinceStart.ToString("F2");
        avgSpeedText.text = birdController.avgSpeed.ToString("F2");
    }
    public void onTimeScaleSliderChange(float newValue)
    {
        if (Time.timeScale == 0) return;
        Time.timeScale = newValue;
        timeScaleText.text = newValue.ToString("F0");
    }
    public void SetSimulationStats(NeuralNetwork nnet, int currentGeneration, int currentSimulation, int totalSimulations)
    {
        populationText.text = "Generation " + currentGeneration.ToString();
        simulationText.text = "Simulation " + currentSimulation.ToString() + "/" + totalSimulations.ToString();
    }
    public void TogglePause()
    {
        if(Time.timeScale != 0)
        {
            lastTimeScale = Time.timeScale;
            Time.timeScale = 0;
            pauseButtonText.text = "Resume";
            timeScaleSlider.enabled = false;
        }
        else
        {
            Time.timeScale = lastTimeScale;
            pauseButtonText.text = "Pause";
            timeScaleSlider.enabled = true;
        }
    }
}
