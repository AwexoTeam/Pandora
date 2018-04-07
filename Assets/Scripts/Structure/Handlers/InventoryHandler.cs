using Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;
using GameDefinations;

public enum SelectState
{
    UP,
    DOWN,
}

public class ItemEntity
{
    public Item item;
    public int count;
    public string UID;
    public bool isInitalized { private set; get; }

    public ItemEntity() { }

    public ItemEntity(Item _item, int _count)
    {
        isInitalized = true;
        item = _item;
        count = _count;
        UID = Guid.NewGuid().ToString();
    }
}

public class InventoryHandler : MonoBehaviour {
    public ItemEntity[,] inventory;
    public ItemEntity[] hotbar;
    public GameDatabase gameDatabase;
    public Gamemaster gameMaster;
    public ItemEntity currItem;
    public string itemName;
    public GameObject Hand;

    public int currSelectedIndex = 0;
    public string[] inv_array;

    private void GiveItem()
    {
        AddItem(gameDatabase.Item_Database[0],1);
        AddItem(gameDatabase.Item_Database[1],1000);

    }

    private void Start()
    {
        inventory = new ItemEntity[UI_CONSTS.INV_SIZE.x, UI_CONSTS.INV_SIZE.y];
        hotbar = new ItemEntity[UI_CONSTS.INV_SIZE.x];

        gameMaster = GameObject.FindGameObjectWithTag("ModLoader").GetComponent<Gamemaster>();
        gameDatabase = gameMaster.gameDatabase;
        
        for (int x = 0; x < inventory.GetLength(0); x++)
        {
            for (int y = 0; y < inventory.GetLength(1); y++)
            {
                inventory[x, y] = new ItemEntity();
            }
        }

        for (int i = 0; i < UI_CONSTS.INV_SIZE.x; i++)
        {
            hotbar[i] = new ItemEntity();
        }

        GiveItem();
        
    }

    private void Update()
    {
        if (currItem != null) { itemName = currItem.item.display_name; }

        if (Input.GetMouseButtonUp(0) && gameMaster.uiHandler.currState == "")
        {
            for (int x = 0; x < inventory.GetLength(0); x++)
            {
                for (int y = 0; y < inventory.GetLength(1); y++)
                {
                    ItemEntity item = inventory[x, y];

                    if (item.isInitalized)
                    {
                        for (int i = 0; i < item.item.functions.Count; i++)
                        {
                            IItemFunction fnk = item.item.functions[i];
                            fnk.Initialize(gameMaster);
                            fnk.OnItemUsed(gameMaster.playerEntityController);
                        }
                    }
                }
            }
        }

        List<string> list = new List<string>();
        
        for (int x = 0; x < inventory.GetLength(0); x++)
        {
            for (int y = 0; y < inventory.GetLength(1); y++)
            {
                if (inventory[x, y] != null)
                {
                    list.Add(inventory[x, y].item.registry_Name);
                }
            }
        }

        inv_array = list.ToArray();
    }

    private Vector2Int FindFirstEmptySpot()
    {
        for (int y = 0; y < inventory.GetLength(1); y++)
        {
            for (int x = 0; x < inventory.GetLength(0); x++)
            {
                if (!inventory[x, y].isInitalized)
                {
                    return new Vector2Int(x, y);
                }
            }
        }
        return new Vector2Int(-1, -1);
    }

    public bool HasItem(Item item)
    {
        for (int x = 0; x < inventory.GetLength(0); x++)
        {
            for (int y = 0; y < inventory.GetLength(1); y++)
            {
                Item currItem = inventory[x, y].item;

                if(currItem == item)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public bool HasItem(Item item, out Vector2Int[] coords)
    {
        bool rtn = false;
        List<Vector2Int> coordList = new List<Vector2Int>();

        for (int x = 0; x < inventory.GetLength(0); x++)
        {
            for (int y = 0; y < inventory.GetLength(1); y++)
            {
                Item currItem = inventory[x, y].item;

                if(currItem == item)
                {
                    coordList.Add(new Vector2Int(x, y));
                    rtn = true;
                }
            }
        }

        coords = coordList.ToArray();
        return rtn;
    }

    public int GetItemAmount(Item item)
    {
        int cnt = 0;

        for (int x = 0; x < inventory.GetLength(0); x++)
        {
            for (int y = 0; y < inventory.GetLength(1); y++)
            {
                ItemEntity currItem = inventory[x, y];

                if(currItem.item.registry_Name == item.registry_Name)
                {
                    cnt += currItem.count;
                }
            }
        }

        return cnt;
    }

    public int GetItemAmount(Item item, out Vector2Int[] coords, bool includeFullStacks = true)
    {
        int cnt = 0;
        List<Vector2Int> coordList = new List<Vector2Int>();

        for (int x = 0; x < inventory.GetLength(0); x++)
        {
            for (int y = 0; y < inventory.GetLength(1); y++)
            {
                ItemEntity currItem = inventory[x, y];

                if (currItem != null)
                {
                    if (currItem.item.registry_Name == item.registry_Name)
                    {
                        cnt += currItem.count;
                        if (includeFullStacks) { coordList.Add(new Vector2Int(x, y)); }
                        else
                        {
                            if(currItem.count < item.maxStack) { coordList.Add(new Vector2Int(x, y)); }
                        }
                    }
                }
            }
        }

        coords = coordList.ToArray();
        return cnt;
    }
    
    public void AddItem(Item item, int amount)
    {
        Vector2Int[] existingItems;
        GetItemAmount(item, out existingItems, false);
        
        if(existingItems.Length > 0)
        {
            for (int i = 0; i < existingItems.Length; i++)
            {
                Vector2Int coord = existingItems[i];
                ItemEntity itemEntity = inventory[coord.x, coord.y];


                if(itemEntity.count + amount <= itemEntity.item.maxStack)
                {
                    AddToInventory(coord, item, itemEntity.count + amount);
                }
                else
                {
                    int difference = item.maxStack - itemEntity.count;
                    
                    AddToInventory(coord, item, item.maxStack);
                    amount -= difference;
                }
            }
        }
        
        if(amount <= item.maxStack && amount > 0)
        {
            AddToInventory(item, amount);
        }
        else
        {
            AddToInventory(item, item.maxStack);
            amount -= item.maxStack;
            AddItem(item, amount);
            
        }
    }
    
    private void AddToInventory(Item item, int amount)
    {
        Vector2Int coords = FindFirstEmptySpot();
        AddToInventory(coords, item, amount);

    }

    private void AddToInventory(Vector2Int coords, Item item, int amount)
    {
        inventory[coords.x, coords.y] = new ItemEntity(item, amount);
        Item i = inventory[coords.x, coords.y].item;
        foreach (IItemFunction func in i.functions)
        {
            func.Initialize(gameMaster);
        }
    }

    public void RemoveItemAt(int x, int y, int amount)
    {
        inventory[x, y].count -= amount;

        if(inventory[x,y].count <= 0) { inventory[x, y] = new ItemEntity(); }
    }
    
    public bool RemoveItem(Item item, int amount)
    {
        Vector2Int[] coords;
        int itemCnt = GetItemAmount(item, out coords);
        
        if(itemCnt < amount) { return false; }
        else
        {
            for (int i = 0; i < coords.Length; i++)
            {
                Vector2Int v = coords[i];
                ItemEntity entity = inventory[v.x, v.y];

                if(amount <= entity.count)
                {
                    if(entity.count - amount <= 0)
                    {
                        inventory[v.x, v.y] = new ItemEntity();
                        return true;
                    }
                    else
                    {
                        inventory[v.x, v.y].count -= amount;
                    }
                }
                else
                {
                    amount -= entity.count;
                    inventory[v.x, v.y] = new ItemEntity();
                    return true;
                }
            }
        }

        return true;
    }

    public void SwitchItemSlot(UI_INV_SLOT a, UI_INV_SLOT b)
    {
        if(a.isHotkey && !b.isHotkey)
        {
            ItemEntity itemA = hotbar[a.pos.x];
            ItemEntity itemB = inventory[b.pos.x, b.pos.y];

            hotbar[a.pos.x] = itemB;
            inventory[b.pos.x, b.pos.y] = itemA;

            if (a.pos.x == currSelectedIndex) { UpdateCurrItem(); }
        }

        else if(!a.isHotkey && b.isHotkey)
        {
            ItemEntity itemB = hotbar[b.pos.x];
            ItemEntity itemA = inventory[a.pos.x, a.pos.y];

            inventory[a.pos.x,a.pos.y] = itemB;
            hotbar[b.pos.x] = itemA;

            if (b.pos.x == currSelectedIndex) { UpdateCurrItem(); }
        }
        else if(!a.isHotkey && !b.isHotkey)
        {
            ItemEntity itemA = inventory[a.pos.x, a.pos.y];
            ItemEntity itemB = inventory[b.pos.x, b.pos.y];
            
            inventory[b.pos.x, b.pos.y] = itemA;
            inventory[a.pos.x,a.pos.y] = itemB;
        }
        else if(a.isHotkey && b.isHotkey)
        {
            ItemEntity itemA = hotbar[a.pos.x];
            ItemEntity itemB = hotbar[b.pos.x];

            hotbar[b.pos.x] = itemA;
            hotbar[a.pos.x] = itemB;

            if (a.pos.x == currSelectedIndex) { UpdateCurrItem(); }
            if (b.pos.x == currSelectedIndex) { UpdateCurrItem(); }
        }
    }

    public void ChangeSelected(SelectState state)
    {
        if(GetComponent<UIHandler>().currState != "inv") { return; }
        int multiplier = state == SelectState.UP ? 1 : -1;
        currSelectedIndex += multiplier;

        if(currSelectedIndex < 0) { currSelectedIndex = UI_CONSTS.INV_SIZE.x - 1; }
        else if(currSelectedIndex > UI_CONSTS.INV_SIZE.x - 1) { currSelectedIndex = 0; }


        UpdateCurrItem();
    }

    private void UpdateCurrItem()
    {
        ItemEntity item = hotbar[currSelectedIndex];

        if (item.isInitalized)
        {
            currItem = item;

            if (item.item.functions.Count > 0)
            {
                foreach (IItemFunction func in item.item.functions)
                {
                    func.SelectedConditionCheck(gameMaster.playerEntityController);
                }
            }
        }
    }
}   

