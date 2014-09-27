using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class Wave
{
    public List<XmlNode> list = new List<XmlNode>();
	public GameManager gm;
	public string description = "";
	public string gameOverText = "";
	private bool showDescription = false;

    public void Add(XmlNode node)
	{
		list.Add(node);
	}

	public void SpawnAll(Spawner s = null)
	{
        foreach (XmlNode node in list)
		{
            TypeInfo t = gm.typeIdDict[int.Parse(node.Attributes["typeId"].Value)];
            GameObject enemyPrefab = Resources.Load(t.prefab) as GameObject;
            try
            {
                InitEnemy(t, enemyPrefab);
                GameObject enemy = GameObject.Instantiate(enemyPrefab, gm.StringToVector2(node.Attributes["pos"].Value), Quaternion.Euler(gm.StringToVector3(node.Attributes["rot"].Value))) as GameObject;
                if (s != null)
                {
                    s.enemyList.Add(enemy);
                }
            }
            catch
            {
                Debug.Log("prefab path: " + t.prefab);
                Debug.Log("prefab: " + enemyPrefab);
            }
		}
	}

    void InitEnemy(TypeInfo t, GameObject g)
    {
        Enemy e = g.GetComponent<Enemy>();
        e.name = t.name;
        e.selfDestroyProbability = t.selfDestroyProbability;
        e.moveToWaypoint = t.moveToWaypoint;
        e.amplitudeX = t.amplitudeX;
        e.amplitudeY = t.amplitudeY;
        e.shootProbability = t.shootProbability;
        e.dampingX = t.dampingX;
        e.dampingY = t.dampingY;
        e.health = t.health;
        e.maxHealth = t.maxHealt;
        e.energy = t.energy;
        e.maxEnergy = t.maxEnergy;
        e.moveSpeed = t.moveSpeed;
        e.hurtCooldown = t.hurtCooldown;
        e.engRegenSpeed = t.energyRegenSpeed;
        e.dropProbability = t.dropProbability;
    }
}
