using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class LevelSelector : MonoBehaviour {

    private List<Level> levels = new List<Level>();
    private Vector2 scrollPosition = Vector2.zero;
    private Rect scrollRect = new Rect(Screen.width / 6, Screen.height / 10, Screen.width / 3, Screen.height / 3);
    private bool show = true;
    private float destroyTimer = 0.5f;
    public Level selectedLevel;
    private GUISkin skin;
    private GUIStyle button;
    private GUIStyle box;
    private GUIStyle scrollH;
    private GUIStyle scrollV;

	void Start()
    {
        if (GameObject.FindObjectOfType<Style>() != null)
        {
            skin = GameObject.FindObjectOfType<Style>().skin;
            button = skin.button;
            box = skin.box;
            scrollH = skin.horizontalScrollbar;
            scrollV = skin.verticalScrollbar;
        }
        else
        {
            Debug.LogError("Style object is not found!");
            button = new GUIStyle();
        }
        Object.DontDestroyOnLoad(this);
        foreach (var v in Directory.GetFiles(Application.dataPath + "/Resources/Levels"))
        {
            if (v.Split('.')[v.Split('.').Length - 1] != "meta")
            {
                Level level = new Level();
                level.ReadXML(v);
                levels.Add(level);
            }
        }
        selectedLevel = levels[0];
    }

    void OnGUI()
    {
        if (show)
        {
            scrollPosition = GUI.BeginScrollView(scrollRect, scrollPosition, new Rect(0, 0, Screen.width / 7, Screen.height / 30 * levels.Count), scrollH, scrollV);
            for (int i = 0; i < levels.Count; i++)
            {
                if (GUI.Button(new Rect(0, Screen.height / 15 * i, scrollRect.width, Screen.height / 15), levels[i].name, button))
                {
                    selectedLevel = levels[i];
                }
            }
            GUI.EndScrollView();

            GUI.Box(new Rect(Screen.width / 2, Screen.height / 10, Screen.width / 3, Screen.height / 2), selectedLevel.description, box);
            if (GUI.Button(new Rect(Screen.width / 6, Screen.height / 30 * 19, Screen.width / 3, Screen.height / 15), "Main Menu", button))
            {
                Application.LoadLevel("MainMenu");
                show = false;
            }
            if (GUI.Button(new Rect(Screen.width / 2, Screen.height / 30 * 19, Screen.width / 3, Screen.height / 15), "Play", button))
            {
                Application.LoadLevel("GameScene");
                show = false;
            }
        }
        else
        {
            destroyTimer -= Time.deltaTime;
            if(destroyTimer < 0)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
