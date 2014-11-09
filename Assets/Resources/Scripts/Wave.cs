using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System;
using System.IO;

public class Wave
{
    public int id;
	public string description = "";
    public string name;
    public BackgoundHolder background;
    public List<GameObject> waypoints = new List<GameObject>();
    public List<Vector3> waypointsPos = new List<Vector3>();
    public List<Enemy> enemies = new List<Enemy>();
    public List<EnemyInfo> enemyInfoList = new List<EnemyInfo>();
    public Dictionary<Enemy, EnemyInfo> enemyEnemyInfoDict = new Dictionary<Enemy, EnemyInfo>();
    private GameManager gm;
    private LevelEditor le;

    public Wave()
    {
        gm = GameObject.FindObjectOfType<GameManager>();
        le = GameObject.FindObjectOfType<LevelEditor>();
    }

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
        RemoveWaypoints();
        InstantiateWaypoints();
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
        SetBackground();
	}

    private void SetBackground()
    {
        Texture2D tex = null;
        byte[] fileData;
        if (File.Exists(background.path))
        {
            fileData = File.ReadAllBytes(background.path);
            tex = new Texture2D(400, 300);
            tex.LoadImage(fileData);
            GameObject.FindGameObjectWithTag("Background").renderer.material.mainTexture = tex;
        }
    }

    private void RemoveWaypoints()
    {
        foreach(GameObject g in GameObject.FindGameObjectsWithTag("Waypoint"))
        {
            GameObject.Destroy(g);
        }
    }
    private void InstantiateWaypoints()
    {
        foreach(Vector3 g in waypointsPos)
        {
            GameObject wp = GameObject.Instantiate(Resources.Load("Prefabs/LevelAssets/Waypoint"), g, Quaternion.identity) as GameObject;
            if(gm != null)
            {
                wp.renderer.enabled = false;
            }
        }
        if(gm != null)
        {
            gm.InitWaypoints();

        }
        else if(le != null)
        {
            le.InitWaypoints();
        }
        else
        {
            Debug.LogError("Neither Game Manger nor Level editor was found. Something is wrong!");
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
        enemyInfoList.Clear();
        foreach(Enemy e in enemies)
        {
            if(e == null)
            {
                enemiesToRemove.Add(e);
                enemyEnemyInfoDict.Remove(e);
                continue;
            }
            enemyEnemyInfoDict[e].pos = e.gameObject.transform.position;
            enemyEnemyInfoDict[e].rot = e.gameObject.transform.rotation;
            enemyInfoList.Add(enemyEnemyInfoDict[e]);
        }
        waypointsPos.Clear();
        foreach(GameObject g in GameObject.FindGameObjectsWithTag("Waypoint"))
        {
            if (!waypoints.Contains(g))
            {
                waypoints.Add(g);
            }
            if(!waypointsPos.Contains(g.transform.position))
            {
                waypointsPos.Add(g.transform.position);
            }
        }
        foreach(Enemy e in enemiesToRemove)
        {
            enemies.Remove(e);
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
        e.spread = t.spread;
        e.bulletSpeed = t.bulletSpeed;
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
            foreach (int i in t.inventoryItems.Keys)
            {
                TypeInfo itemInfo = TypeIdDict[i];
                GameObject newItem = Resources.Load(itemInfo.prefab) as GameObject;
                newItem.GetComponent<Item>().durability = t.inventoryItems[i];
                e.inventory.Add(newItem.GetComponent<Item>());
            }
        }
    }
}
