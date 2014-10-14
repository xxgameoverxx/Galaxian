using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Linq;


public class LevelEditor : MonoBehaviour
{
    public Vector2 scrollPosition = Vector2.zero;
    public Vector2 waveScrollPosition = Vector2.zero;
    public List<string> levels = new List<string>();
    public List<Vector3> waypoints = new List<Vector3>();
    private int currentEnemyIndex = 0;
    private bool changePrefab = true;
    private TypeInfo currentEnemy;
    private bool deleteLevelCheck = false;

    private TypeInfoHolder typeInfoHolder;
    private Dictionary<int, TypeInfo> typeIdDict;
    public Dictionary<int, TypeInfo> TypeIdDict
    {
        get
        {
            if(typeInfoHolder == null)
            {
                typeInfoHolder = new TypeInfoHolder();
                typeIdDict = typeInfoHolder.typeInfoDict;
            }
            return typeIdDict;
        }
    }

    private GameObject currentPrefab;
    private GameObject CurrentPrefab
    {
        get
        {
            return Resources.Load(TypeIdDict[typeInfoHolder.enemyIds[currentEnemyIndex]].prefab) as GameObject;
        }
    }

    private Rect scrollRect = new Rect(Screen.width / 30, Screen.height / 40, Screen.width / 5, Screen.height / 7);
    private Rect waveScrollRect = new Rect(Screen.width / 30, Screen.height / 40 * 35, Screen.width / 5, Screen.height / 7);
    private string selectedLevel = "";

    private string newLevelName = "";
    private string newEndGame = "";
    private string newGameOver = "";
    private string newLevelDescription = "";
    private List<Level> levelList;
    private Dictionary<string, Level> levelDict = new Dictionary<string, Level>();
    private Level currentLevel;
    private int index = 0;
    private Wave currentWave;
    private string currentWaveName = "";
    private string currentWaveDescription = "";

    void Start()
    {
        foreach(GameObject g in GameObject.FindGameObjectsWithTag("Waypoint"))
        {
            waypoints.Add(g.transform.position);
        }
        ReadLevels();
        
        UpdateLevel();
    }

    void ReadLevels()
    {
        levelDict.Clear();
        levels.Clear();
        foreach (var v in Directory.GetFiles(Application.dataPath + "/Resources/Levels"))
        {
            if (v.Split('.')[v.Split('.').Length - 1] != "meta")
            {
                Level level = new Level();
                level.ReadXML(v);
                level.directory = v;
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
    }

    void UpdateLevel()
    {
        newLevelName = currentLevel.name;
        newEndGame = currentLevel.endGameMessage;
        newGameOver = currentLevel.gameOverMessage;
        newLevelDescription = currentLevel.description;
        currentWave = currentLevel.waveDict[currentLevel.wavesIds[0]];
        UpdateWave();
    }

    void UpdateWave()
    {
        foreach(GameObject g in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (g.GetComponent<Actor>() != null)
            {
                g.GetComponent<Actor>().Die();
            }
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
            if (GUI.Button(new Rect(0, Screen.height / 30 * i, scrollRect.width, Screen.height / 30), currentLevel.waveDict[currentLevel.wavesIds[i]].name))
            {
                currentWave = currentLevel.waveDict[currentLevel.wavesIds[i]];
                UpdateWave();
            }
        }
        GUI.EndScrollView();

        newLevelName = GUI.TextField(new Rect(Screen.width / 30 * 7.5f, Screen.height / 40, Screen.width / 5, Screen.height / 30), newLevelName);
        newEndGame = GUI.TextArea(new Rect(Screen.width / 2, Screen.height / 30 * 2, Screen.width / 4, Screen.height / 8), newEndGame);
        newGameOver = GUI.TextArea(new Rect(Screen.width / 4 * 3, Screen.height / 30 * 2, Screen.width / 4, Screen.height / 8), newGameOver);
        newLevelDescription = GUI.TextArea(new Rect(Screen.width / 4 * 3, Screen.height / 30 * 7, Screen.width / 4, Screen.height / 2), newLevelDescription);
        GUI.Box(new Rect(Screen.width / 4 * 3, Screen.height / 5, Screen.width / 4, Screen.height / 30), "Level Description");

        currentWaveName = GUI.TextField(new Rect(Screen.width / 30 * 7.5f, Screen.height / 40 * 35, Screen.width / 5, Screen.height / 30), currentWaveName);
        currentWaveDescription = GUI.TextArea(new Rect(Screen.width / 2, Screen.height / 30 * 25, Screen.width / 2, Screen.height / 8), currentWaveDescription);
        GUI.Box(new Rect(Screen.width / 2, Screen.height / 30 * 23.75f, Screen.width / 2, Screen.height / 30), "Wave Message");
        //GUI.Button(new Rect(Screen.width / 30 * 7.5f, Screen.height / 120 * 110, Screen.width / 10, Screen.height / 30), "Load Wave");
        if(GUI.Button(new Rect(Screen.width / 30 * 7.5f + Screen.width / 10, Screen.height / 120 * 110, Screen.width / 10, Screen.height / 30), "Save Wave"))
        {
            currentWave.description = currentWaveDescription;
            currentWave.name = currentWaveName;
            currentWave.Save();
        }
        if(GUI.Button(new Rect(Screen.width / 30 * 7.5f, Screen.height / 120 * 114, Screen.width / 10, Screen.height / 30), "Delete Wave"))
        {
            if (currentLevel.waveDict.Count > 1)
            {
                currentLevel.DeleteWave(currentWave);
                currentWave = currentLevel.waveDict[currentLevel.wavesIds[0]];
                UpdateLevel();
            }
        }
        if(GUI.Button(new Rect(Screen.width / 30 * 7.5f + Screen.width / 10, Screen.height / 120 * 114, Screen.width / 10, Screen.height / 30), "New Wave"))
        {
            currentLevel.CreateNewWave();
        }
        if (GUI.Button(new Rect(Screen.width / 30 * 7.5f + Screen.width / 10, Screen.height / 120 * 118, Screen.width / 10, Screen.height / 30), "Update Wave"))
        {
            UpdateWave();
        }
        if (GUI.Button(new Rect(Screen.width / 30 * 7.5f, Screen.height / 120 * 110, Screen.width / 10, Screen.height / 30), "Start / Stop"))
        {
            currentWave.ToggleEnemies();
        }

        GUI.Box(new Rect(Screen.width / 2, Screen.height / 40, Screen.width / 4, Screen.height / 30), "End Game Message");
        GUI.Box(new Rect(Screen.width / 4 * 3, Screen.height / 40, Screen.width / 4, Screen.height / 30), "Game Over Message");
        //GUI.Button(new Rect(Screen.width / 30 * 7.5f, Screen.height / 120 * 8, Screen.width / 10, Screen.height / 30), "Load Level");
        if(GUI.Button(new Rect(Screen.width / 30 * 7.5f + Screen.width / 10, Screen.height / 120 * 8, Screen.width / 10, Screen.height / 30), "Save Level"))
        {
            currentLevel.WriteXML();
            ReadLevels();
        }
        if(GUI.Button(new Rect(Screen.width / 30 * 7.5f, Screen.height / 30 * 2, Screen.width / 10, Screen.height / 30), "Delete Level"))
        {
            deleteLevelCheck = true;
        }
        if(deleteLevelCheck)
        {
            GUI.Box(new Rect(Screen.width / 30 * 7.5f, Screen.height / 30 * 3, Screen.width / 10, Screen.height / 30), "Are you sure?");
            if (GUI.Button(new Rect(Screen.width / 30 * 7.5f, Screen.height / 30 * 4, Screen.width / 20, Screen.height / 30), "No"))
            {
                deleteLevelCheck = false;
            }
            if (GUI.Button(new Rect(Screen.width / 30 * 7.5f + Screen.width / 20, Screen.height / 30 * 4, Screen.width / 20, Screen.height / 30), "Yes"))
            {
                File.Copy(currentLevel.directory, Application.dataPath + "/Resources/Levels/DeletedLevels/" + currentLevel.directory.Split('\\').Last(), true);
                File.Delete(currentLevel.directory);
                ReadLevels();
                deleteLevelCheck = false;
            }
        }
        if(GUI.Button(new Rect(Screen.width / 30 * 7.5f + Screen.width / 10, Screen.height / 10, Screen.width / 10, Screen.height / 30), "New Level"))
        {
            Level newLevel = new Level();
            newLevel.name = "New Level";
            for(int i = 0; i < 100; i++)
            {
                if(!levelDict.ContainsKey(newLevel.name + " (" + i.ToString() + ")"))
                {
                    newLevel.name = newLevel + " (" + i.ToString() + ")";
                    break;
                }
            }
            levelDict.Add(newLevel.name, newLevel);
            levels.Add(newLevel.name);
            newLevel.CreateNewWave();
            currentLevel = newLevel;
            UpdateLevel();
        }
        if(GUI.Button(new Rect(Screen.width / 30 * 7.5f + Screen.width / 10, Screen.height / 15 * 2, Screen.width / 10, Screen.height / 30), "Apply"))
        {
            Apply();
        }

        if (GUI.Button(new Rect(Screen.width / 8 * 5, Screen.height / 30 * 19, Screen.width / 8, Screen.height / 30), "Next"))
        {
            Next();
        }
        if (GUI.Button(new Rect(Screen.width / 2, Screen.height / 30 * 19, Screen.width / 8, Screen.height / 30), "Previous"))
        {
            Previous();
        }
        if (GUI.Button(new Rect(Screen.width / 2, Screen.height / 30 * 20, Screen.width / 4, Screen.height / 15), "Add"))
        {
            Add();
        }

        GUI.Box(new Rect(Screen.width / 2, Screen.height / 5, Screen.width / 4, Screen.height / 15), "Name: " + currentEnemy.name + "\nId:" + currentEnemy.id.ToString());
    }

    void Apply()
    {
        Level dummy = levelDict[currentLevel.name];
        dummy.name = newLevelName;
        dummy.endGameMessage = newEndGame;
        dummy.gameOverMessage = newGameOver;
        dummy.description = newLevelDescription;
        levelDict.Remove(currentLevel.name);
        levelDict.Add(dummy.name, dummy);
        currentLevel = dummy;
    }

    void Add()
    {
        currentPrefab.gameObject.transform.localScale = new Vector3(1, 1, 1);
        currentPrefab.transform.position = Vector2.zero;
        currentPrefab.tag = "Enemy";
        currentPrefab.AddComponent<MouseMove>();
        currentWave.enemies.Add(currentPrefab.GetComponent<Enemy>());
        EnemyInfo newInfo = new EnemyInfo();
        newInfo.info = TypeIdDict[typeInfoHolder.enemyIds[currentEnemyIndex]];
        newInfo.pos = currentPrefab.transform.position;
        newInfo.rot = currentPrefab.transform.rotation;
        currentWave.enemyInfoList.Add(newInfo);
        currentWave.enemyEnemyInfoDict.Add(currentPrefab.GetComponent<Enemy>(), newInfo);
        currentPrefab = null;
    }

    void Update()
    {
        if(currentPrefab == null)
        {
            changePrefab = true;
        }
        if(changePrefab)
        {
            changePrefab = false;
            if (currentPrefab != null)
            {
                Destroy(currentPrefab);
            }
            currentPrefab = GameObject.Instantiate(CurrentPrefab, new Vector3(33, 3, 0), Quaternion.Euler(new Vector3(0, 0, 180))) as GameObject;
            currentPrefab.GetComponent<Actor>().enabled = false;
            currentEnemy = TypeIdDict[typeInfoHolder.enemyIds[currentEnemyIndex]];
            currentPrefab.tag = "Untagged";
            currentPrefab.gameObject.transform.localScale = new Vector3(5, 5, 1);
        }
    }

    void Next()
    {
        if(currentEnemyIndex < typeInfoHolder.enemyIds.Count - 1)
        {
            currentEnemyIndex++;
        }
        else
        {
            currentEnemyIndex = 0;
        }
        changePrefab = true;
    }

    void Previous()
    {
        if (currentEnemyIndex < 1)
        {
            currentEnemyIndex = typeInfoHolder.enemyIds.Count - 1;
        }
        else
        {
            currentEnemyIndex--;
        }
        changePrefab = true;
    }
}
