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
	}

	public void SpawnAll(bool enabled = true, Spawner s = null)
	{
        enemies.Clear();
        foreach (XmlNode node in list)
		{
            TypeInfo t = TypeIdDict[int.Parse(node.Attributes["typeId"].Value)];
            GameObject enemyPrefab = Resources.Load(t.prefab) as GameObject;
            GameObject enemy = GameObject.Instantiate(enemyPrefab, StringToVector2(node.Attributes["pos"].Value), Quaternion.Euler(StringToVector3(node.Attributes["rot"].Value))) as GameObject;
            InitEnemy(t, enemy, node);
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
        }
    }

    public void ToggleEnemies()
    {
        foreach(Enemy e in enemies)
        {
            e.enabled = !e.enabled;
        }
    }

    public Vector2 StringToVector2(string s)
    {
        float x;
        float y;
        string newS = s.Trim(new Char[] { '(', ')' });
        x = float.Parse(newS.Split(new Char[] { ',' })[0]);
        y = float.Parse(newS.Split(new Char[] { ',' })[1]);
        return new Vector2(x, y);
    }

    public Vector3 StringToVector3(string s)
    {
        float x;
        float y;
        float z;
        string newS = s.Trim(new Char[] { '(', ')' });
        x = float.Parse(newS.Split(new Char[] { ',' })[0]);
        y = float.Parse(newS.Split(new Char[] { ',' })[1]);
        z = float.Parse(newS.Split(new char[] { ',' })[2]);
        return new Vector3(x, y, z);
    }

    void InitEnemy(TypeInfo t, GameObject g, XmlNode n)
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
        foreach (XmlNode xnode in n.ChildNodes)
        {
            if (xnode.Name == "SelfDestroyProbability")
            {
                e.selfDestroyProbability = float.Parse(xnode.Attributes["value"].Value);
            }
            else if (xnode.Name == "MoveToWaypoint")
            {
                e.moveToWaypoint = bool.Parse(xnode.Attributes["value"].Value);
            }
            else if (xnode.Name == "Amplitude")
            {
                e.amplitudeX = float.Parse(xnode.Attributes["x"].Value);
                e.amplitudeY = float.Parse(xnode.Attributes["y"].Value);
            }
            else if (xnode.Name == "ShootProbability")
            {
                e.shootProbability = float.Parse(xnode.Attributes["value"].Value);
            }
            else if (xnode.Name == "Damping")
            {
                e.dampingX = float.Parse(xnode.Attributes["x"].Value);
                e.dampingY = float.Parse(xnode.Attributes["y"].Value);
            }
            else if (xnode.Name == "Health")
            {
                e.health = float.Parse(xnode.Attributes["value"].Value);
                e.maxHealth = float.Parse(xnode.Attributes["maxValue"].Value);
            }
            else if (xnode.Name == "Energy")
            {
                e.energy = float.Parse(xnode.Attributes["value"].Value);
                e.maxEnergy = float.Parse(xnode.Attributes["maxValue"].Value);
                e.engRegenSpeed = float.Parse(xnode.Attributes["regenSpeed"].Value);
            }
            else if (xnode.Name == "MoveSpeed")
            {
                e.moveSpeed = float.Parse(xnode.Attributes["value"].Value);
            }
            else if (xnode.Name == "HurtCooldown")
            {
                e.hurtCooldown = float.Parse(xnode.Attributes["value"].Value);
            }
            else if (xnode.Name == "DropProbability")
            {
                e.dropProbability = float.Parse(xnode.Attributes["value"].Value);
            }
            else if (xnode.Name == "InventoryItem")
            {
                GameObject newItem = Resources.Load(TypeIdDict[int.Parse(xnode.Attributes["value"].Value)].prefab) as GameObject;
                e.inventory.Add(newItem.GetComponent<Item>());
            }
            else if (xnode.Name == "Weapon")
            {
                TypeInfo newWeapon = TypeIdDict[int.Parse(xnode.Attributes["value"].Value)];
                GameObject weaponPref = Resources.Load(newWeapon.prefab) as GameObject;
                GameObject weapon = GameObject.Instantiate(weaponPref, g.transform.position, g.transform.rotation) as GameObject;
                weapon.transform.parent = g.transform;
                weapon.GetComponent<Weapon>().ammoCount = int.Parse(xnode.Attributes["ammoCount"].Value);
            }
            else
            {
                Debug.LogError("Unknown node: " + xnode.Name + ". Child of " + n.Name);
            }
        }
    }
}
