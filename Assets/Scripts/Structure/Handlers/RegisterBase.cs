using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemFunctions;

public class RegisterBase : MonoBehaviour {

    GameDatabase gameDatabase;

    void Start () {
        gameDatabase = GameObject.FindGameObjectWithTag("ModLoader").GetComponent<GameDatabase>();

        RegisterResources();
	}

    void RegisterResources()
    {
        gameDatabase.RegisterItem("Gather axe", "axe_t_01", 1, null, new GatherTool());
        gameDatabase.RegisterItem("Wood", "wd_t_04", 1000, null, null);
    }
	
}
