using System;
using System.Collections.Generic;
using UnityEngine;

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

    public int inv_width { get { return 9; } }
    public int inv_height { get { return 5; } }
    public ItemEntity[,] inventory;
    public GameDatabase gameDatabase;

    public string[] inv_array;

    private void Start()
    {
        inventory = new ItemEntity[inv_width, inv_height];
        gameDatabase = GameObject.FindGameObjectWithTag("ModLoader").GetComponent<GameDatabase>();

        for (int x = 0; x < inventory.GetLength(0); x++)
        {
            for (int y = 0; y < inventory.GetLength(1); y++)
            {
                inventory[x, y] = new ItemEntity();
            }
        }
    }

    private void Update()
    {
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
    }
}   

