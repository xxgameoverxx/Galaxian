using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour
{
    public GUISkin skin;
    private GUIStyle button;
    private GUIStyle box;

    private Rect groupRect = new Rect(Screen.width / 2 - 256, Screen.height / 10, 512, 500);

    void Start()
    {
        //if (GameObject.FindObjectOfType<Style>() != null)
        //{
        //    skin = GameObject.FindObjectOfType<Style>().skin;
        //    button = skin.button;
        //    box = skin.box;
        //}
        //else
        //{
        //    Debug.LogError("Style object is not found!");
        //    button = new GUIStyle();
        //}
    }

    void OnGUI()
    {
        GUI.skin = skin;
        GUI.BeginGroup(groupRect);
        GUI.Box(new Rect(0, 0, 512, 200), "AWESOME SPACE INVADERS CLONE");
        if (GUI.Button(new Rect(128, 210, 256, 50), "New Game"))
        {
            Application.LoadLevel("LevelSelect");
        }
        if (GUI.Button(new Rect(128, 270, 256, 50), "Editor"))
        {
            Application.LoadLevel("EditorSelector");
        }
        if (GUI.Button(new Rect(128, 330, 256, 50), "Options"))
        {
            Debug.Log("Options");
        }
        if (GUI.Button(new Rect(128, 390, 256, 50), "Credits"))
        {
            Debug.Log("Credits");
        }
        if (GUI.Button(new Rect(128, 450, 256, 50), "Quit"))
        {
            Application.Quit();
        }
        GUI.EndGroup();
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
