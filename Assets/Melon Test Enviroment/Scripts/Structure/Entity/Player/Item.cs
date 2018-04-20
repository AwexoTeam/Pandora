using Interfaces;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public struct Item {
    public string image_name { private set; get; }
    public string registry_Name { private set; get; }
    public string display_name { private set; get; }
    public int maxStack { private set; get; }
    
    public ModelInfo modelInfo { set; get; }
    public Dictionary<int, float> item_stats { set; get; }

    public List<IItemFunction> functions { set; get; }

    public Item(string imgName = "")
    {
        image_name = "";
        registry_Name = "";
        display_name = "";
        maxStack = 0;
        item_stats = null;
        functions = new List<IItemFunction>();
        modelInfo = null;
    }

    public Item(string _image_name, string _display_name, int _maxStack, ModelInfo info = null)
    {
        image_name = _image_name;
        registry_Name = "_" + _display_name.Replace(' ', '_').ToLower() + "_";
        display_name = _display_name;

        maxStack = _maxStack;
        item_stats = new Dictionary<int, float>();
        functions = new List<IItemFunction>();
        modelInfo = info;
    }

    public Item(string _registry_name, string _image_name, string _display_name, int _maxStack, ModelInfo info = null)
    {
        image_name = _image_name;
        registry_Name = _registry_name;
        display_name = _display_name;
        maxStack = _maxStack;
        modelInfo = info;

        item_stats = new Dictionary<int, float>();
        functions = new List<IItemFunction>();
    }
    
    public Sprite getSprite(FileDatabase database)
    {
        Sprite icon = new Sprite();
        bool sucess = database.UI_Database.TryGetValue(this.image_name, out icon);

        if (sucess) { return icon; }
        else
        {
            if (database.UI_Database.Count > 0)
            {
                foreach (var item in database.UI_Database)
                {
                    icon = item.Value;
                    break;
                }

                return icon;
            }
            else { return null; }
        }
    }

    public void onEquip(Status activator)
    {
        foreach (var item in item_stats)
        {
            activator.AddToCurrStat(item.Key, item.Value);
        }
    }
    public void onUnEquip(Status activator)
    {
        foreach (var item in item_stats)
        {
            activator.AddToCurrStat(item.Key, item.Value * -1);
        }
    }

    public bool isNull()
    {
        return this == new Item();
    }

    public static bool operator ==(Item a, Item b)
    {
        bool rtn = false;
        rtn = a.registry_Name == b.registry_Name;

        return rtn;
    }

    public static bool operator !=(Item a, Item b)
    {
        bool rtn = false;
        rtn = a.registry_Name != b.registry_Name;

        return rtn;
    }
}
