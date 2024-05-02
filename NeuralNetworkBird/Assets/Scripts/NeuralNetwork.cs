using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MathNet.Numerics.LinearAlgebra;
using System;
public class NeuralNetwork : MonoBehaviour
{
    Matrix<float> inputLayer;
    List<Matrix<float>> hiddenLayers;
    Matrix<float> outputLayer;
    List<Matrix<float>> weights;
    List<float> biases;
    public void Initialize(int inputNeuronsCount, int hiddenLayersCount, int hiddenNeuronsCount)
    {
        inputLayer = Matrix<float>.Build.Dense(1, inputNeuronsCount);
        hiddenLayers = new List<Matrix<float>>();
        outputLayer = Matrix<float>.Build.Dense(1, 2);
        weights = new List<Matrix<float>>();
        biases = new List<float>();

        for (int i = 0; i < hiddenLayersCount + 1; i++)
        {
            Matrix<float> f = Matrix<float>.Build.Dense(1, hiddenNeuronsCount);
            hiddenLayers.Add(f);
            biases.Add(UnityEngine.Random.Range(-1f, 1f));

            if (i == 0)
            {
                Matrix<float> inputToH1 = Matrix<float>.Build.Dense(inputNeuronsCount, hiddenNeuronsCount);
                weights.Add(inputToH1);
            }
            Matrix<float> HiddenToHidden = Matrix<float>.Build.Dense(hiddenNeuronsCount, hiddenNeuronsCount);
            weights.Add(HiddenToHidden);
        }

        Matrix<float> OutputWeight = Matrix<float>.Build.Dense(hiddenNeuronsCount, 2);
        weights.Add(OutputWeight);
        biases.Add(UnityEngine.Random.Range(-1f, 1f));

        RandomiseWeights();
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

    public (float, float) RunNetwork(List<float> sensorValues)
    {
        if (sensorValues.Count != inputLayer.ColumnCount) return (0, 0);
        for (int i = 0; i < sensorValues.Count; i++)
        {
            inputLayer[0, i] = sensorValues[i];
        }

        inputLayer = inputLayer.PointwiseTanh();

        hiddenLayers[0] = ((inputLayer * weights[0]) + biases[0]).PointwiseTanh();

        for (int i = 1; i < hiddenLayers.Count; i++)
        {
            hiddenLayers[i] = ((hiddenLayers[i - 1] * weights[i]) + biases[i]).PointwiseTanh();
        }

        outputLayer = ((hiddenLayers[hiddenLayers.Count - 1] * weights[weights.Count - 1]) + biases[biases.Count - 1]).PointwiseTanh();

        //First output is speed and second output is turn
        return (Sigmoid(outputLayer[0, 0]), (float)Math.Tanh(outputLayer[0, 1]));
    }

    private float Sigmoid(float s)
    {
        return (1 / (1 + Mathf.Exp(-s)));
    }
}

