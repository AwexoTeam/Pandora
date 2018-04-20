using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDefinations;
using UnityEngine.UI;

public class inventory_init : MonoBehaviour
{
    public GameObject slot_prefab;
    public Sprite bread;
    public Sprite selectedSlot;
    public Sprite slot_icon;
    private FileDatabase fileDatabase;
    private GameDatabase gameDatabase;
    private InventoryHandler inventory;

    public Dictionary<Vector2, GameObject> slots = new Dictionary<Vector2, GameObject>();
    
    // Use this for initialization
    private void Start()
    {
        fileDatabase = GameObject.FindGameObjectWithTag("ModLoader").GetComponent<FileDatabase>();
        gameDatabase = GameObject.FindGameObjectWithTag("ModLoader").GetComponent<GameDatabase>();
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<InventoryHandler>();
        
        GridLayoutGroup grid = GetComponent<GridLayoutGroup>();
        grid.cellSize = UI_CONSTS.INV_SLOT_SIZE;
        grid.spacing = new Vector2(UI_CONSTS.INV_SLOT_OFFSET, UI_CONSTS.INV_SLOT_OFFSET);
        
        for (int x = 0; x < UI_CONSTS.INV_SIZE.x; x++)
        {
            for (int y = 0; y < UI_CONSTS.INV_SIZE.y; y++)
            {
                GameObject go = Instantiate(slot_prefab);
                go.name = "Slot_" + x;
                
                go.transform.parent = this.transform;
                UI_INV_SLOT slot = go.AddComponent<UI_INV_SLOT>();
                slot.pos = new Vector2Int(x, y);
                slot.isHotkey = false;
                slots.Add(new Vector2(x, y), go);
            }
        }

        for (int x = 0; x < UI_CONSTS.INV_SIZE.x; x++)
        {
            GameObject go = Instantiate(slot_prefab);
            int y = UI_CONSTS.INV_SIZE.y + 1;
            go.name = "HotKey_Slot_" + x;

            go.transform.parent = this.transform;
            UI_INV_SLOT slot = go.AddComponent<UI_INV_SLOT>();
            slot.pos = new Vector2Int(x, y);
            slot.isHotkey = true;
            slots.Add(new Vector2(x, y), go);
        }
    }

    private void Update()
    {
        for (int x = 0; x < UI_CONSTS.INV_SIZE.x; x++)
        {
            for (int y = 0; y < UI_CONSTS.INV_SIZE.y; y++)
            {
                Vector2 currPos = new Vector2(x, y);
                GameObject go;

                slots.TryGetValue(currPos, out go);

                ItemEntity item = inventory.inventory[x, y];
                if (item.isInitalized)
                {
                    go.transform.GetChild(0).GetComponent<Image>().sprite = item.item.getSprite(fileDatabase);
                }
                else
                {
                    go.transform.GetChild(0).GetComponent<Image>().sprite = null;
                }
                
            }
        }
        for (int i = 0; i < UI_CONSTS.INV_SIZE.x; i++)
        {
            Vector2 currPos = new Vector2(i, UI_CONSTS.INV_SIZE.y+1);
            GameObject go;

            slots.TryGetValue(currPos, out go);

            ItemEntity item = inventory.hotbar[i];

            if(i == inventory.currSelectedIndex)
            {
                go.GetComponent<Image>().sprite = selectedSlot;
            }
            else
            {
                go.GetComponent<Image>().sprite = slot_icon;
            }

            if (item.isInitalized)
            {
                go.transform.GetChild(0).GetComponent<Image>().sprite = item.item.getSprite(fileDatabase);
            }
            else
            {
                go.transform.GetChild(0).GetComponent<Image>().sprite = null;
            }
        }

    }
}
