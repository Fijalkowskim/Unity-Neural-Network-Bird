using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
public class SensorAnalizer : MonoBehaviour
{
    public int currentSensor { get; set; }
    public float[] sensorValues { get; private set; }
    public float speedValue { get; private set; }
    public float turnValue { get; private set; }
    public float currentSensorValue { get; private set; }
    [SerializeField] bool saveToFile = true;
    [SerializeField] SensorUI sensorUI;
    [SerializeField] bool drawRay;
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] Color hitColor, notHitColor;

    List<float> expectedTurnValues;
    List<float> realTurnValues;
    [SerializeField] int ticksPerSample = 10;
    float sampleCounter;
    const string path = "/Expected-Turn";
    private void Awake()
    {
        currentSensor = 0;
        SetupParams();
    }
    void SetupParams()
    {
        sampleCounter = 0;
        expectedTurnValues = new List<float>();
        realTurnValues = new List<float>();
    }
    public void AnalizeSensors(float[] sensorValues,float speedValue,float turnValue)
    {
        this.sensorValues = sensorValues;
        this.speedValue = speedValue;
        this.turnValue = turnValue;
        currentSensorValue = sensorValues[currentSensor];
        sensorUI.UpdateUI(currentSensor, currentSensorValue, speedValue, turnValue);
        sampleCounter++;
        if(sampleCounter >= ticksPerSample)
        {
            sampleCounter = 0;
            int sensorsToDivide = (sensorValues.Length - 1) / 2;
            float expectedValue = 0f;
            for (int i = 0; i < sensorsToDivide; i++)
            {
                expectedValue += sensorValues[i] * (1 - sensorsToDivide * i); //Left most
                expectedValue += sensorValues[sensorValues.Length - 1 - i] * (-1 + sensorsToDivide * i); //Right most
            }
            expectedValue /= (sensorValues.Length - 1);
            expectedTurnValues.Add(expectedValue);
            realTurnValues.Add(turnValue);
        }
    }
    public void UpdateSensorValue(Vector3 sensorDir, float sensorLength, bool didHit)
    {
        lineRenderer.enabled = drawRay;
        if (drawRay)
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, transform.position + sensorDir.normalized*sensorLength);

            lineRenderer.SetColors(didHit ? hitColor : notHitColor, didHit ? hitColor : notHitColor);
        }
    }
    public void onBirdDeath(float fitness)
    {
        SaveSensorToFile(fitness);
        SetupParams();
    }
    void SaveSensorToFile(float fitness)
    {
        if (!saveToFile) return;
        string newPath = Application.dataPath + path;
        if (!Directory.Exists(newPath))
        {
            Directory.CreateDirectory(newPath);
        }
        string ID = DateTime.Now.Ticks.ToString();
        string fileName = "/expectedTurn" + ID + "-fitness-"+ fitness.ToString() +".txt";

        string content = "expectedTurn,realTurn";
        for (int i = 0; i < expectedTurnValues.Count; i++)
        {
            content += $"\n{expectedTurnValues[i]},{realTurnValues[i]}";
        }

        File.WriteAllText(newPath + fileName, content);
    }

}
