using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
public class FileManager : MonoBehaviour
{
    public static FileManager Instance { get; private set; }
    const string path = "/Saved-Neural-Networks";
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            Init();
        }
    }
    void Init()
    {
    }
    public void SaveNetworkToJSON(NeuralNetwork nnet)
    {
        string newPath = Application.dataPath + path;
        if (!Directory.Exists(newPath))
        {
            Directory.CreateDirectory(newPath);  
        }
        string ID = DateTime.Now.Ticks.ToString();
        string fileName = "/nnet" + nnet.generation + "-" + nnet.index + "-" + ID + ".txt";

        Debug.Log("Neural network saved as: " + fileName);

        string json = JsonConvert.SerializeObject(nnet);
        File.WriteAllText(newPath + fileName, json);
    }
}
