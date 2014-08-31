﻿using UnityEngine;
using System.Collections;
using System.Xml;
using System;

public class GameManager : MonoBehaviour {

	public int playerLife = 5;
	public string levelName;
	public string levelDescription;

	private Spawner spawner;

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

	public string spawnerPrefab = "Prefabs/Spawner";
	private GameObject spawnerGO
	{
		get { return Resources.Load(spawnerPrefab) as GameObject; }
	}

	void Start()
	{
		BeginLevel();
		ReadXml(Application.dataPath + "/Resources/Levels/level.xml");
	}

	private void BeginLevel()
	{
		GameObject s = GameObject.Instantiate(spawnerGO, new Vector2(100, 100), Quaternion.identity) as GameObject;
		spawner = s.GetComponent<Spawner>();
		spawner.gameManager = this as GameManager;
		spawner.playerPrefab = PlayerPrefab;
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
		}
		else if(node.Name == "Wave")
		{
			int waveNumber = int.Parse(node.Attributes["val"].Value);
			Wave w = new Wave();
			w.gm = this;
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