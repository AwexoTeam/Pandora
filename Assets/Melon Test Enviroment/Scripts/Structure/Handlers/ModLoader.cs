using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using System;
using System.Reflection;
using GameDefinations;

public class ModLoader : MonoBehaviour
{
    public FileDatabase database;
    
    void Awake()
    {
        database = GetComponent<FileDatabase>();

        string[] visualMods = Directory.GetFiles(CONST.MOD_PATH);
        
        for (int i = 0; i < visualMods.Length; i++)
        {
            database.HandleFile(visualMods[i]);
        }
    }
}