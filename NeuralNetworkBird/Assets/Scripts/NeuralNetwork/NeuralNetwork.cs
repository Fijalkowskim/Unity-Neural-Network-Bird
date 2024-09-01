using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MathNet.Numerics.LinearAlgebra;
using System;
[Serializable]
public class NeuralNetwork 
{
    public Matrix<float> inputLayer;
    public List<Matrix<float>> hiddenLayers;
    public Matrix<float> outputLayer;
    public List<Matrix<float>> weights { get; set; }
    public List<float> biases { get; set; }
    public float fitness { get; set; }
    public int generation { get; set; }
    public int index { get; set; }
    public string parentA { get; set; }
    public string parentB { get; set; }
    public int mutatedWeights { get; set; }
    public float previousFitness { get; set; }
    public void Init(int inputNeuronsCount, int generation, int index)
    {
        this.generation = generation; 
        this.index = index;
        mutatedWeights = 0;
        fitness = 0;

        inputLayer = Matrix<float>.Build.Dense(1, inputNeuronsCount);
        hiddenLayers = new List<Matrix<float>>();
        outputLayer = Matrix<float>.Build.Dense(1, 2);
        weights = new List<Matrix<float>>();
        biases = new List<float>();

        float hiddenLayersCount = inputNeuronsCount - 3;

        for (int i = 1; i < hiddenLayersCount + 1; i++)
        {
            int hiddenNeuronsCount = inputNeuronsCount - i;
            Matrix<float> f = Matrix<float>.Build.Dense(1, hiddenNeuronsCount);
            hiddenLayers.Add(f);
            biases.Add(UnityEngine.Random.Range(-0.1f, 0.1f));

            Matrix<float> weightsMatrix = Matrix<float>.Build.Dense(hiddenNeuronsCount + 1, hiddenNeuronsCount);
            weights.Add(weightsMatrix);
        }

        Matrix<float> OutputWeight = Matrix<float>.Build.Dense(3, 2);
        weights.Add(OutputWeight);
        biases.Add(UnityEngine.Random.Range(-0.1f, 0.1f));
    }

    public NeuralNetwork(int inputNeuronsCount, int generation, int index)
    {
        Init(inputNeuronsCount, generation, index);
        parentA = null;
        parentB = null;
        previousFitness = -1;

        RandomiseWeights();
    }
    public NeuralNetwork(NeuralNetwork neuralNetwork)
    {
        Init(neuralNetwork.inputLayer.ColumnCount, generation, index);
        parentA = neuralNetwork.parentA;
        parentB = neuralNetwork.parentB;
        fitness = neuralNetwork.fitness;

        mutatedWeights = neuralNetwork.mutatedWeights;
        previousFitness = neuralNetwork.fitness;

        weights = new List<Matrix<float>>(neuralNetwork.weights);
        biases = new List<float>(neuralNetwork.biases);  
    }
    public NeuralNetwork(NeuralNetwork parentA, NeuralNetwork parentB, int generation, int index)
    {
        Init(parentA.inputLayer.ColumnCount, generation, index);

        this.parentA = "Gen " + parentA.generation.ToString() + "/" + parentA.index.ToString();
        this.parentB = "Gen " + parentB.generation.ToString() + "/" + parentB.index.ToString();

        previousFitness = -1;
        
        for (int i = 0; i < weights.Count; i++)
        {
            weights[i] = (parentA.weights[i] + parentB.weights[i]) / 2f;
        }
        for (int i = 0; i < biases.Count; i++)
        {
            biases[i] = (parentA.biases[i] + parentB.biases[i]) / 2f;
        }
    }

    public void RandomiseWeights()
    {
        for (int i = 0; i < weights.Count; i++)
        {
            for (int x = 0; x < weights[i].RowCount; x++)
            {
                for (int y = 0; y < weights[i].ColumnCount; y++)
                {
                    weights[i][x, y] = UnityEngine.Random.Range(-1f, 1f);
                }

            }
        }
    }

    public (float, float) RunNeuralNetworkwork(List<float> sensorValues)
    {
        if (sensorValues.Count != inputLayer.ColumnCount) return (0, 0);
        for (int i = 0; i < sensorValues.Count; i++)
        {
            inputLayer[0, i] = sensorValues[i];
        }

        inputLayer = inputLayer.PointwiseTanh();
    

        for (int i = 0; i < hiddenLayers.Count; i++)
        {
            if (i == 0)
            {
                hiddenLayers[0] = ((inputLayer * weights[0]) + biases[0]).PointwiseTanh();
                hiddenLayers[0] = ((inputLayer * weights[0])).PointwiseTanh();
            }
            else
            {
                hiddenLayers[i] = ((hiddenLayers[i - 1] * weights[i]) + biases[i]).PointwiseTanh();
                hiddenLayers[i] = ((hiddenLayers[i - 1] * weights[i])).PointwiseTanh();
            }
        }

        outputLayer = ((hiddenLayers[hiddenLayers.Count - 1] * weights[weights.Count - 1])).PointwiseTanh();

        //First output is speed and second output is turn
        return (Sigmoid(outputLayer[0, 0]), (float)Math.Tanh(outputLayer[0, 1]));
    }

    private float Sigmoid(float s)
    {
        return (1 / (1 + Mathf.Exp(-s)));
    }
}

