using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class Wave
{
	public List<XmlNode> list = new List<XmlNode>();
	public GameManager gm;
	public string description = "";
	private bool showDescription = false;

	public void Add(XmlNode e)
	{
		list.Add(e);
	}

	public void SpawnAll(Spawner s = null)
	{
		foreach(XmlNode node in list)
		{
			GameObject enemyPrefab = Resources.Load("Prefabs/Enemies/" + node.Attributes["prefabName"].Value) as GameObject;
			GameObject enemy = GameObject.Instantiate(enemyPrefab, gm.StringToVector2(node.Attributes["pos"].Value), Quaternion.Euler (gm.StringToVector3(node.Attributes["rot"].Value))) as GameObject;
			if(s != null)
			{
				s.enemyList.Add(enemy);
			}
		}
	}
}
