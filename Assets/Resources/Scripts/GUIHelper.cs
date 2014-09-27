using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUIHelper : MonoBehaviour {

	public string message = "";
    private Text text;
    private Canvas canvas;

    void Start()
    {
        text = GetComponentInChildren<Text>();
        canvas = GetComponent<Canvas>();
        HideMessage();
    }


    public void ShowMessage(string msg)
    {
        canvas.enabled = true;
        text.text = msg;
    }

    public void UpdateMessage(string msg)
    {
        message = msg;
        text.text = msg;
    }

    public void HideMessage()
    {
        canvas.enabled = false;
    }
}
