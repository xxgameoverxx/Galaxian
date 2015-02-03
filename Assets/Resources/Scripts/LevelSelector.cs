using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class LevelSelector : MonoBehaviour {

    private List<Level> levels = new List<Level>();
    private Vector2 scrollPosition = Vector2.zero;
    private Rect scrollRect = new Rect(0, 0, 350, 400);
    private Rect groupRect = new Rect(Screen.width / 2 - 350, Screen.height / 10, 700, 500);
    private bool show = true;
    private float destroyTimer = 0.5f;
    public Level selectedLevel;
    public GUISkin skin;


	void Start()
    {
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
        GUI.skin = skin;
        if (show)
        {
            GUI.BeginGroup(groupRect);
            scrollPosition = GUI.BeginScrollView(scrollRect, scrollPosition, new Rect(0, 0, Screen.width / 7, Screen.height / 15 * levels.Count));
            for (int i = 0; i < levels.Count; i++)
            {
                if (GUI.Button(new Rect(0, Screen.height / 15 * i, scrollRect.width, Screen.height / 15), levels[i].name))
                {
                    selectedLevel = levels[i];
                }
            }
            GUI.EndScrollView();

            GUI.Box(new Rect(350, 0, 350, 400), selectedLevel.name + "\n\n" + selectedLevel.description);
            if (GUI.Button(new Rect(100, 410, 200, 90), "Main Menu"))
            {
                Application.LoadLevel("MainMenu");
                show = false;
            }
            if (GUI.Button(new Rect(400, 410, 200, 90), "Play"))
            {
                Application.LoadLevel("GameScene");
                show = false;
            }
            GUI.EndGroup();
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
