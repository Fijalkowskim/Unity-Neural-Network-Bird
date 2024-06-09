using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
public class FileManager : MonoBehaviour
{
    public static FileManager Instance { get; private set; }
    const string path = "/saved";
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
        string newPath = Application.dataPath + path + "/SavedBrains";
        if (!System.IO.Directory.Exists(newPath))
        {
            Directory.CreateDirectory(newPath);
           
        }
        Debug.Log("Neural network saved to: " + new DirectoryInfo(newPath).FullName);

        string ID = DateTime.Now.Ticks.ToString();
        string json = JsonConvert.SerializeObject(nnet);
        File.WriteAllText(newPath + "/nnet-" + ID + ".txt", json);
        NeuralNetwork newNew = (NeuralNetwork) JsonConvert.DeserializeObject(json);
        /*string json = JsonConvert.SerializeObject(nnet.inputLayer.ToArray());

        File.WriteAllText(newPath + "/inputLayer_" + ID + ".txt", json);
        json = JsonConvert.SerializeObject(nnet.hiddenLayers.ToArray());
        File.WriteAllText(newPath + "/hiddenLayers_" + ID + ".txt", json);
        json = JsonConvert.SerializeObject(nnet.outputLayer.ToArray());
        File.WriteAllText(newPath + "/outputLayers_" + ID + ".txt", json);
        json = JsonConvert.SerializeObject(nnet.weights.ToArray());
        File.WriteAllText(newPath + "/weights_" + ID + ".txt", json);
        json = JsonConvert.SerializeObject(nnet.biases.ToArray());
        File.WriteAllText(newPath + "/biases_" + ID + ".txt", json);*/
    }
}
