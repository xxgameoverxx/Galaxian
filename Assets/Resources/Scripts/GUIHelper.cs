using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUIHelper : MonoBehaviour {

	public string message = "";
    private Text text;
    private Canvas canvas;
    private GUIStyle style;
    private GUISkin skin;

    private bool show = false;

    void Start()
    {
        HideMessage();
        if (GameObject.FindObjectOfType<Style>() != null)
        {
            skin = GameObject.FindObjectOfType<Style>().skin;
            style = skin.box;
        }
        else
        {
            Debug.LogError("Style object is not found!");
            style = new GUIStyle();
        }
    }

    public void ShowMessage(string msg)
    {
        show = true;
        message = msg;
    }

    public void UpdateMessage(string msg)
    {
        message = msg;
    }

    public void HideMessage()
    {
        show = false;
    }
    public Rect rect = new Rect(Screen.width * 1.5f, Screen.height / 2.5f, Screen.width / 2, 60);
    void OnGUI()
    {
        if (show)
        {

            GUI.BeginGroup(new Rect(Screen.width / 8, Screen.height / 10, Screen.width / 4 * 3, Screen.height / 1.5f));
            GUI.Box(new Rect(0, 0, Screen.width / 4 * 3, Screen.height / 1.75f), message, style);
            GUI.Box(new Rect(Screen.width / 8, Screen.height / 1.75f, Screen.width / 2, 60), "Press SPACE to continue", style);
            GUI.EndGroup();
        }
    }
}
