using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Interfaces;

public class GameDatabase : MonoBehaviour
{
    private void Start()
    {
        RegisterUIState("main_inventory"); 
    }
    
    public List<string> Stat_Database = new List<string>();
    public List<string> State_Database = new List<string>();
    public List<string> UI_State_Database = new List<string>();
    public List<Item> Item_Database = new List<Item>();
    //public Dictionary<string, Node> Node_Database;
    //public Dictionary<string, BuildingParts> BuildPart_Database;
    //public Dictionary<string, Monsters> Monster_Database;

    public int stats { get { return Stat_Database.Count; } }
    public int states { get { return State_Database.Count; } }
    public int ui_states { get { return UI_State_Database.Count; } }
    public int items { get { return Item_Database.Count; } }

    public void RegisterStat(string name)
    {
        Stat_Database.Add(name);
    }
    public void RegisterState(string name)
    {
        State_Database.Add(name);
    }
    public void RegisterUIState(string name)
    {
        UI_State_Database.Add(name);
    }
    

    public void RegisterItem(string name, string img_name, int maxStack, Dictionary<int, float> item_stats = null, params IItemFunction[] functions)
    {
        string regName = "_" + name.Replace(' ', '_') + "_";
        RegisterItem(name, regName, img_name, maxStack, item_stats, functions);
    }
    public void RegisterItem(string name, string registery_name, string image_name, int maxStack, Dictionary<int, float> item_stats, params IItemFunction[] functions)
    {   
        Item item = new Item(registery_name, image_name, name, maxStack);

        if (functions != null)
        {
            item.functions = functions.ToList();
        }

        item.item_stats = item_stats;

        if(Item_Database.Contains(item))
        {
            Item_Database.Remove(item);
        }

        Item_Database.Add(item);
    }

    public void RegisterItem(Item item)
    {
        Item_Database.Add(item);
    }

    public int GetStatIndexByName(string name) { return Stat_Database.FindIndex(x => x == name); }
    public string GetStatNameByIndex(int index) { return Stat_Database[index]; }

    public int GetStateIndexByName(string name) { return State_Database.FindIndex(x => x == name); }
    public string GetStateNameByIndex(int index) { return State_Database[index]; }
    
    public int GetUIStateIndexByName(string name) { return UI_State_Database.FindIndex(x => x == name); }
    public string GetUIStateNameByIndex(int index) { return UI_State_Database[index]; }

    public int GetItemIndexByName(string name) { return Item_Database.FindIndex(x => x.registry_Name == name); }
    public Item GetItemByIndex(int index) { return Item_Database[index]; }
    public Item GetItemByName(string name) { return Item_Database.Find(x => x.registry_Name == name); }
}
    