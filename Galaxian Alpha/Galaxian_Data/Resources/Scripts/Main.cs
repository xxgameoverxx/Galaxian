using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour
{
    void OnGUI()
    {
        GUI.Box(new Rect(Screen.width / 4, Screen.height / 15, Screen.width / 2, Screen.height / 5), "AWESOME SPACE INVADERS CLONE");
        if(GUI.Button(new Rect(Screen.width / 4, Screen.height / 3, Screen.width / 2, Screen.height / 15), "New Game"))
        {
            Application.LoadLevel("LevelSelect");
        }
        if (GUI.Button(new Rect(Screen.width / 4, Screen.height / 3 + Screen.height / 15 + 20, Screen.width / 2, Screen.height / 15), "Editor"))
        {
            Application.LoadLevel("EditorSelector");
        }
        if (GUI.Button(new Rect(Screen.width / 4, Screen.height / 3 + Screen.height / 15 * 2 + 40, Screen.width / 2, Screen.height / 15), "Options"))
        {
            Debug.Log("Options");
        }
        if (GUI.Button(new Rect(Screen.width / 4, Screen.height / 3 + Screen.height / 15 * 3 + 60, Screen.width / 2, Screen.height / 15), "Credits"))
        {
            Debug.Log("Credits");
        }
        if (GUI.Button(new Rect(Screen.width / 4, Screen.height / 3 + Screen.height / 15 * 4 + 80, Screen.width / 2, Screen.height / 15), "Quit"))
        {
            Application.Quit();
        }
    }
}
