﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using SIEvents;

public class StatusBars : MonoBehaviour
{
    [SerializeField]
    private Text healthBarText;
    [SerializeField]
    private Image healthBar;

    [SerializeField]
    private Text fuelBarText;
    [SerializeField]
    private Image fuelBar;

    void Awake()
    {
        UpdateAmounts();
        EventManager.Instance.OnHealthChange.AddListener(UpdateAmounts);
    }

    private void Update() {
        int count = DataTracker.Current.Player.Inventory.Contains("Fuel");
        fuelBarText.text = count.ToString("0");
        fuelBar.fillAmount =  count / 20.0f;
    }

    void UpdateAmounts(int i=0)
    {
        healthBarText.text = DataTracker.Current.Player.health.ToString("0");
        healthBar.fillAmount = DataTracker.Current.Player.health / DataTracker.Current.Player.healthCap;


    }
}