using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public enum Types
{
    TypeEnemy = 0,
    TypeItem = 1
}

public class TypeInfo
{
    public float amplitudeX;
    public float amplitudeY;
    public float shootProbability;
    public float selfDestroyProbability;
    public float dampingX;
    public float dampingY;
    public float health;
    public float maxHealth;
    public float energy;
    public float engRegenSpeed;
    public float maxEnergy;
    public float moveSpeed = 100;
    public float hurtCooldown;
    public float dropProbability;
    public float durability;
    public int id;
    public int weapon = -1;
    public int ammoCount;
    public int spread = 1;
    public float bulletSpeed = 10;
    public Dictionary<int, float> inventoryItems = new Dictionary<int, float>();
    public string prefab;
    public string name;
    public bool moveToWaypoint;
    public bool locked = false;
    public Types type;
    public SlotName slotName;

    public void Clone(TypeInfo t)
    {
        amplitudeX = t.amplitudeX;
        amplitudeY = t.amplitudeY;
        shootProbability = t.shootProbability;
        selfDestroyProbability = t.selfDestroyProbability;
        dampingX = t.dampingX;
        dampingY = t.dampingY;
        health = t.health;
        maxHealth = t.maxHealth;
        energy = t.energy;
        engRegenSpeed = t.engRegenSpeed;
        maxEnergy = t.maxEnergy;
        moveSpeed = t.moveSpeed;
        hurtCooldown = t.hurtCooldown;
        dropProbability = t.dropProbability;
        durability = t.durability;
        id = t.id;
        weapon = t.weapon;
        ammoCount = t.ammoCount;
        inventoryItems = t.inventoryItems;
        prefab = t.prefab;
        name = t.name;
        moveToWaypoint = t.moveToWaypoint;
        type = t.type;
        slotName = t.slotName;
    }

    public void ReadInfo(XmlNode node)
    {
        foreach (XmlAttribute att in node.Attributes)
        {
            if (att.Name == "type")
            {
                id = int.Parse(att.Value);
            }
            else if (att.Name == "name")
            {
                name = att.Value;
            }
            else if (att.Name == "prefab")
            {
                prefab = att.Value;
            }
            else if (att.Name == "durability")
            {
                durability = float.Parse(att.Value);
            }
            else if (att.Name == "slotName")
            {
                slotName = (SlotName)(int.Parse(att.Value));
            }
            else if(att.Name == "locked")
            {
                locked = bool.Parse(att.Value);
            }
            else if (att.Name == "spread")
            {
                spread = int.Parse(att.Value);
            }
            else if (att.Name == "bulletSpeed")
            {
                bulletSpeed = float.Parse(att.Value);
            }
        }
        foreach (XmlNode xnode in node.ChildNodes)
        {
            if (xnode.Name == "SelfDestroyProbability")
            {
                selfDestroyProbability = float.Parse(xnode.Attributes["value"].Value);
            }
            else if (xnode.Name == "MoveToWaypoint")
            {
                moveToWaypoint = bool.Parse(xnode.Attributes["value"].Value);
            }
            else if (xnode.Name == "Amplitude")
            {
                amplitudeX = float.Parse(xnode.Attributes["x"].Value);
                amplitudeY = float.Parse(xnode.Attributes["y"].Value);
            }
            else if (xnode.Name == "ShootProbability")
            {
                shootProbability = float.Parse(xnode.Attributes["value"].Value);
            }
            else if (xnode.Name == "Damping")
            {
                dampingX = float.Parse(xnode.Attributes["x"].Value);
                dampingY = float.Parse(xnode.Attributes["y"].Value);
            }
            else if (xnode.Name == "Health")
            {
                health = float.Parse(xnode.Attributes["value"].Value);
                maxHealth = float.Parse(xnode.Attributes["maxValue"].Value);
            }
            else if (xnode.Name == "Energy")
            {
                energy = float.Parse(xnode.Attributes["value"].Value);
                maxEnergy = float.Parse(xnode.Attributes["maxValue"].Value);
                engRegenSpeed = float.Parse(xnode.Attributes["regenSpeed"].Value);
            }
            else if (xnode.Name == "MoveSpeed")
            {
                moveSpeed = float.Parse(xnode.Attributes["value"].Value);
            }
            else if (xnode.Name == "HurtCooldown")
            {
                hurtCooldown = float.Parse(xnode.Attributes["value"].Value);
            }
            else if (xnode.Name == "DropProbability")
            {
                dropProbability = float.Parse(xnode.Attributes["value"].Value);
            }
            else if (xnode.Name == "InventoryItem")
            {
                if (inventoryItems == null)
                {
                    inventoryItems = new Dictionary<int, float>();
                }
                inventoryItems.Add(int.Parse(xnode.Attributes["value"].Value), float.Parse(xnode.Attributes["durability"].Value));
            }
            else if (xnode.Name == "Weapon")
            {
                weapon = int.Parse(xnode.Attributes["value"].Value);
                ammoCount = int.Parse(xnode.Attributes["ammoCount"].Value);
            }
            else
            {
                if (xnode.Name != "Dummy")
                {
                    Debug.LogError("Unknown node: " + xnode.Name + ". Child of " + node.Name);
                }
            }
        }
    }
}
