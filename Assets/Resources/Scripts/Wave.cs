using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System;

public class Wave
{
    public int id;
	public string description = "";
    //public string gameOverText = "";
    public string name;
    public List<Enemy> enemies = new List<Enemy>();
    public List<EnemyInfo> enemyInfoList = new List<EnemyInfo>();
    public Dictionary<Enemy, EnemyInfo> enemyEnemyInfoDict = new Dictionary<Enemy, EnemyInfo>();

    private TypeInfoHolder typeInfoHolder;
    private Dictionary<int, TypeInfo> typeIdDict;
    public Dictionary<int, TypeInfo> TypeIdDict
    {
        get
        {
            if (typeInfoHolder == null)
            {
                typeInfoHolder = new TypeInfoHolder();
                typeIdDict = typeInfoHolder.typeInfoDict;
            }
            return typeIdDict;
        }
    }

    public void Add(XmlNode node)
	{
        EnemyInfo ei = new EnemyInfo();
        ei.ReadXml(node);
        enemyInfoList.Add(ei);
	}

	public void SpawnAll(bool enabled = true, Spawner s = null)
	{
        if(enemyInfoList.Count == 0)
        {
            CreateBasicEnemy();
        }
        enemies.Clear();
        enemyEnemyInfoDict.Clear();
        foreach (EnemyInfo i in enemyInfoList)
        {
            TypeInfo t = i.info;
            GameObject enemyPrefab = Resources.Load(t.prefab) as GameObject;
            GameObject enemyGO = GameObject.Instantiate(enemyPrefab, i.pos, i.rot) as GameObject;
            Enemy enemy = enemyGO.GetComponent<Enemy>();
            InitEnemy(t, enemy);
            enemies.Add(enemy);
            enemyEnemyInfoDict.Add(enemy, i);
            EnableEnemies(enabled);
            if (s != null)
            {
                s.enemyList.Add(enemyGO);
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

    public void Save()
    {
        List<Enemy> enemiesToRemove = new List<Enemy>();
        foreach(Enemy e in enemies)
        {
            if(e == null)
            {
                enemiesToRemove.Add(e);
                continue;
            }
            enemyEnemyInfoDict[e].pos = e.gameObject.transform.position;
            enemyEnemyInfoDict[e].rot = e.gameObject.transform.rotation;
            
        }
        foreach(Enemy e in enemiesToRemove)
        {
            enemies.Remove(e);
            enemyInfoList.Remove(enemyEnemyInfoDict[e]);
            enemyEnemyInfoDict.Remove(e);
        }
    }

    void CreateBasicEnemy()
    {
        EnemyInfo ei = new EnemyInfo();
        for (int i = 0; i < TypeIdDict.Count; i++)
        {
            if (TypeIdDict.ContainsKey(i))
            {
                ei.info = TypeIdDict[i];
                break;
            }
        }
        ei.pos = Vector2.zero;
        ei.rot = Quaternion.Euler(0, 0, 180);
        enemyInfoList.Add(ei);
    }

    void InitEnemy(TypeInfo t, Enemy e)
    {
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
            GameObject weapon = GameObject.Instantiate(weaponPref, e.transform.position, e.transform.rotation) as GameObject;
            weapon.transform.parent = e.transform;
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
