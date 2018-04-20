using GameDefinations;  
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

/// <summary>
/// This class is for every file such as:
/// Animations,
/// Prefabs and other game models,
/// Audio Clips (SFX, Voice acting and music)
/// Textures
/// UI and sprites so no extra casting
/// TextAssets are every other assets such as text files or other things.
/// </summary>
public class FileDatabase : MonoBehaviour
{
    private GameDatabase gameDatabase;

    public Dictionary<string, AnimationClip> Animation_Database = new Dictionary<string, AnimationClip>();
    public Dictionary<string, GameObject> Model_Database = new Dictionary<string, GameObject>();
    public Dictionary<string, AudioClip> AudioClip_Database = new Dictionary<string, AudioClip>();
    public Dictionary<string, Texture2D> Texture_Database = new Dictionary<string, Texture2D>();
    public Dictionary<string, Sprite> UI_Database = new Dictionary<string, Sprite>();
    public   Dictionary<string, TextAsset> File_Database = new Dictionary<string, TextAsset>();
    
    /// <summary>
    /// Register an Animation and add it to the database.
    /// If it exist we override it to avoid errors.
    /// </summary>
    /// <param name="val">the animation to be registered</param>
    public void Register(AnimationClip val)
    {
        string name = val.name.Replace('@', '_').Replace('-', '_');

        if (Animation_Database.ContainsKey(name))
        {
            Animation_Database.Remove(name);
        }

        Animation_Database.Add(name, val);
    }

    /// <summary>
    /// Register an Audio Clip and add it to the database.
    /// If it exist we override it to avoid errors.
    /// </summary>
    /// <param name="val"></param>
    public void Register(AudioClip val)
    {
        string name = val.name;

        if (AudioClip_Database.ContainsKey(name))
        {
            AudioClip_Database.Remove(name);
        }

        AudioClip_Database.Add(name, val);
    }

    /// <summary>
    /// Register a Texture and add it to the database.
    /// If it exist we override it to avoid errors.
    /// </summary>
    /// <param name="val"></param>
    public void Register(Texture2D val)
    {
        string name = val.name;

        if (Texture_Database.ContainsKey(name))
        {
            Texture_Database.Remove(name);
        }

        Texture_Database.Add(name, val);
    }

    /// <summary>
    /// Register a 3D model or prefab and add it to the database.
    /// If it exist we override it to avoid errors.
    /// </summary>
    /// <param name="val"></param>
    public void Register(GameObject val)
    {
        string name = val.name;

        if (Model_Database.ContainsKey(name))
        {
            Model_Database.Remove(name);
        }

        Model_Database.Add(name, val);
    }

    /// <summary>
    /// Register a Sprite and add it to the database.
    /// If it exist we override it to avoid errors.
    /// </summary>
    /// <param name="val"></param>
    public void Register(Sprite val)
    {
        string name = val.name;

        if (UI_Database.ContainsKey(name))
        {
            UI_Database.Remove(name);
        }

        UI_Database.Add(name, val);
    }

    /// <summary>
    /// Register a Text Asset and add it to the database.
    /// If it exist we override it to avoid errors.
    /// </summary>
    /// <param name="val"></param>
    public void Register(TextAsset val)
    {
        string name = val.name;

        if (File_Database.ContainsKey(name))
        {
            File_Database.Remove(name);
        }

        File_Database.Add(name, val);
    }

    /// <summary>
    /// Private Start function which we setup this script.
    /// </summary>
    private void Start()
    {
        gameDatabase = GameObject.FindGameObjectWithTag("ModLoader").GetComponent<GameDatabase>();
    }

    /// <summary>
    /// Ignore this :D
    /// </summary>
    public string[] debug_array;

    /// <summary>
    /// Also ignore this ;) 
    /// </summary>
    private void Update()
    {
        List<string> list = new List<string>();

        foreach (var sprite in Animation_Database)
        {
            list.Add(sprite.Key);
        }

        debug_array = list.ToArray();
    }

    /// <summary>
    /// We may need this to be public sooner or later but this will load in each mod and if its an assetbundle
    /// We will register it as a mod else if its DLL we will load its assembly and run our mod start.
    /// </summary>
    /// <param name="path"></param>
    public void HandleFile(string path)
    {
        string extension = Path.GetExtension(path);

        if (extension == string.Empty)
        {
            AssetBundle mod = AssetBundle.LoadFromFile(path);
            if (mod != null)
            {
                RegisterMod(mod);
            }
        }
        else if (extension == ".dll")
        {
            var file = Assembly.LoadFile(path);
            var type = file.GetType(CONST.MOD_CLASS);
            var obj = Activator.CreateInstance(type);

            var method = type.GetMethod(CONST.MOD_METHOD, BindingFlags.NonPublic | BindingFlags.Instance);
            method.Invoke(obj, null);
        }
    }

    /// <summary>
    /// Here we register all files in said assetbundle :)
    /// </summary>
    /// <param name="mod"></param>
    private void RegisterMod(AssetBundle mod)
    {
        AnimationClip[] anims = mod.LoadAllAssets<AnimationClip>();
        for (int i = 0; i < anims.Length; i++) { Register(anims[i]); }

        GameObject[] objs = mod.LoadAllAssets<GameObject>();
        for (int i = 0; i < objs.Length; i++) { Register(objs[i]); }

        AudioClip[] clips = mod.LoadAllAssets<AudioClip>();
        for (int i = 0; i < clips.Length; i++) { Register(clips[i]); }

        Sprite[] sprites = mod.LoadAllAssets<Sprite>();
        for (int i = 0; i < sprites.Length; i++) { Register(sprites[i]); }

        Texture2D[] textures = mod.LoadAllAssets<Texture2D>();
        for (int i = 0; i < textures.Length; i++) { Register(textures[i]); }

        TextAsset[] textAssets = mod.LoadAllAssets<TextAsset>();
        for (int i = 0; i < textAssets.Length; i++) { Register(textAssets[i]); }

    }
}
