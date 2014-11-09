using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour
{
    private GUISkin skin;
    private GUIStyle button;
    private GUIStyle box;

    void Start()
    {
        if (GameObject.FindObjectOfType<Style>() != null)
        {
            skin = GameObject.FindObjectOfType<Style>().skin;
            button = skin.button;
            box = skin.box;
        }
        else
        {
            Debug.LogError("Style object is not found!");
            button = new GUIStyle();
        }
    }

    void OnGUI()
    {
        GUI.Box(new Rect(Screen.width / 4, Screen.height / 15, Screen.width / 2, Screen.height / 5), "AWESOME SPACE INVADERS CLONE", box);
        if (GUI.Button(new Rect(Screen.width / 4, Screen.height / 3, Screen.width / 2, Screen.height / 15), "New Game", button))
        {
            Application.LoadLevel("LevelSelect");
        }
        if (GUI.Button(new Rect(Screen.width / 4, Screen.height / 3 + Screen.height / 15 + 20, Screen.width / 2, Screen.height / 15), "Editor", button))
        {
            Application.LoadLevel("EditorSelector");
        }
        if (GUI.Button(new Rect(Screen.width / 4, Screen.height / 3 + Screen.height / 15 * 2 + 40, Screen.width / 2, Screen.height / 15), "Options", button))
        {
            Debug.Log("Options");
        }
        if (GUI.Button(new Rect(Screen.width / 4, Screen.height / 3 + Screen.height / 15 * 3 + 60, Screen.width / 2, Screen.height / 15), "Credits", button))
        {
            Debug.Log("Credits");
        }
        if (GUI.Button(new Rect(Screen.width / 4, Screen.height / 3 + Screen.height / 15 * 4 + 80, Screen.width / 2, Screen.height / 15), "Quit", button))
        {
            Application.Quit();
        }
        //GUI.BeginGroup(new Rect(Screen.width / 4, Screen.height / 3, Screen.width / 2, Screen.height / 2), skin.box);
        //GUI.EndGroup();
        //GUILayout.BeginArea(new Rect(Screen.width / 4, Screen.height / 3, Screen.width / 2, Screen.height / 2), skin.box);
        //if (GUI.Button(new Rect(200, 100, 200, 50), "New Game", button))
        //{
        //    Application.LoadLevel("LevelSelect");
        //}
        //GUILayout.EndArea();
    }
}
