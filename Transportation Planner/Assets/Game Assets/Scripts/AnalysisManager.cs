using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnalysisManager : MonoBehaviour
{

    [ReadOnly] public List<CarSpawner> carSpawners;
    [ReadOnly] public List<BusSpawner> busSpawners;

    public Transform carSpawnerParent;
    public Transform busSpawnerParent;

    [HorizontalLine]
    [ReadOnly] public float efficiencyTimer;
    [ReadOnly] public int totalCurrCar;
    [ReadOnly] public int totalMaxCar;
    [ReadOnly] public int totalCurrBus;
    [ReadOnly] public int totalMaxBus;
    public float efficiencyCheckInterval;
    public TMP_Text carCountText;
    public TMP_Text busCountText;
    public TMP_Text efficiencyText;

    [HorizontalLine]
    public Image speedButton;
    public Sprite normalSpeedIcon;
    public Sprite fastSpeedIcon;
    public TMP_Text speedText;
    public int speedMultiplier = 2;
    bool fast;

    string timerText;
    int efficiencyPercentageInt;

    void Start()
    {
        efficiencyTimer = efficiencyCheckInterval;
    }

    void Update()
    {
        CalculateEfficiency();
        PopulateSpawnerList();

        totalCurrCar = carSpawners.Sum(carSpawner => carSpawner.currCars);
        totalMaxCar = carSpawners.Sum(carSpawner => carSpawner.maxCars);
        totalCurrBus = busSpawners.Sum(busSpawner => busSpawner.currBuses);
        totalMaxBus = busSpawners.Sum(busSpawner => busSpawner.maxBuses);

        carCountText.text = "Total Cars:\n" + totalCurrCar + "/" + totalMaxCar;
        busCountText.text = "Total Buses:\n" + totalCurrBus + "/" + totalMaxBus;
    }

    public void ToggleTimeScale()
    {
        if(!fast)
        {
            speedButton.sprite = fastSpeedIcon;
            speedText.text = "x" + speedMultiplier;

            Time.timeScale = speedMultiplier;
        } else 
        {
            speedButton.sprite = normalSpeedIcon;
            speedText.text = "x1";

            Time.timeScale = 1;
        }
        fast = !fast;
    }

    void PopulateSpawnerList()
    {
        if (carSpawners.Count != carSpawnerParent.childCount - 1) // Subtract 1 to exclude the parent object
        {
            carSpawners = carSpawnerParent.GetComponentsInChildren<CarSpawner>()
                .ToList();
        }

        if (busSpawners.Count != busSpawnerParent.childCount)
        {
            busSpawners = busSpawnerParent.GetComponentsInChildren<BusSpawner>()
                .ToList();
        }
    }

    void CalculateEfficiency()
    {
        efficiencyTimer -= Time.deltaTime;

        TimeSpan t = TimeSpan.FromSeconds(efficiencyTimer);
        if (t.Seconds < 10)
        {
            timerText = t.Minutes + ":0" + t.Seconds;
        }
        else
        {
            timerText = t.Minutes + ":" + t.Seconds;
        }

        if (efficiencyTimer < 0)
        {
            efficiencyTimer = efficiencyCheckInterval;
            float efficiencyPercentage = (1f - (float)totalCurrCar / totalMaxCar) * 100f;
            efficiencyPercentageInt = Mathf.RoundToInt(efficiencyPercentage);
        }
        efficiencyText.text = "Average Efficiency:\n" + efficiencyPercentageInt + "%\n" + timerText + " left";
    }

}
