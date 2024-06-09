using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class GenerationSummaryData
{
    public float avgFitness;
    public float fitnessComparePercent;

    public GenerationSummaryData(float avgFitness, GenerationSummaryData lastData)
    {
        this.avgFitness = avgFitness;
        if(lastData == null || lastData.avgFitness == 0)
        {
            fitnessComparePercent = 100;
        }
        else
        {
           fitnessComparePercent = (avgFitness / lastData.avgFitness - 1) * 100; 
        }
    }
}
