using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SensorUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI sensorNumber;
    [SerializeField] TextMeshProUGUI sensorValue;
    [SerializeField] TextMeshProUGUI speedValue;
    [SerializeField] TextMeshProUGUI turnValue;
    
    public void UpdateUI(int sensorNumber, float sensorValue,float speedValue,float turnValue)
    {
        this.sensorNumber.text = "Sensor " + sensorNumber.ToString();
        this.sensorValue.text = "Value " + sensorValue.ToString("F2");
        this.speedValue.text = "Speed " + speedValue.ToString("F2");
        this.turnValue.text = "Turn " + turnValue.ToString("F2");
    }
}
