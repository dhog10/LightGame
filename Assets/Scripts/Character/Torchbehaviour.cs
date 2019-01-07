﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torchbehaviour : MonoBehaviour {


    //LightIntensity value between 0 and 8.
    public float maxLightIntensity;
    public float currentLightIntenisity;
    public float batteryDrain;
    public int maxBatteries;
    public int batteryCount;
    public int batteryPercent;
    Light light;


	// Use this for initialization
	void Start () {
        light = GetComponent<Light>();
	}

    void replacebattery()
    {
        if (batteryCount > 0) {
            if (Input.GetKeyDown(KeyCode.T))
            {
                currentLightIntenisity = maxLightIntensity;
                batteryCount--;
            }
        }
    }

    public int GetBatteryCount()
    {
        return batteryCount;
    }

    public int GetMaxBatteries()
    {
        return maxBatteries;
    }

    public int GetBatteryLife()
    {
        batteryPercent = (int)((100.0f / maxLightIntensity) * currentLightIntenisity);
        return batteryPercent;
    }
	// Update is called once per frame
	void Update () {
        replacebattery();
        currentLightIntenisity -= batteryDrain;
        light.intensity = currentLightIntenisity;
	}
}
