using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;


public class LevelEditor : MonoBehaviour
{
    public Vector2 scrollPosition = Vector2.zero;
    public Vector2 waveScrollPosition = Vector2.zero;
    public List<string> levels = new List<string>();
    public List<Vector3> waypoints = new List<Vector3>();

    private Dictionary<int, TypeInfo> typeIdDict;
    public Dictionary<int, TypeInfo> TypeIdDict
    {
        get
        {
            if(typeIdDict == null)
            {
                typeIdDict = GameObject.FindObjectOfType<TypeInfoHolder>().typeInfoDict;
            }
            return typeIdDict;
        }
    }

    private Rect scrollRect = new Rect(Screen.width / 30, Screen.height / 40, Screen.width / 5, Screen.height / 7);
    private Rect waveScrollRect = new Rect(Screen.width / 30, Screen.height / 40 * 35, Screen.width / 5, Screen.height / 7);
    private string selectedLevel = "";

    private string newLevelName = "";
    private string newEndGame = "";
    private string newGameOver = "";
    private List<Level> levelList;
    private Dictionary<string, Level> levelDict = new Dictionary<string, Level>();
    private Level currentLevel;
    private int index = 0;
    private Wave currentWave;
    private string currentWaveName = "";
    private string currentWaveDescription = "";

    void Awake()
    {
        foreach(GameObject g in GameObject.FindGameObjectsWithTag("Waypoint"))
        {
            waypoints.Add(g.transform.position);
        }
        foreach(var v in Directory.GetFiles(Application.dataPath + "/Resources/Levels"))
        {
            if(v.Split('.')[v.Split('.').Length - 1] != "meta")
            {
                Level level = new Level();
                level.ReadXML(v);
                if (levelDict.ContainsKey(level.name))
                {
                    int i = 0;
                    while (levelDict.ContainsKey(level.name + " (" + i.ToString() + ")"))
                    {
                        i++;
                    }
                    level.name = level.name + " (" + i.ToString() + ")";
                }
                levelDict.Add(level.name, level);
                levels.Add(level.name);
            }
        }
        currentLevel = levelDict[levels[0]];
        currentWave = currentLevel.waveDict[currentLevel.wavesIds[0]];
        currentWaveName = currentWave.name;
        UpdateLevel();
    }

    void UpdateLevel()
    {
        newLevelName = currentLevel.name;
        newEndGame = currentLevel.endGameMessage;
        newGameOver = currentLevel.gameOverMessage;
        currentWave = currentLevel.waveDict[currentLevel.wavesIds[0]];
        UpdateWave();
    }

    void UpdateWave()
    {
        foreach(Enemy e in GameObject.FindObjectsOfType<Enemy>())
        {
            e.Die();
        }
        currentWaveName = currentWave.name;
        currentWaveDescription = currentWave.description;
        currentWave.SpawnAll(false);
        foreach(Actor a in GameObject.FindObjectsOfType<Actor>())
        {
            if (a.gameObject.GetComponent<MouseMove>() == null)
            {
                a.gameObject.AddComponent<MouseMove>();
            }
        }
    }

    void OnGUI()
    {
        scrollPosition = GUI.BeginScrollView(scrollRect, scrollPosition, new Rect(0, 0, Screen.width / 7, Screen.height / 30 * levels.Count));
        for (int i = 0; i < levels.Count; i++)
        {
            if (GUI.Button(new Rect(0, Screen.height / 30 * i, scrollRect.width, Screen.height / 30), levels[i]))
            {
                selectedLevel = levels[i];
                currentLevel = levelDict[levels[i]];
                UpdateLevel();
            }
        }
        GUI.EndScrollView();

        waveScrollPosition = GUI.BeginScrollView(waveScrollRect, waveScrollPosition, new Rect(0, 0, Screen.width / 7, Screen.height / 30 * currentLevel.wavesIds.Count));
        for (int i = 0; i < currentLevel.wavesIds.Count; i++)
        {
            if (GUI.Button(new Rect(0, Screen.height / 30 * i, scrollRect.width, Screen.height / 30), currentLevel.wavesIds[i].ToString()))
            {
                currentWave = currentLevel.waveDict[currentLevel.wavesIds[i]];
                UpdateWave();
            }
        }
        GUI.EndScrollView();

        newLevelName = GUI.TextField(new Rect(Screen.width / 30 * 7.5f, Screen.height / 40, Screen.width / 5, Screen.height / 30), newLevelName);
        newEndGame = GUI.TextArea(new Rect(Screen.width / 2, Screen.height / 30 * 2, Screen.width / 4, Screen.height / 8), newEndGame);
        newGameOver = GUI.TextArea(new Rect(Screen.width / 4 * 3, Screen.height / 30 * 2, Screen.width / 4, Screen.height / 8), newGameOver);

        currentWaveName = GUI.TextField(new Rect(Screen.width / 30 * 7.5f, Screen.height / 40 * 35, Screen.width / 5, Screen.height / 30), currentWaveName);
        currentWaveDescription = GUI.TextArea(new Rect(Screen.width / 2, Screen.height / 30 * 25, Screen.width / 2, Screen.height / 8), currentWaveDescription);
        GUI.Box(new Rect(Screen.width / 2, Screen.height / 30 * 23.75f, Screen.width / 2, Screen.height / 30), "Wave Message");
        GUI.Button(new Rect(Screen.width / 30 * 7.5f, Screen.height / 120 * 110, Screen.width / 10, Screen.height / 30), "Load Wave");
        GUI.Button(new Rect(Screen.width / 30 * 7.5f + Screen.width / 10, Screen.height / 120 * 110, Screen.width / 10, Screen.height / 30), "Save Wave");
        GUI.Button(new Rect(Screen.width / 30 * 7.5f, Screen.height / 120 * 114, Screen.width / 10, Screen.height / 30), "Delete Wave");
        GUI.Button(new Rect(Screen.width / 30 * 7.5f + Screen.width / 10, Screen.height / 120 * 114, Screen.width / 10, Screen.height / 30), "New Level");
        if (GUI.Button(new Rect(Screen.width / 30 * 7.5f + Screen.width / 10, Screen.height / 120 * 118, Screen.width / 10, Screen.height / 30), "Update Wave"))
        {
            UpdateWave();
        }
        if (GUI.Button(new Rect(Screen.width / 30 * 7.5f, Screen.height / 120 * 118, Screen.width / 10, Screen.height / 30), "Start / Stop"))
        {
            currentWave.ToggleEnemies();
        }

        GUI.Box(new Rect(Screen.width / 2, Screen.height / 40, Screen.width / 4, Screen.height / 30), "End Game Message");
        GUI.Box(new Rect(Screen.width / 4 * 3, Screen.height / 40, Screen.width / 4, Screen.height / 30), "Game Over Message");
        GUI.Button(new Rect(Screen.width / 30 * 7.5f, Screen.height / 120 * 8, Screen.width / 10, Screen.height / 30), "Load Level");
        GUI.Button(new Rect(Screen.width / 30 * 7.5f + Screen.width / 10, Screen.height / 120 * 8, Screen.width / 10, Screen.height / 30), "Save Level");
        GUI.Button(new Rect(Screen.width / 30 * 7.5f, Screen.height / 10, Screen.width / 10, Screen.height / 30), "Delete Level");
        GUI.Button(new Rect(Screen.width / 30 * 7.5f + Screen.width / 10, Screen.height / 10, Screen.width / 10, Screen.height / 30), "New Level");
        if(GUI.Button(new Rect(Screen.width / 30 * 7.5f + Screen.width / 10, Screen.height / 15 * 2, Screen.width / 10, Screen.height / 30), "Update Level"))
        {
            Apply();
        }
    }

    void Apply()
    {
        levelDict[currentLevel.name].name = newLevelName;
        levelDict[currentLevel.name].endGameMessage = newEndGame;
        levelDict[currentLevel.name].gameOverMessage = newGameOver;
    }
}
