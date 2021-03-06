﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemFunctions;
using GameDefinations;

public class RegisterBase : MonoBehaviour {

    GameDatabase gameDatabase;
    public GameObject gatherAxe;

    void Start () {
        gameDatabase = GameObject.FindGameObjectWithTag("ModLoader").GetComponent<GameDatabase>();

        RegisterResources();
	}

    void RegisterResources()
    {
        Item axe = new Item("axe_t_01", "Gather axe", 1);
        axe.functions.Add(new GatherTool());
        axe.modelInfo = new ModelInfo(gatherAxe, new Vector3(-0.542f, -0.054f, 0.026f), new Vector3(185, 186, -5.5f), Vector3.one);
        
        gameDatabase.RegisterItem(axe);
        gameDatabase.RegisterItem("Wood", "wd_t_04", 1000, null, null);
        gameDatabase.RegisterItem("HP POT", "az_bottle6", 10, null, new Usable(0,100));

        Stats[] all_stats = Utils.GetEnumArray<Stats>();

        for (int i = 0; i < all_stats.Length; i++)
        {
            gameDatabase.RegisterStat(all_stats[i].ToString());
        }
    }
}
