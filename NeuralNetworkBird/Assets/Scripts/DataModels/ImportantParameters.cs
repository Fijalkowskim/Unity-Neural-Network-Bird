using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ImportantParameters
{
    public int numberOfSensors;
    public int fieldOfView;
    public float sensorDistance;
    public float totalSimulations;
    public float mutationRate;
    public float eliteSelection;
    public float percentOfCrossovers;
    public ImportantParameters() { }
    public override string ToString()
    {
        return $"Parameters:\n" +
               $"Number of Sensors: {numberOfSensors}\n" +
               $"Field of View: {fieldOfView}\n" +
               $"Sensor Distance: {sensorDistance}\n" +
               $"Total Simulations: {totalSimulations}\n" +
               $"Mutation Rate: {mutationRate}\n" +
               $"Elite Selection: {eliteSelection}\n" +
               $"Percent of Crossovers: {percentOfCrossovers}\n";
    }
}
