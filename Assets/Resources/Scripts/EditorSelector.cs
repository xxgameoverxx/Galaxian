using UnityEngine;
using System.Collections;

public class EditorSelector : MonoBehaviour {

    private GUIStyle button;
    private GUISkin skin;
    private Rect groupRect = new Rect(Screen.width / 2 - 256, Screen.height / 4, 512, 500);

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
        GUI.BeginGroup(groupRect);
        if(GUI.Button(new Rect(0, 0, 250, 100), "Unit Editor", button))
        {
            Application.LoadLevel("UnitEditor");
        }
        if (GUI.Button(new Rect(256, 0, 250, 100), "Level Editor", button))
        {
            Application.LoadLevel("LevelEditor");
        }
        if (GUI.Button(new Rect(128, 250, 256, 50), "Main Menu", button))
        {
            Application.LoadLevel("MainMenu");
        }
        GUI.EndGroup();
    }
}
