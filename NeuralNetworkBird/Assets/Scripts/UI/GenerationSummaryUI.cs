using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GenerationSummaryUI : MonoBehaviour
{
    [SerializeField] GameObject wrapperUi;
    [SerializeField] TextMeshProUGUI avgFitness, comparedToPrev;
    private void Start()
    {
        wrapperUi.SetActive(false);
    }
    private void OnEnable()
    {
        GameManager.Instance.onGenerationSummary.AddListener(StartGenerationSummary);
    }
    private void OnDisable()
    {
        GameManager.Instance.onGenerationSummary.RemoveListener(StartGenerationSummary);
    }
    public void StartGenerationSummary(GenerationSummaryData data)
    {
        avgFitness.text = data.avgFitness.ToString("F2");
        comparedToPrev.text = data.fitnessComparePercent >= 0 ? "+" : "";
        comparedToPrev.text += data.fitnessComparePercent.ToString("F2") + "%";
        wrapperUi.SetActive(true);
    }
    public void StoptGenerationSummary()
    {
        wrapperUi.SetActive(false);
        GameManager.Instance.StopGenerationSummary();
    }
}
