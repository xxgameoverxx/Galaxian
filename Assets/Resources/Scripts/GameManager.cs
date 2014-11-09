using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System;

public class GameManager : MonoBehaviour {

	public int playerLife = 5;
	public string levelName;
	public string levelDescription;
	public string winText;
    public string gameOverText;
    private string levelNameToLoad;
    private List<Vector3> waypoints;
	private Spawner spawner;
    private TypeInfo typeInfo;
    private GUIStyle button;
    private GUISkin skin;

    public List<Vector3> Waypoints
    {
        get
        {
            if(waypoints == null)
            {
                if(typeInfoHolder == null)
                {
                    typeInfoHolder = new TypeInfoHolder();
                }
                waypoints = typeInfoHolder.waypoints;
            }
            return typeInfoHolder.waypoints;
        }
    }

    private GameObject respawnPos;
    public Vector2 RespawnPos
    {
        get
        {
            if (respawnPos == null)
            {
                respawnPos = GameObject.FindGameObjectWithTag("Respawn");
            }
            return respawnPos.transform.position;
        }
        set
        {
            if (respawnPos == null)
            {
                respawnPos = GameObject.FindGameObjectWithTag("Respawn");
            }
            respawnPos.transform.position = value;
        }
    }

    private Transform leftBorder;
    public Transform LeftBorder
    {
        get
        {
            if (leftBorder == null)
            {
                leftBorder = GameObject.FindGameObjectWithTag("LeftBorder").transform;
            }
            return leftBorder;
        }
    }
    private Transform rightBorder;
    public Transform RightBorder
    {
        get
        {
            if (rightBorder == null)
            {
                rightBorder = GameObject.FindGameObjectWithTag("RightBorder").transform;
            }
            return rightBorder;
        }
    }
    private Transform topBorder;
    public Transform TopBorder
    {
        get
        {
            if (topBorder == null)
            {
                topBorder = GameObject.FindGameObjectWithTag("TopBorder").transform;
            }
            return topBorder;
        }
    }
    private Transform bottomBorder;
    public Transform BottomBorder
    {
        get
        {
            if (bottomBorder == null)
            {
                bottomBorder = GameObject.FindGameObjectWithTag("BottomBorder").transform;
            }
            return bottomBorder;
        }
    }

	private string playerPrefab;
	public string PlayerPrefab
	{
		get
		{
			if(playerPrefab == null)
			{
				playerPrefab = "Prefabs/Player";
				Debug.LogWarning("Player prefab path is set to default value: Prefabs/Player by " + this);
			}
			return playerPrefab;
		}
		set { playerPrefab = value; }
	}

    private TypeInfoHolder typeInfoHolder;
    private TypeInfoHolder TypeInfoHolder
    {
        get
        {
            if(typeInfoHolder == null)
            {
                typeInfoHolder = new TypeInfoHolder();
            }
            return typeInfoHolder;
        }
    }

    private Dictionary<int, TypeInfo> typeIdDict;
    private Dictionary<int, TypeInfo> TypeIdDict
    {
        get
        {
            if (typeIdDict == null)
            {
                typeIdDict = TypeInfoHolder.typeInfoDict;
            }
            return typeIdDict;
        }
    }

	public string spawnerPrefab = "Prefabs/Spawner";
	private GameObject spawnerGO
	{
		get { return Resources.Load(spawnerPrefab) as GameObject; }
	}

    public void InitWaypoints()
    {
        TypeInfoHolder.UpdateWaypoints();
        //waypoints = new List<Vector3>();
        //foreach (GameObject g in GameObject.FindGameObjectsWithTag("Waypoint"))
        //{
        //    waypoints.Add(g.gameObject.transform.position);
        //}
    }

    void OnGUI()
    {
        if(GUI.Button(new Rect(Screen.width / 10 * 9, Screen.height / 60, Screen.width / 11, Screen.height / 30), "Main Menu", button))
        {
            Application.LoadLevel("MainMenu");
        }
    }

    //void Awake()
    //{
    //}

    void Awake()
    {
        levelNameToLoad = GameObject.FindObjectOfType<LevelSelector>().selectedLevel.name;
        BeginLevel();
        InitWaypoints();
        ReadXml(Application.dataPath + "/Resources/Levels/" + levelNameToLoad + ".xml");
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

	private void BeginLevel()
	{
		GameObject s = GameObject.Instantiate(spawnerGO, new Vector2(100, 100), Quaternion.identity) as GameObject;
		spawner = s.GetComponent<Spawner>();
		spawner.gameManager = this as GameManager;
		spawner.playerPrefab = PlayerPrefab;
        InitWaypoints();
	}

	public void ReadXml(string path)
	{
		XmlDocument doc = new XmlDocument();
		doc.Load(path);
		foreach(XmlNode node in doc)
		{
			ReadNode(node);
		}
        
	}

	private void ReadNode(XmlNode node, XmlNode parentNode = null)
	{
		if(node.Name == "Level")
		{
			levelName = node.Attributes["name"].Value;
            LeftBorder.transform.position = StringToVector2(node.Attributes["leftBorder"].Value);
            RightBorder.transform.position = StringToVector2(node.Attributes["rightBorder"].Value);
            TopBorder.transform.position = StringToVector2(node.Attributes["topBorder"].Value);
            BottomBorder.transform.position = StringToVector2(node.Attributes["bottomBorder"].Value);
            RespawnPos = StringToVector3(node.Attributes["respawnPos"].Value);
            int playerId = int.Parse(node.Attributes["player"].Value);
            playerPrefab = TypeIdDict[playerId].prefab;
            spawner.playerInfo = TypeIdDict[playerId];
            spawner.playerPrefab = playerPrefab;
            playerLife = int.Parse(node.Attributes["life"].Value);
        }
		else if(node.Name == "Wave")
		{
			int waveNumber = int.Parse(node.Attributes["val"].Value);
			Wave w = new Wave();
            w.background = new BackgoundHolder(node.Attributes["background"].Value, int.Parse(node.Attributes["backgroundId"].Value));
			spawner.waves.Insert(waveNumber, w);
		}
		else if(node.Name == "Enemy")
		{
			int waveNumber = int.Parse(parentNode.Attributes["val"].Value);
            spawner.waves[waveNumber].Add(node);
		}
		else if(node.Name == "Description")
		{
			if(node.Attributes["tag"].Value == "level")
			{
				levelDescription = node.InnerText;
			}
			else if(node.Attributes["tag"].Value == "wave")
			{
				int waveNumber = int.Parse(parentNode.Attributes["val"].Value);
				spawner.waves[waveNumber].description = node.InnerText;
			}
		}
        else if (node.Name == "EndGameText")
		{
            //if(node.Attributes["tag"].Value == "level")
            //{
				winText = node.InnerText;
            //}
            //else if(node.Attributes["tag"].Value == "wave")
            //{
            //    int waveNumber = int.Parse(parentNode.Attributes["val"].Value);
            //    spawner.waves[waveNumber].gameOverText = node.InnerText;
            //}
		}
        else if (node.Name == "GameOverText")
        {
            gameOverText = node.InnerText;
        }
        else if(node.Name == "Waypoint")
        {
            int waveNumber = int.Parse(parentNode.Attributes["val"].Value);
            spawner.waves[waveNumber].waypointsPos.Add(StringToVector3(node.Attributes["pos"].Value));
        }

		foreach(XmlNode childNode in node.ChildNodes)
		{
			ReadNode(childNode, node);
		}
	}

	private GameObject CreateObjectFromNode(XmlNode node)
	{
		GameObject enemy = new GameObject(node.Attributes["name"].Value);
		enemy = Resources.Load("Prefabs/Enemies/" + node.Attributes["prefabName"].Value, typeof(GameObject)) as GameObject;
		return enemy;
	}
	
	public Vector2 StringToVector2(string s)
	{
		float x;
		float y;
		string newS = s.Trim( new Char[] { '(', ')'} );
		x = float.Parse(newS.Split( new Char[] {','})[0]);
		y = float.Parse(newS.Split( new Char[] {','})[1]);
		return new Vector2(x, y);
	}

	public Vector3 StringToVector3(string s)
	{
		float x;
		float y;
		float z;
		string newS = s.Trim( new Char[] { '(', ')'} );
		x = float.Parse(newS.Split( new Char[] {','})[0]);
		y = float.Parse(newS.Split( new Char[] {','})[1]);
		z = float.Parse(newS.Split( new char[] {','})[2]);
		return new Vector3(x, y, z);
	}
}
