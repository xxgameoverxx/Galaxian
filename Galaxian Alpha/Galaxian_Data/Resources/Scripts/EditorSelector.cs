﻿using UnityEngine;
using System.Collections;

public class EditorSelector : MonoBehaviour {

	void OnGUI()
    {
        if(GUI.Button(new Rect(Screen.width / 6, Screen.height / 6, Screen.width / 3, Screen.height / 3), "Unit Editor"))
        {
            Application.LoadLevel("UnitEditor");
        }
        if (GUI.Button(new Rect(Screen.width / 2, Screen.height / 6, Screen.width / 3, Screen.height / 3), "Level Editor"))
        {
            Application.LoadLevel("LevelEditor");
        }
    }
}
