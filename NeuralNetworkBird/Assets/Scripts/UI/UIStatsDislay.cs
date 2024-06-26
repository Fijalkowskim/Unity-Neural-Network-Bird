using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class UIStatsDislay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI fitnessText, distanceText, timeText, avgSpeedText, populationText, simulationText, timeScaleText, pauseButtonText;
    [SerializeField] GameObject statsWrapper;
    [SerializeField] BirdController birdController;
    [SerializeField] GeneticAlgorithm geneticAlgorithm;
    [SerializeField] Slider timeScaleSlider;
    [Header("Simulations stats")]
    [SerializeField] TextMeshProUGUI simulationGenerationIndex;
    [SerializeField] TextMeshProUGUI simulationIndex;
    [SerializeField] TextMeshProUGUI simulationParents;
    [SerializeField] TextMeshProUGUI simulationMutatedWeigths;
    [SerializeField] TextMeshProUGUI eliteText;
    void Start()
    {
        fitnessText.text = 0.ToString();
        distanceText.text = 0.ToString();
        timeText.text = 0.ToString();
        avgSpeedText.text = 0.ToString();
        timeScaleText.text = 1.ToString();
        pauseButtonText.text = "Pause";

        timeScaleSlider.value = 0;
        timeScaleSlider.onValueChanged.AddListener(onTimeScaleSliderChange);
        eliteText.gameObject.SetActive(false);
        GameManager.Instance.onPauseToggle.AddListener(OnPauseToggle);
    }
            
    private void OnDestroy()
    {
        timeScaleSlider.onValueChanged.RemoveListener(onTimeScaleSliderChange);
        GameManager.Instance.onPauseToggle.RemoveListener(OnPauseToggle);
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
        if (GameManager.Instance.gamePaused) return;
        GameManager.Instance.setTimeScale(newValue);
        timeScaleText.text = newValue.ToString("F0");
    }
    public void SetSimulationStats(NeuralNetwork nnet, int currentGeneration, int currentSimulation, int totalSimulations)
    {
        populationText.text = "Generation " + currentGeneration.ToString();
        simulationText.text = "Simulation " + currentSimulation.ToString() + "/" + totalSimulations.ToString();

        simulationGenerationIndex.text = $"Generation: {nnet.generation}";
        simulationIndex.text = $"Index: {nnet.index}";
        simulationParents.text = $"Parents: {(nnet.parentA == null ? "-":nnet.parentA + " + " + nnet.parentB)}";
        simulationMutatedWeigths.text = $"Mutated weights: {nnet.mutatedWeights}";

        if (nnet.previousFitness >= 0)
        {
            eliteText.text = $"Elite {nnet.previousFitness.ToString("F2")} fitness";
            eliteText.gameObject.SetActive(true);
        }
        else
            eliteText.gameObject.SetActive(false);
    }
    public void TogglePause()
    {
        GameManager.Instance.TogglePause();
    }
    void OnPauseToggle(bool paused)
    {
        pauseButtonText.text = paused ? "Resume" : "Pause";
        timeScaleSlider.enabled = !paused;
    }
}
