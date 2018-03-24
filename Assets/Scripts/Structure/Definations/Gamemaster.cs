using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamemaster : MonoBehaviour {
    public FileDatabase fileDatabase;
    public GameDatabase gameDatabase;
    public GameObject player;
    public EntityController playerEntityController;

    private void Start()
    {
        GameObject modLoader = GameObject.FindGameObjectWithTag("ModLoader");

        fileDatabase = modLoader.GetComponent<FileDatabase>();
        gameDatabase = modLoader.GetComponent<GameDatabase>();

        player = GameObject.FindGameObjectWithTag("Player");
        playerEntityController = player.GetComponent<EntityController>();
    }
}
