using UnityEngine;
using System.Collections;

public class EditorSelector : MonoBehaviour {

    private GUIStyle button;
    private GUISkin skin;

    void Start()
    {
        if (GameObject.FindObjectOfType<Style>() != null)
        {
            skin = GameObject.FindObjectOfType<Style>().skin;
            button = skin.button;
        }
        else
        {
            Debug.LogError("Style object is not found!");
            button = new GUIStyle();
        }
    }

	void OnGUI()
    {
        if(GUI.Button(new Rect(Screen.width / 6, Screen.height / 6, Screen.width / 3, Screen.height / 3), "Unit Editor", button))
        {
            Application.LoadLevel("UnitEditor");
        }
        if (GUI.Button(new Rect(Screen.width / 2, Screen.height / 6, Screen.width / 3, Screen.height / 3), "Level Editor", button))
        {
            Application.LoadLevel("LevelEditor");
        }
        if (GUI.Button(new Rect(Screen.width / 4, Screen.height / 2, Screen.width / 2, Screen.height / 10), "Main Menu", button))
        {
            Application.LoadLevel("MainMenu");
        }
    }
}
