﻿using Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Item {
    public string image_name { private set; get; }
    public string registry_Name { private set; get; }
    public string display_name { private set; get; }
    public int maxStack { private set; get; }

    public GameObject model { private set; get; }
    public Dictionary<int, float> item_stats { set; get; }

    public List<IItemFunction> functions { set; get; }

    public Item(string imgName = "")
    {
        image_name = "";
        registry_Name = "";
        display_name = "";
        maxStack = 0;
        item_stats = null;
        functions = null;
        model = null;
    }

    public Item(string _image_name, string _display_name, int _maxStack, GameObject _model = null)
    {
        image_name = _image_name;
        registry_Name = "_" + _display_name.Replace(' ', '_').ToLower() + "_";
        display_name = _display_name;

        maxStack = _maxStack;
        item_stats = new Dictionary<int, float>();
        functions = new List<IItemFunction>();
        model = _model;
    }

    public Item(string _registry_name, string _image_name, string _display_name, int _maxStack, GameObject _model = null)
    {
        image_name = _image_name;
        registry_Name = _registry_name;
        display_name = _display_name;
        maxStack = _maxStack;
        model = _model;

        item_stats = new Dictionary<int, float>();
        functions = new List<IItemFunction>();
    }
    
    public Sprite getSprite(FileDatabase database) { return database.UI_Database[image_name]; }

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
