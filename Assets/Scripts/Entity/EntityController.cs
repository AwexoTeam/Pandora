using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EntityTag
{
    Player,
    Enemy,
    NPC,
    Entity,
}

public class EntityController : MonoBehaviour {

    public EntityTag tag;
    public Status status;
    public InventoryHandler inventory;
    public Gamemaster gm;
    public AnimationHandler animationHandler;

    public void Awake()
    {
        status = gameObject.GetComponent<Status>();
        animationHandler = GetComponent<AnimationHandler>();

        if(tag == EntityTag.Player)
        {
            
            inventory = GetComponent<InventoryHandler>();
        }

        gm = GameObject.FindGameObjectWithTag("ModLoader").GetComponent<Gamemaster>();
    }

    public void setState(string name, bool state)
    {
        int index = 0;
        index = gm.gameDatabase.State_Database.FindIndex(x => x == name);
        setState(index, state);
    }

    public void setState(int id, bool state)
    {
        if(id <= status.states.Length)
        {
            status.states[id] = state;
        }
    }
}
