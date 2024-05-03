using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationHistoryData
{
    public int generation;
    public int index;
    public NeuralNetwork nnet;

    public SimulationHistoryData(int generation, int index, NeuralNetwork nnet)
    {
        this.generation = generation;
        this.index = index;
        this.nnet = new NeuralNetwork(nnet);
    }
}
