using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
public class SimulationCard : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI dataText;
    [SerializeField] Button saveButton;
    UnityEvent<int, int> onSaveButtonClick;
    
    public int generation { get; private set; }
    public int index { get; private set; }
    public void Init(SimulationHistoryData data, GeneticAlgorithm geneticAlgorithm)
    {
        dataText.text = $"Gen {data.generation} | Sim {data.index} | Fitness {data.nnet.fitness}";
        this.generation = data.generation;
        this.index = data.index;
        onSaveButtonClick = new UnityEvent<int, int>();
        onSaveButtonClick.AddListener(geneticAlgorithm.SaveNetworkToFile);
    }
    public void SaveToFileClick()
    {
        onSaveButtonClick?.Invoke(index, generation);
    }
    public void DisactivateButton()
    {
        saveButton.gameObject.SetActive(false);
    }
}
