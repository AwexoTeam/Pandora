    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testy : MonoBehaviour {

    public Sprite Img;
    FileDatabase fileDB;

    private void Start()
    {
        fileDB = GameObject.FindGameObjectWithTag("ModLoader").GetComponent<FileDatabase>();

        Sprite test_image;
        if(fileDB.UI_Database.TryGetValue("Test_Image", out test_image))
        {
            Img = test_image;
        }
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(0, 0, 100, 100), Img.texture);
    }
}
