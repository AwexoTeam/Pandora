using Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct StateEffect
{
    public int stateIndex;
    public int ticks;
    public int value;
}

public class Status : MonoBehaviour {
    private GameDatabase gameDatabase;
    private FileDatabase fileDatabase;

    public float[] currStat;
    public float[] baseStat;
    public bool[] states;

    public float hp;

    private void Start()
    {
        GameObject modLoader = GameObject.FindGameObjectWithTag("ModLoader");
        gameDatabase = modLoader.GetComponent<GameDatabase>();
        fileDatabase = modLoader.GetComponent<FileDatabase>();

        currStat = new float[gameDatabase.stats];
        baseStat = new float[gameDatabase.stats];
        states = new bool[gameDatabase.states];
    }

    private void Update()
    {
        hp = currStat[0];
    }

    public void AddToCurrStat(string name, float val) { AddToCurrStat(gameDatabase.GetStatIndexByName(name), val); }
    public void AddToCurrStat(int index, float val)
    {
        if(index > -1)
        {
            currStat[index] += val;
        }
    }
    
    public void AddToBaseStat(string name, float val) { AddToBaseStat(gameDatabase.GetStatIndexByName(name), val); }
    public void AddToBaseStat(int index, float val)
    {
        if(index > -1)
        {
            baseStat[index] += val;
        }
    }
    
    public bool GetState(string name)
    {
        bool rtn = false;
        rtn = GetState(gameDatabase.GetStateIndexByName(name));

        return rtn;
    }
    public bool GetState(int index) { return states[index]; }
    public void SetState(string name, bool val) { SetState(gameDatabase.GetStateIndexByName(name), val); }
    public void SetState(int index, bool val)
    {
        if (index > -1)
        {
            states[index] = val;
        }
    }
    public void FlipState(string name) { FlipState(gameDatabase.GetStateIndexByName(name)); }
    public void FlipState(int index) { states[index] = !states[index]; }
}
