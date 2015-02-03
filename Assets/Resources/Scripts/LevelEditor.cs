using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Linq;

public class BackgoundHolder
{
    public string path;
    public Texture2D texture;
    public int id;

    public BackgoundHolder()
    {

    }

    public BackgoundHolder(int _id)
    {
        id = _id;
    }

    public BackgoundHolder(string _path, int _id)
    {
        path = _path;
        id = _id;
        Texture2D tex = null;
        byte[] fileData;
        if (File.Exists(Application.dataPath + "/Resources/Backgrounds/" + path))
        {
            fileData = File.ReadAllBytes(Application.dataPath + "/Resources/Backgrounds/" + path);
            tex = new Texture2D(400, 300);
            tex.LoadImage(fileData);
            texture = tex;
        }
        else
        {
            Debug.LogError("No such background path: " + Application.dataPath + "/Resources/Backgrounds/" + path);
            fileData = File.ReadAllBytes(Application.dataPath + "/Resources/Backgrounds/black.jpg");
            tex = new Texture2D(400, 300);
            tex.LoadImage(fileData);
            texture = tex;
        }
    }

    //public void LoadTexture(string _path)
    //{
    //    path = _path;
    //    Texture2D tex = null;
    //    byte[] fileData;
    //    if (File.Exists(path))
    //    {
    //        fileData = File.ReadAllBytes(path);
    //        tex = new Texture2D(400, 300);
    //        tex.LoadImage(fileData);
    //        texture = tex;
    //    }
    //    else
    //    {
    //        Debug.LogError("No such background path: " + path);
    //        fileData = File.ReadAllBytes(Application.dataPath + "/Resources/Resources/Backgrounds/black.jpg");
    //        tex = new Texture2D(400, 300);
    //        tex.LoadImage(fileData);
    //        texture = tex;
    //    }
    //}
}

public class LevelEditor : MonoBehaviour
{
    public Vector2 scrollPosition = Vector2.zero;
    public Vector2 waveScrollPosition = Vector2.zero;
    public List<string> levels = new List<string>();
    public List<Vector3> waypoints = new List<Vector3>();
    private int currentEnemyIndex = 0;
    private int currentPlayerIndex = 0;
    private bool changePrefab = true;
    private bool changePlayerPrefab = true;
    private bool levelUpdated = true;
    private TypeInfo currentEnemy;
    private TypeInfo currentPlayer;
    private bool deleteLevelCheck = false;
    private bool lockWaypoints = true;
    private bool lockEnemies = false;
    private bool previousWaypointsLock = false;
    private bool previousEnemiesLock = true;
    private int backgroundIndex = 0;
    private int lifeCount = 5;
    private Dictionary<int, BackgoundHolder> pics;
    private TypeInfoHolder typeInfoHolder;
    private TypeInfoHolder TypeInfoHolder
    {
        get
        {
            if (typeInfoHolder == null)
            {
                typeInfoHolder = new TypeInfoHolder();
            }
            return typeInfoHolder;
        }
    }
    private Dictionary<int, TypeInfo> typeIdDict;
    public Dictionary<int, TypeInfo> TypeIdDict
    {
        get
        {
            if(typeIdDict == null)
            {
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
            return Resources.Load(TypeIdDict[TypeInfoHolder.enemyIds[currentEnemyIndex]].prefab) as GameObject;
        }
    }
    private GameObject currentPlayerPrefab;
    private GameObject CurrentPlayerPrefab
    {
        get
        {
            return Resources.Load(TypeIdDict[TypeInfoHolder.enemyIds[currentPlayerIndex]].prefab) as GameObject;
        }
    }

    private GameObject background;

    private Rect scrollRect = new Rect(Screen.width / 10, Screen.height / 40, Screen.width / 6, Screen.height / 7);
    private Rect waveScrollRect = new Rect(Screen.width / 15, Screen.height / 30 * 25, Screen.width / 6, Screen.height / 7);
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

    private GUIStyle button;
    private GUIStyle box;
    private GUIStyle textField;
    private GUIStyle toggle;
    //private GUIStyle textArea;
    //private GUIStyle scrollH;
    //private GUIStyle scrollV;
    private GUISkin skin;

    void Start()
    {
        if (GameObject.FindObjectOfType<Style>() != null)
        {
            skin = GameObject.FindObjectOfType<Style>().skin;
        }
        else
        {
            Debug.LogError("Style object is not found!");
            skin = new GUISkin();
        }
        //skin = new GUISkin();
        button = skin.customStyles[2];
        textField = skin.customStyles[3];
        box = skin.customStyles[4];
        toggle = skin.customStyles[5];
        //textArea = skin.textArea;
        //scrollH = skin.horizontalScrollbar;
        //scrollV = skin.verticalScrollbar;
        foreach(GameObject g in GameObject.FindGameObjectsWithTag("Waypoint"))
        {
            waypoints.Add(g.transform.position);
        }
        ReadLevels();
        ReadPics();
        background = GameObject.FindGameObjectWithTag("Background");
        UpdateLevel();
        SetTexture();
    }

    void ReadPics()
    {
        pics = new Dictionary<int, BackgoundHolder>();
        GameObject bg = GameObject.FindGameObjectWithTag("Background");
        int i = 0;
        foreach (var v in Directory.GetFiles(Application.dataPath + "/Resources/Backgrounds"))
        {
            if (v.Split('.').Last() == "jpg" || v.Split('.').Last() == "png" || v.Split('.').Last() == "bmp")
            {
                Texture2D tex = null;
                byte[] fileData;
                if (File.Exists(v))
                {
                    fileData = File.ReadAllBytes(v);
                    tex = new Texture2D(400, 300);
                    tex.LoadImage(fileData);
                    BackgoundHolder bh = new BackgoundHolder();
                    bh.id = i;
                    bh.path = v.Split('/').Last();
                    bh.texture = tex;
                    pics.Add(i, bh);
                    i++;
                }
            }
        }

    }

    public void InitWaypoints()
    {
        if (typeInfoHolder == null)
        {
            typeInfoHolder = new TypeInfoHolder();
        }
        typeInfoHolder.UpdateWaypoints();
    }

    void ReadLevels()
    {
        levelDict.Clear();
        levels.Clear();
        foreach (var v in Directory.GetFiles(Application.dataPath + "/Resources/Levels"))
        {
            if (v.Split('.').Last() != "meta")
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

    void UpdateLevel(bool readBorders = true)
    {
        newLevelName = currentLevel.name;
        newEndGame = currentLevel.endGameMessage;
        newGameOver = currentLevel.gameOverMessage;
        newLevelDescription = currentLevel.description;
        currentWave = currentLevel.waveDict[currentLevel.wavesIds[0]];
        currentPlayer = currentLevel.player;
        currentPlayerIndex = currentPlayer.id;
        lifeCount = currentLevel.lifeCount;
        changePlayerPrefab = true;
        levelUpdated = true;
        if (readBorders)
        {
            GameObject.FindGameObjectWithTag("LeftBorder").transform.position = currentLevel.leftBorder;
            GameObject.FindGameObjectWithTag("RightBorder").transform.position = currentLevel.rightBorder;
            GameObject.FindGameObjectWithTag("TopBorder").transform.position = currentLevel.topBorder;
            GameObject.FindGameObjectWithTag("BottomBorder").transform.position = currentLevel.bottomBorder;
        }
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
        SetTexture(currentWave.background);
        currentWaveDescription = currentWave.description;
        currentWave.SpawnAll(false);
        foreach(Actor a in GameObject.FindObjectsOfType<Actor>())
        {
            if (a.gameObject.GetComponent<MouseMove>() == null)
            {
                a.gameObject.AddComponent<MouseMove>();
            }
        }
        TypeInfoHolder.UpdateWaypoints();
    }

    void OnGUI()
    {
        if(GUI.Button(new Rect(20, Screen.height / 40, Screen.width / 15, Screen.height / 15), "Menu", button))
        {
            Application.LoadLevel("MainMenu");
        }
        scrollPosition = GUI.BeginScrollView(scrollRect, scrollPosition, new Rect(0, 0, Screen.width / 7, Screen.height / 30 * levels.Count));
        for (int i = 0; i < levels.Count; i++)
        {
            if (GUI.Button(new Rect(0, Screen.height / 30 * i, scrollRect.width, Screen.height / 30), levels[i], button))
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
            if (GUI.Button(new Rect(0, Screen.height / 30 * i, scrollRect.width, Screen.height / 30), currentLevel.waveDict[currentLevel.wavesIds[i]].name, button))
            {
                currentWave = currentLevel.waveDict[currentLevel.wavesIds[i]];
                UpdateWave();
            }
        }
        GUI.EndScrollView();

        newLevelName = GUI.TextField(new Rect(Screen.width / 30 * 8.5f, Screen.height / 40, Screen.width / 5, Screen.height / 30), newLevelName, 20, textField);
        newEndGame = GUI.TextArea(new Rect(Screen.width / 2, Screen.height / 30 * 2, Screen.width / 4, Screen.height / 8), newEndGame, textField);
        newGameOver = GUI.TextArea(new Rect(Screen.width / 4 * 3, Screen.height / 30 * 2, Screen.width / 4, Screen.height / 8), newGameOver, textField);
        newLevelDescription = GUI.TextArea(new Rect(Screen.width / 4 * 3, Screen.height / 30 * 7, Screen.width / 4, Screen.height / 4), newLevelDescription, textField);
        GUI.Box(new Rect(Screen.width / 4 * 3, Screen.height / 5, Screen.width / 4, Screen.height / 30), "Level Description", box);
        GUI.DrawTexture(new Rect(Screen.width / 4 * 3, Screen.height / 30 * 7 + Screen.height / 4, Screen.width / 4, Screen.height / 4), pics[backgroundIndex].texture, ScaleMode.ScaleToFit);

        if (GUI.Button(new Rect(Screen.width / 12 * 11, Screen.height / 30 * 7 + Screen.height / 2, Screen.width / 14, Screen.height / 30), "Next", button))
        {
            NextPic();
        }
        if (GUI.Button(new Rect(Screen.width / 4 * 3, Screen.height / 30 * 7 + Screen.height / 2, Screen.width / 14, Screen.height / 30), "Previous", button))
        {
            PreviousPic();
        }
        if (GUI.Button(new Rect(Screen.width / 12 * 10, Screen.height / 30 * 7 + Screen.height / 2, Screen.width / 14, Screen.height / 30), "Set", button))
        {
            SetTexture();
        }

        currentWaveName = GUI.TextField(new Rect(Screen.width / 30 * 8.5f, Screen.height / 30 * 25, Screen.width / 5, Screen.height / 30), currentWaveName, 20, textField);
        currentWaveDescription = GUI.TextArea(new Rect(Screen.width / 2, Screen.height / 30 * 25, Screen.width / 2, Screen.height / 8), currentWaveDescription, textField);
        GUI.Box(new Rect(Screen.width / 2, Screen.height / 30 * 23.75f, Screen.width / 2, Screen.height / 30), "Wave Message", box);
        if (GUI.Button(new Rect(Screen.width / 30 * 8.5f + Screen.width / 10, Screen.height / 30 * 26.5f, Screen.width / 10, Screen.height / 30), "Save Wave", button))
        {
            currentWave.description = currentWaveDescription;
            currentWave.name = currentWaveName;
            currentWave.Save();
            currentLevel.waveDict[currentWave.id] = currentWave;
        }
        if (GUI.Button(new Rect(Screen.width / 30 * 8.5f, Screen.height / 30 * 27.5f, Screen.width / 10, Screen.height / 30), "Delete Wave", button))
        {
            if (currentLevel.waveDict.Count > 1)
            {
                currentLevel.DeleteWave(currentWave);
                currentWave = currentLevel.waveDict[currentLevel.wavesIds[0]];
                UpdateLevel();
            }
        }
        if (GUI.Button(new Rect(Screen.width / 30 * 8.5f + Screen.width / 10, Screen.height / 30 * 27.5f, Screen.width / 10, Screen.height / 30), "New Wave", button))
        {
            currentLevel.CreateNewWave();
        }
        //if (GUI.Button(new Rect(Screen.width / 30 * 8.5f + Screen.width / 10, Screen.height / 120 * 118, Screen.width / 10, Screen.height / 30), "Update Wave"))
        //{
        //    UpdateWave();
        //}
        if (GUI.Button(new Rect(Screen.width / 30 * 8.5f, Screen.height / 30 * 26.5f, Screen.width / 10, Screen.height / 30), "Start / Stop", button))
        {
            currentWave.ToggleEnemies();
        }

        GUI.Box(new Rect(Screen.width / 2, Screen.height / 40, Screen.width / 4, Screen.height / 30), "End Game Message", box);
        GUI.Box(new Rect(Screen.width / 4 * 3, Screen.height / 40, Screen.width / 4, Screen.height / 30), "Game Over Message", box);
        if (GUI.Button(new Rect(Screen.width / 30 * 8.5f + Screen.width / 10, Screen.height / 30 * 2, Screen.width / 10, Screen.height / 30), "Save Level", button))
        {
            currentLevel.WriteXML(currentPlayer);
            ReadLevels();
        }
        if (GUI.Button(new Rect(Screen.width / 30 * 8.5f, Screen.height / 30 * 2, Screen.width / 10, Screen.height / 30), "Delete Level", button))
        {
            if(levelDict.Count > 1)
            {
                deleteLevelCheck = true;
            }
        }
        if(deleteLevelCheck)
        {
            GUI.Box(new Rect(Screen.width / 30 * 8.5f, Screen.height / 30 * 3, Screen.width / 10, Screen.height / 30), "Are you sure?", box);
            if (GUI.Button(new Rect(Screen.width / 30 * 8.5f, Screen.height / 30 * 4, Screen.width / 20, Screen.height / 30), "No", button))
            {
                deleteLevelCheck = false;
            }
            if (GUI.Button(new Rect(Screen.width / 30 * 8.5f + Screen.width / 20, Screen.height / 30 * 4, Screen.width / 20, Screen.height / 30), "Yes", button))
            {
                File.Copy(currentLevel.directory, Application.dataPath + "/Resources/Levels/DeletedLevels/" + currentLevel.directory.Split('\\').Last(), true);
                File.Delete(currentLevel.directory);
                ReadLevels();
                deleteLevelCheck = false;
            }
        }
        if (GUI.Button(new Rect(Screen.width / 30 * 8.5f + Screen.width / 10, Screen.height / 10, Screen.width / 10, Screen.height / 30), "New Level", button))
        {
            Level newLevel = new Level();
            newLevel.name = "New Level";
            for(int i = 0; i < 100; i++)
            {
                if(!levelDict.ContainsKey(newLevel.name + " (" + i.ToString() + ")"))
                {
                    newLevel.name = newLevel.name + " (" + i.ToString() + ")";
                    break;
                }
            }
            levelDict.Add(newLevel.name, newLevel);
            levels.Add(newLevel.name);
            newLevel.CreateNewWave();
            newLevel.waveDict[0].background = currentWave.background;
            currentLevel = newLevel;
            newLevel.player = currentPlayer;
            UpdateLevel(false);
        }
        if (GUI.Button(new Rect(Screen.width / 30 * 8.5f + Screen.width / 10, Screen.height / 15 * 2, Screen.width / 10, Screen.height / 30), "Apply", button))
        {
            Apply();
        }

        if (GUI.Button(new Rect(Screen.width / 8 * 5, Screen.height / 30 * 11, Screen.width / 8, Screen.height / 30), "Next", button))
        {
            Next();
        }
        if (GUI.Button(new Rect(Screen.width / 2, Screen.height / 30 * 11, Screen.width / 8, Screen.height / 30), "Previous", button))
        {
            Previous();
        }
        if (GUI.Button(new Rect(Screen.width / 2, Screen.height / 30 * 12, Screen.width / 4, Screen.height / 30), "Add", button))
        {
            Add();
        }

        GUI.Box(new Rect(Screen.width / 8 * 5f, Screen.height / 30 * 15f, Screen.width / 16, Screen.height / 30), "Life:", box);
        try
        {
            lifeCount = int.Parse(GUI.TextField(new Rect(Screen.width / 8 * 5.5f, Screen.height / 30 * 15f, Screen.width / 16, Screen.height / 30), lifeCount.ToString(), 3, textField));
        }
        catch
        {
            lifeCount = 0;
        }

        if (GUI.Button(new Rect(Screen.width / 8 * 4.5f, Screen.height / 30 * 17.5f, Screen.width / 16, Screen.height / 30), "Next", button))
        {
            NextPlayer();
        }
        if (GUI.Button(new Rect(Screen.width / 8 * 4, Screen.height / 30 * 17.5f, Screen.width / 16, Screen.height / 30), "Previous", button))
        {
            PreviousPlayer();
        }
        if (GUI.Button(new Rect(Screen.width / 8 * 4, Screen.height / 30 * 18.5f, Screen.width / 8, Screen.height / 30), "Set", button))
        {
            SetPlayer();
        }

        lockEnemies = GUI.Toggle(new Rect(Screen.width / 2, Screen.height / 30 * 21.5f, Screen.width / 8, Screen.height / 30), lockEnemies, "Lock Enemies Positions");
        lockWaypoints = GUI.Toggle(new Rect(Screen.width / 2, Screen.height / 30 * 22.5f, Screen.width / 8, Screen.height / 30), lockWaypoints, "Lock Waypoints Positions");
        if (GUI.Button(new Rect(new Rect(Screen.width / 8 * 5, Screen.height / 30 * 21, Screen.width / 8, Screen.height / 30)), "Add Waypoint", button))
        {
            AddWaypoint();
        }
        GUI.Box(new Rect(Screen.width / 2, Screen.height / 5, Screen.width / 4, Screen.height / 15), "Name: " + currentEnemy.name + "\nId:" + currentEnemy.id.ToString(), box);
        GUI.Box(new Rect(Screen.width / 2, Screen.height / 30 * 13, Screen.width / 4, Screen.height / 15), "Player Name: " + currentPlayer.name + "\nPlayer Id: " + currentPlayer.id.ToString(), box);
    }

    void NextPic()
    {
        if (backgroundIndex < pics.Count - 1)
        {
            backgroundIndex++;
        }
        else
        {
            backgroundIndex = 0;
        }
    }

    void PreviousPic()
    {
        if (backgroundIndex < 1)
        {
            backgroundIndex = pics.Count - 1;
        }
        else
        {
            backgroundIndex--;
        }
    }

    void SetTexture(BackgoundHolder bg = null)
    {
        if (bg == null)
        {
            currentLevel.waveDict[currentWave.id].background = pics[backgroundIndex];
            background.renderer.material.mainTexture = pics[backgroundIndex].texture;
        }
        else
        {
            background.renderer.material.mainTexture = bg.texture;
        }

    }

    void AddWaypoint()
    {
        GameObject wp = Instantiate(Resources.Load("Prefabs/LevelAssets/Waypoint") as GameObject, Vector3.zero, Quaternion.identity) as GameObject;
        wp.AddComponent<CircleCollider2D>();
        wp.collider2D.isTrigger = true;
        wp.AddComponent<MouseMove>();
        wp.GetComponent<MouseMove>().enabled = !lockWaypoints;
        currentWave.waypoints.Add(wp);
        currentWave.waypointsPos.Add(wp.transform.position);
    }

    void Apply()
    {
        Level dummy = levelDict[currentLevel.name];
        dummy.name = newLevelName;
        dummy.endGameMessage = newEndGame;
        dummy.gameOverMessage = newGameOver;
        dummy.description = newLevelDescription;
        dummy.lifeCount = lifeCount;
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
        currentPrefab.GetComponent<MouseMove>().enabled = !lockEnemies;
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
            currentPrefab = GameObject.Instantiate(CurrentPrefab, new Vector3(33, 9.5f, 0), Quaternion.Euler(new Vector3(0, 0, 180))) as GameObject;
            currentPrefab.GetComponent<Actor>().enabled = false;
            currentEnemy = TypeIdDict[typeInfoHolder.enemyIds[currentEnemyIndex]];
            currentPrefab.tag = "Untagged";
            //currentPrefab.gameObject.transform.localScale = new Vector3(5, 5, 1);
        }
        if(currentPlayerPrefab == null)
        {
            changePlayerPrefab = true;
        }
        if(changePlayerPrefab)
        {
            changePlayerPrefab = false;
            if (currentPlayerPrefab != null)
            {
                Destroy(currentPlayerPrefab);
            }
            currentPlayerPrefab = GameObject.Instantiate(CurrentPlayerPrefab, new Vector3(27, -1.5f, 0), Quaternion.Euler(new Vector3(0, 0, 0))) as GameObject;
            currentPlayerPrefab.GetComponent<Actor>().enabled = false;
            currentPlayerPrefab.tag = "Untagged";
        }
        if(previousEnemiesLock != lockEnemies)
        {
            previousEnemiesLock = lockEnemies;
            foreach(GameObject g in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                if (g.GetComponent<MouseMove>() == null)
                {
                    g.AddComponent<MouseMove>();
                }
                g.GetComponent<MouseMove>().enabled = !lockEnemies;
            }
        }
        if (previousWaypointsLock != lockWaypoints)
        {
            previousWaypointsLock = lockWaypoints;
            foreach (GameObject g in GameObject.FindGameObjectsWithTag("Waypoint"))
            {
                if (g.GetComponent<MouseMove>() == null)
                {
                    g.AddComponent<MouseMove>();
                }
                if(g.GetComponent<CircleCollider2D>() == null)
                {
                    g.AddComponent<CircleCollider2D>();
                }
                g.GetComponent<MouseMove>().enabled = !lockWaypoints;
            }
        }
    }

    void LateUpdate()
    {
        if(levelUpdated)
        {
            levelUpdated = false;
            SetPlayer();
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

    void NextPlayer()
    {
        if (currentPlayerIndex < typeInfoHolder.enemyIds.Count - 1)
        {
            currentPlayerIndex++;
        }
        else
        {
            currentPlayerIndex = 0;
        }
        changePlayerPrefab = true;
    }

    void PreviousPlayer()
    {
        if (currentPlayerIndex < 1)
        {
            currentPlayerIndex = typeInfoHolder.enemyIds.Count - 1;
        }
        else
        {
            currentPlayerIndex--;
        }
        changePlayerPrefab = true;
    }

    void SetPlayer()
    {
        GameObject currentPlayerObject = GameObject.FindObjectOfType<Player>().gameObject;
        Vector3 playerPos = currentPlayerObject.transform.position;
        Destroy(currentPlayerObject);
        currentPlayerPrefab.transform.position = playerPos;
        currentPlayerPrefab.tag = "Player";
        Destroy(currentPlayerPrefab.GetComponent<Enemy>());
        currentPlayerPrefab.AddComponent<Player>().enabled = false;
        currentPlayerPrefab.AddComponent<MouseMove>();
        currentPlayerPrefab.name = "Player";
        currentPlayerPrefab = null;
        changePlayerPrefab = true;
        currentPlayer = TypeIdDict[typeInfoHolder.enemyIds[currentPlayerIndex]];
    }
}
