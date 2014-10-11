using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System;

public class Wave
{
    public List<XmlNode> list = new List<XmlNode>();
	public string description = "";
	public string gameOverText = "";
    public string name;
    public List<Enemy> enemies = new List<Enemy>();
    public List<EnemyInfo> enemyInfoList = new List<EnemyInfo>();

    private Dictionary<int, TypeInfo> typeIdDict;
    public Dictionary<int, TypeInfo> TypeIdDict
    {
        get
        {
            if (typeIdDict == null)
            {
                typeIdDict = GameObject.FindObjectOfType<TypeInfoHolder>().typeInfoDict;
            }
            return typeIdDict;
        }
    }

    public void Add(XmlNode node)
	{
		list.Add(node);
        EnemyInfo ei = new EnemyInfo();
        ei.ReadXml(node);
        enemyInfoList.Add(ei);
	}

	public void SpawnAll(bool enabled = true, Spawner s = null)
	{
        enemies.Clear();
        //foreach (XmlNode node in list)
        //{
        //    TypeInfo t = TypeIdDict[int.Parse(node.Attributes["typeId"].Value)];
        //    GameObject enemyPrefab = Resources.Load(t.prefab) as GameObject;
        //    GameObject enemy = GameObject.Instantiate(enemyPrefab, StringToVector2(node.Attributes["pos"].Value), Quaternion.Euler(StringToVector3(node.Attributes["rot"].Value))) as GameObject;
        //    InitEnemy(t, enemy);
        //    enemies.Add(enemy.GetComponent<Enemy>());
        //    EnableEnemies(enabled);
        //    if (s != null)
        //    {
        //        s.enemyList.Add(enemy);
        //    }
        //}
        foreach (EnemyInfo i in enemyInfoList)
        {
            TypeInfo t = i.info;
            GameObject enemyPrefab = Resources.Load(t.prefab) as GameObject;
            GameObject enemy = GameObject.Instantiate(enemyPrefab, i.pos, i.rot) as GameObject;
            InitEnemy(t, enemy);
            enemies.Add(enemy.GetComponent<Enemy>());
            EnableEnemies(enabled);
            if (s != null)
            {
                s.enemyList.Add(enemy);
            }
        }
	}

    public void EnableEnemies(bool enable = true)
    {
        foreach(Enemy e in enemies)
        {
            e.enabled = enable;
            if (!e.enabled)
            {
                e.gameObject.rigidbody2D.velocity = Vector2.zero;
            }
        }
    }

    public void ToggleEnemies()
    {
        foreach(Enemy e in enemies)
        {
            e.enabled = !e.enabled;
            if(!e.enabled)
            {
                e.gameObject.rigidbody2D.velocity = Vector2.zero;
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
        e.maxHealth = t.maxHealth;
        e.energy = t.energy;
        e.maxEnergy = t.maxEnergy;
        e.moveSpeed = t.moveSpeed;
        e.hurtCooldown = t.hurtCooldown;
        e.engRegenSpeed = t.engRegenSpeed;
        e.dropProbability = t.dropProbability;
        e.inventory = new List<Item>();

        if(t.weapon != -1)
        {
            TypeInfo newWeapon = TypeIdDict[t.weapon];
            GameObject weaponPref = Resources.Load(newWeapon.prefab) as GameObject;
            GameObject weapon = GameObject.Instantiate(weaponPref, g.transform.position, g.transform.rotation) as GameObject;
            weapon.transform.parent = g.transform;
            weapon.GetComponent<Weapon>().ammoCount = (int)t.ammoCount;
        }

        if (t.inventoryItems != null)
        {
            foreach (int i in t.inventoryItems)
            {
                TypeInfo itemInfo = TypeIdDict[i];
                GameObject newItem = Resources.Load(itemInfo.prefab) as GameObject;
                e.inventory.Add(newItem.GetComponent<Item>());
            }
        }
    }
}
