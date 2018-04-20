using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightScript : MonoBehaviour {
    float startUpTime = 0;
    float intensity = 0;
    Gamemaster gm;
    Light l;
    bool isOn;

    float timeStamp;

    private void Start()
    {
        startUpTime = Random.Range(0, 200) / 100;
        gm = GameObject.FindGameObjectWithTag("ModLoader").GetComponent<Gamemaster>();
        l = GetComponent<Light>();
        if(l == null) { Destroy(this); }
        intensity = l.intensity;

        timeStamp = Time.time + startUpTime;
    }

    private void Update()
    {
        float hour = gm.effectManager.currTime.Military_Hour;
        if (hour >= 0 && hour <= 8) { TurnOn(); }
        else if (hour > 5 && hour <= 18) { TurnOff(); }
        else { TurnOn(); }
    }

    private void TurnOn()
    {
        if(isOn == false)
        {
            isOn = true;
            timeStamp = Time.time + startUpTime;
        }

        if(Time.time >= timeStamp && isOn)
        {
            l.intensity = intensity;
        }
    }

    private void TurnOff()
    {
        if (isOn)
        {
            isOn = false;
            timeStamp = Time.time + startUpTime;
        }

        if(Time.time >= timeStamp && isOn == false)
        {
            l.intensity = 0;
        }
    }
}
