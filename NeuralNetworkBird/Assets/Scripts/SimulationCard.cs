using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SimulationCard : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI dataText;
    public void Init(SimulationHistoryData data)
    {
        dataText.text = $"Generation {data.generation} | Simulation {data.index} | Fitness {data.nnet.fitness}";
    }
}
