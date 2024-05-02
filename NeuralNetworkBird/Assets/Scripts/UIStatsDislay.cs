using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UIStatsDislay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI fitnessText, distanceText, timeText, avgSpeedText;
    [SerializeField] BirdController birdController;
    void Awake()
    {
        fitnessText.text = 0.ToString();
        distanceText.text = 0.ToString();
        timeText.text = 0.ToString();
        avgSpeedText.text = 0.ToString();
    }

 
    void LateUpdate()
    {
        if (birdController == null) return;
        fitnessText.text = birdController.fitness.ToString("F2");
        distanceText.text = birdController.totalDistance.ToString("F2");
        timeText.text = birdController.timeSinceStart.ToString("F2");
        avgSpeedText.text = birdController.avgSpeed.ToString("F2");
    }
}
