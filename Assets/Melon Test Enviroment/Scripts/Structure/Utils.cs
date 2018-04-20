using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Reflection;
using System.IO;

public static class Utils {

    /// <summary>
    /// This function returns an array of all T enum value.
    /// Use this to get all values in a defined enum.
    /// </summary>
    /// <typeparam name="T">T is the enum type</typeparam>
    /// <returns>An Array of all values in defined enum.</returns>
    public static T[] GetEnumArray<T>()
    {
        return (T[])Enum.GetValues(typeof(T)).Cast<T>();
    }
    

    /// <summary>
    /// Returns if the number is even.
    /// </summary>
    /// <param name="chk">The number to be checked</param>
    /// <returns>Returns true if the number is even</returns>
    public static bool isEven(float chk)
    {
        return chk % 2 == 0;
    }

    public static bool isEven(int chk)
    {
        return chk % 2 == 0;
    }
    
    public static IEnumerable<T> GetEnumerableOfType<T>() where T : class
    {
        List<T> objects = new List<T>();

        var list = Assembly.GetAssembly(typeof(T)).GetTypes().Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(T)));

        foreach (Type type in list)
        {
            objects.Add((T)Activator.CreateInstance(type, null));
        }

        objects.Sort();
        return objects;
    }

    public static List<String> DirSearch(string sDir)
    {
        List<String> files = new List<String>();
        try
        {
            foreach (string f in Directory.GetFiles(sDir))
            {
                files.Add(f);
            }
            foreach (string d in Directory.GetDirectories(sDir))
            {
                files.AddRange(DirSearch(d));
            }
        }
        catch (System.Exception excpt)
        {
            Debug.Log(excpt.Message);
        }

        return files;
    }

    public static bool isOfFileType(string fileName,string type)
    {
        string[] split = fileName.Split('.');
        return split[1] == type;
    }
}
