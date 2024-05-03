using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class UIStatsDislay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI fitnessText, distanceText, timeText, avgSpeedText, populationText, simulationText, timeScale;
    [SerializeField] BirdController birdController;
    [SerializeField] GeneticAlgorithm geneticAlgorithm;
    [SerializeField] Slider timeScaleSlider;
    void Awake()
    {
        fitnessText.text = 0.ToString();
        distanceText.text = 0.ToString();
        timeText.text = 0.ToString();
        avgSpeedText.text = 0.ToString();
        timeScale.text = 1.ToString();
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
        timeScale.text = Time.timeScale.ToString("F0");
    }
    public void onTimeScaleSliderChange(float newValue)
    {
        Time.timeScale = newValue;
    }
    public void SetGenerationStats(int currentPopulation, int currentSimulation, int totalSimulations)
    {
        populationText.text = "Population " + currentPopulation.ToString();
        simulationText.text = "Simulation " + currentSimulation.ToString() + "/" + totalSimulations.ToString();
    }
}
