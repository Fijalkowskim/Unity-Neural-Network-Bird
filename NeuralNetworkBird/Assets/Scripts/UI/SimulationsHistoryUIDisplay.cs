using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimulationsHistoryUIDisplay : MonoBehaviour
{
    [SerializeField] SimulationsHistory simulationsHistory;
    [SerializeField] VerticalLayoutGroup layoutGroup;
    [SerializeField] SimulationCard simulationCardPrefab;
    [SerializeField] CanvasGroup panel;
    List<SimulationCard> simulationCards;
    [SerializeField] GeneticAlgorithm geneticAlgorithm;
    void Start()
    {
        ClearContent();
        SetContentHeight();
        DisablePanel();
        simulationsHistory.onSimulationAdded.AddListener(UpdateContent);
        GameManager.Instance.onPauseToggle.AddListener(OnPauseToggle);
        simulationCards = new List<SimulationCard>();
    }
    private void OnDestroy()
    {
        simulationsHistory.onSimulationAdded.RemoveListener(UpdateContent);
        GameManager.Instance.onPauseToggle.RemoveListener(OnPauseToggle);
    }

    void OnPauseToggle(bool paused)
    {
        if (paused) EnablePanel();
        else DisablePanel();
    }
    void DisablePanel()
    {
        panel.alpha = 0;
        panel.interactable = false;
    }
    void EnablePanel()
    {
        panel.alpha = 1;
        panel.interactable = true;
    }
    void UpdateContent(SimulationHistoryData data)
    {
        SimulationCard instance = Instantiate(simulationCardPrefab, layoutGroup.GetComponent<RectTransform>());
        instance.Init(data, geneticAlgorithm);
        simulationCards.Add(instance);
        SetContentHeight();
    }
    void SetContentHeight()
    {
        float totalHeight = CalculateTotalHeight();
        RectTransform containerRect = layoutGroup.GetComponent<RectTransform>();
        containerRect.sizeDelta = new Vector2(containerRect.sizeDelta.x, totalHeight);
    }

    float CalculateTotalHeight()
    {
        if (layoutGroup.transform.childCount == 0) return 0;
        float totalHeight = 0f;

        foreach (RectTransform child in layoutGroup.transform)
        {
            totalHeight += child.rect.height;
        }

        totalHeight += (layoutGroup.spacing * (layoutGroup.transform.childCount - 1));

        return totalHeight;
    }
    void ClearContent()
    {
        foreach (RectTransform child in layoutGroup.transform)
        {
            Destroy(child.gameObject);
        }
    }
    public void NewGeneration(int newGenerationIndex)
    {
        foreach (SimulationCard card in simulationCards)
        {
            if(card.generation != newGenerationIndex)
            {
                card.DisactivateButton();
            }
        }
    }
}
