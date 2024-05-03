using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MathNet.Numerics.LinearAlgebra;
using System;

public class GeneticAlgorithm : MonoBehaviour
{
    [SerializeField] BirdController controller;
    [SerializeField] UIStatsDislay uIStatsDislay;

    [Header("Controls")]
    [Range(1,100)]
    [SerializeField] int hiddenLayers = 2;
    [Range(3, 200)]
    [SerializeField] int hiddenNeurons = 10;
    [SerializeField] int totalSimulations = 85;
    
    [Header("Mutation Controls")]
    [Tooltip("Percent to mutate each newly created Neural Network. Mutation takes randomly selected weights and adds or subtracts small amount from it.")]
    [Range(0.0f, 1.0f)]
    [SerializeField] float mutationRate = 0.05f;
    [Tooltip("Maximum value to add/subtract from mutated weight.")]
    [Range(0.0f, 1.0f)]
    [SerializeField] float maxMutationChange = 1f;

    [Header("New Generation Controls")]
    [Tooltip("Percent of best simulations picked from previous generation that will be moved to next generation")]
    [Range(0.0f, 1.0f)]
    [SerializeField] float eliteSelectionRate = 0.3f;
    [Tooltip("Percent of best simulations from previous generation that will be pick as crossover parents")]
    [Range(0.0f, 1.0f)]
    [SerializeField] float crossoverParentSelectionRate = 0.4f;
    [Tooltip("Percent of new generation that will be children of crossovers from previous generation")]
    [Range(0.0f, 1.0f)]
    [SerializeField] float percentOfCrossovers = 0.5f;

    [SerializeField] NeuralNetwork[] generation;

    int currentGeneration, currentSimulation;
    int amountOfEliteSelection, amountOfCossovers, amountOfCrossoverParents;
    [Tooltip("Number of already created simulations for new generation")]
    int newSimulationsCounter;
    List<int> crossoverParentsIndexes;

    void Awake()
    {
        ResetAll();
    }
    public void ResetAll()
    {
        if (totalSimulations <= 0) return;
        amountOfEliteSelection = (int)(totalSimulations * eliteSelectionRate);
        amountOfCossovers = Mathf.Clamp((int)(totalSimulations * percentOfCrossovers), 0, totalSimulations - amountOfEliteSelection);
        amountOfCrossoverParents = (int)(totalSimulations * crossoverParentSelectionRate);
        crossoverParentsIndexes = new List<int>();
        for (int i = 0; i < amountOfCrossoverParents; i++)
        {
            crossoverParentsIndexes.Add(i);
        }
        ResetGenerations();
    }
    void ResetGenerations()
    {
        currentGeneration = 0;
        currentSimulation = 0; 
        newSimulationsCounter = 0;
        generation = new NeuralNetwork[totalSimulations];
        for (int i = 0; i < totalSimulations; i++)
        {
            generation[i] = new NeuralNetwork(controller.numberOfSensors, hiddenLayers, hiddenNeurons, currentGeneration, i);
        }
        ResetControllerWithNewSimulation();
    }

    private void ResetControllerWithNewSimulation()
    {
        uIStatsDislay.SetSimulationStats(generation[currentSimulation], currentGeneration, currentSimulation, totalSimulations);
        controller.ResetWithNetwork(generation[currentSimulation]);
    }

    public void Death(float fitness)
    {
        if (currentSimulation < generation.Length - 1)
        {
            generation[currentSimulation].fitness = fitness;
            currentSimulation++;
            ResetControllerWithNewSimulation();
        }
        else
            CreateNewGeneration();
    }


    private void CreateNewGeneration()
    {
        SortPreviousGeneration();

        NeuralNetwork[] newGeneration = new NeuralNetwork[totalSimulations];
        newSimulationsCounter = 0;

        currentGeneration++;
        currentSimulation = 0;

        FillWithBestSimulations(ref newGeneration);
        FillWithCrossovers(ref newGeneration);
        FillWithRandomSimulations(ref newGeneration);

        generation = newGeneration;


        ResetControllerWithNewSimulation();
    }



    void FillWithBestSimulations(ref NeuralNetwork[] newGeneration)
    {
        for (int i = 0; i < amountOfEliteSelection; i++)
        {
            newGeneration[newSimulationsCounter] = new NeuralNetwork(generation[i]);
            newSimulationsCounter++;
        }
    }
    private void FillWithCrossovers(ref NeuralNetwork[] newGeneration)
    {
        if (amountOfCossovers <= 0 || amountOfCrossoverParents < 2) return;
        
        int AIndex, BIndex, randAIndex;

        for (int i = amountOfEliteSelection; i < amountOfEliteSelection + amountOfCossovers; i++)
        {
            List<int> tmpCrossoverParentsIndexes = new List<int>(crossoverParentsIndexes);

            randAIndex = UnityEngine.Random.Range(0, tmpCrossoverParentsIndexes.Count);
            AIndex = tmpCrossoverParentsIndexes[randAIndex];
            tmpCrossoverParentsIndexes.RemoveAt(randAIndex);
            BIndex = tmpCrossoverParentsIndexes[UnityEngine.Random.Range(0, tmpCrossoverParentsIndexes.Count)];

            NeuralNetwork child = new NeuralNetwork(generation[AIndex], generation[BIndex], currentGeneration, newSimulationsCounter);

            if (UnityEngine.Random.Range(0.0f, 1.0f) < mutationRate) 
                Mutate(ref child);

            newGeneration[newSimulationsCounter] = child;

            newSimulationsCounter++;
        }
    }
    void FillWithRandomSimulations(ref NeuralNetwork[] newGeneration)
    {
        int startIndex = newSimulationsCounter;
        for (int i = startIndex; i < newGeneration.Length; i++)
        {
            newGeneration[newSimulationsCounter] = new NeuralNetwork(controller.numberOfSensors, hiddenLayers, hiddenNeurons,currentGeneration, newSimulationsCounter);
            newSimulationsCounter++;
        }
    }
    private void Mutate(ref NeuralNetwork nnet)
    {
        int totalMutatedWeights = 0;
        for (int weight = 0; weight < nnet.weights.Count; weight++)
        {
            (Matrix<float> newWeights, int mutatedWeights) = MutateMatrix(nnet.weights[weight]);
            nnet.weights[weight] = newWeights;
            totalMutatedWeights += mutatedWeights;
        }
        nnet.mutatedWeights += totalMutatedWeights;
    }
    (Matrix<float>, int) MutateMatrix(Matrix<float> matrix)
    {
        int amountToMutate = UnityEngine.Random.Range(1, matrix.RowCount * matrix.ColumnCount);
        Matrix<float> mutatedMatrix = matrix;
        for (int i = 0; i < amountToMutate; i++)
        {
            int randomColumn = UnityEngine.Random.Range(0, mutatedMatrix.ColumnCount);
            int randomRow = UnityEngine.Random.Range(0, mutatedMatrix.RowCount);
            mutatedMatrix[randomRow, randomColumn] = Mathf.Clamp(mutatedMatrix[randomRow, randomColumn] + UnityEngine.Random.Range(-maxMutationChange, maxMutationChange), -1f, 1f);
        }
        return (mutatedMatrix, amountToMutate);
    }
    private void SortPreviousGeneration()
    {
        Array.Sort(generation, (nn1, nn2) => nn2.fitness.CompareTo(nn1.fitness));
    }
}
