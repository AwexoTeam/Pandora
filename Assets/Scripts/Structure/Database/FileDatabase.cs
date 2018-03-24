using GameDefinations;  
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

public class FileDatabase : MonoBehaviour
{
    private GameDatabase gameDatabase;

    public Dictionary<string, AnimationClip> Animation_Database = new Dictionary<string, AnimationClip>();
    public Dictionary<string, GameObject> Model_Database = new Dictionary<string, GameObject>();
    public Dictionary<string, AudioClip> AudioClip_Database = new Dictionary<string, AudioClip>();
    public Dictionary<string, Texture2D> Texture_Database = new Dictionary<string, Texture2D>();
    public Dictionary<string, Sprite> UI_Database = new Dictionary<string, Sprite>();
    public Dictionary<string, TextAsset> File_Database = new Dictionary<string, TextAsset>();
    
    public void Register(AnimationClip val)
    {
        string name = val.name.Replace('@', '_').Replace('-', '_');

        if (Animation_Database.ContainsKey(name))
        {
            Animation_Database.Remove(name);
        }

        Animation_Database.Add(name, val);
    }
    public void Register(AudioClip val)
    {
        string name = val.name;

        if (AudioClip_Database.ContainsKey(name))
        {
            AudioClip_Database.Remove(name);
        }

        AudioClip_Database.Add(name, val);
    }
    public void Register(Texture2D val)
    {
        string name = val.name;

        if (Texture_Database.ContainsKey(name))
        {
            Texture_Database.Remove(name);
        }

        Texture_Database.Add(name, val);
    }
    public void Register(GameObject val)
    {
        string name = val.name;

        if (Model_Database.ContainsKey(name))
        {
            Model_Database.Remove(name);
        }

        Model_Database.Add(name, val);
    }
    public void Register(Sprite val)
    {
        string name = val.name;

        if (UI_Database.ContainsKey(name))
        {
            UI_Database.Remove(name);
        }

        UI_Database.Add(name, val);
    }
    public void Register(TextAsset val)
    {
        string name = val.name;

        if (File_Database.ContainsKey(name))
        {
            File_Database.Remove(name);
        }

        File_Database.Add(name, val);
    }

    private void Start()
    {
        gameDatabase = GameObject.FindGameObjectWithTag("ModLoader").GetComponent<GameDatabase>();
    }

    public string[] debug_array;
    private void Update()
    {
        List<string> list = new List<string>();

        foreach (var sprite in Animation_Database)
        {
            list.Add(sprite.Key);
        }

        debug_array = list.ToArray();
    }

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
