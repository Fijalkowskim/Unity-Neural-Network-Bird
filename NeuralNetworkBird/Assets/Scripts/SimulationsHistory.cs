using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SimulationsHistory : MonoBehaviour
{
    public List<SimulationHistoryData> simulationsHistory { get; private set; }
    public UnityEvent<SimulationHistoryData> onSimulationAdded { get; private set; }
    public void Initialize()
    {
        simulationsHistory = new List<SimulationHistoryData>();
    }
    public void addFinishedSimulation(SimulationHistoryData data)
    {
        simulationsHistory.Add(data);
        onSimulationAdded?.Invoke(data);
    }
}
