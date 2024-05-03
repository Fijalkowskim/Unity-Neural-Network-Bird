using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationsHistory : MonoBehaviour
{
    public List<NeuralNetwork> simulationsHistory { get; private set; }
    public void Initialize()
    {
        simulationsHistory = new List<NeuralNetwork>();
    }
    public void addFinishedSimulation(NeuralNetwork nnet)
    {
        simulationsHistory.Add(new NeuralNetwork(nnet));
    }
}
