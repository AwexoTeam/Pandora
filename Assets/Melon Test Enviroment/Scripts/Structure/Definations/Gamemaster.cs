using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamemaster : MonoBehaviour {
    public FileDatabase fileDatabase;
    public GameDatabase gameDatabase;
    public UIHandler uiHandler;
    public GameObject player;
    public EntityController playerEntityController;
    public EffectManager effectManager;

    private void Start()
    {
        GameObject modLoader = GameObject.FindGameObjectWithTag("ModLoader");

        fileDatabase = modLoader.GetComponent<FileDatabase>();
        gameDatabase = modLoader.GetComponent<GameDatabase>();

        player = GameObject.FindGameObjectWithTag("Player");
        playerEntityController = player.GetComponent<EntityController>();
        uiHandler = player.GetComponent<UIHandler>();
        effectManager = modLoader.GetComponent<EffectManager>();
    }
}
