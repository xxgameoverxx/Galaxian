﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;

public class Manager : MonoBehaviour {

    Dictionary<int, TypeInfo> enemyTypeDict;
    Dictionary<int, TypeInfo> itemTypeDict;
    List<int> enemyTypes;
    List<int> itemTypes;
    GameObject currentDummy;
    int index = 0;
    Vector3 prfabPos;
    bool updateEnemy = false;
    float updateCounter = 0;

    public List<Vector3> wayPoints;
    private GUISkin skin;
    private GUIStyle button;
    private GUIStyle box;
    private GUIStyle textField;
    private GUIStyle label;

    #region New Values
    string ammoCount = "0";
    string newName = "NOPE";
    string newHealth = "NOPE";
    string newEnergy = "NOPE";
    string newEnergyRegen = "NOPE";
    float newSelfDestroyProb = 0;
    float newShootProb = 0;
    int spread = 1;
    float bulletSpeed = 10;
    string newHurtCooldown = "NOPE";
    string newAmplitudeX = "NOPE";
    string newAmplitudeY = "NOPE";
    string newDampingX = "NOPE";
    string newDampingY = "NOPE";
    string newMoveSpeed = "NOPE";
    float newDropProb = 100;
    bool newMoveToWaypoint = false;



    bool hasHealth = false;
    bool hasShield = false;
    bool hasShotgunAmmo = false;
    bool hasLaserAmmo = false;
    bool hasRocketAmmo = false;
    bool hasDevineIntervention = false;

    bool shotgun = false;
    bool Shotgun
    {
        get
        {
            return shotgun;
        }
        set
        {
            shotgun = value;
            if(value)
            {
                laser = false;
                rocket = false;
            }
        }
    }
    bool laser = false;
    bool Laser
    {
        get
        {
            return laser;
        }
        set
        {
            laser = value;
            if (value)
            {
                shotgun = false;
                rocket = false;
            }
        }
    }
    bool rocket = false;
    bool Rocket
    {
        get
        {
            return rocket;
        }
        set
        {
            rocket = value;
            if (value)
            {
                laser = false;
                shotgun = false;
            }
        }
    }

    float shotGunAmmoCount = 5;
    float laserAmmoCount = 5;
    float rocketAmmoCount = 5;
    float divineInterventionAmmoCount = 1;
    float shileldDuration = 5;
    float healthAmount = 3;
    #endregion

    private TypeInfo currentType;
    public TypeInfo CurrentType
    {
        get
        {
            return currentType;
        }
    }
    private Enemy _enemy;
    public Enemy enemy
    {
        get
        {
            if(_enemy == null)
            {
                _enemy = currentDummy.GetComponent<Enemy>();
            }
            return _enemy;
        }
        set
        {
            _enemy = value;
        }
    }

    private TypeInfoHolder typeInfoHolder;
    private Dictionary<int, TypeInfo> EnemyTypeDict
    {
        get
        {
            if (typeInfoHolder == null)
            {
                typeInfoHolder = new TypeInfoHolder();
            }
            if(enemyTypeDict == null)
            {
                enemyTypeDict = typeInfoHolder.enemyTypeDict;
            }
            return enemyTypeDict;
        }
    }
    private Dictionary<int, TypeInfo> ItemTypeDict
    {
        get
        {
            if (typeInfoHolder == null)
            {
                typeInfoHolder = new TypeInfoHolder();
            }
            if (itemTypeDict == null)
            {
                itemTypeDict = typeInfoHolder.itemTypeDict;
            }
            return itemTypeDict;
        }
    }
    private List<int> EnemyTypes
    {
        get
        {
            if (typeInfoHolder == null)
            {
                typeInfoHolder = new TypeInfoHolder();
            }
            if (enemyTypes == null)
            {
                enemyTypes = typeInfoHolder.enemyIds;
            }
            return enemyTypes;
        }
    }
    private List<int> ItemTypes
    {
        get
        {
            if (typeInfoHolder == null)
            {
                typeInfoHolder = new TypeInfoHolder();
            }
            if (itemTypes == null)
            {
                itemTypes = typeInfoHolder.itemIds;
            }
            return itemTypes;
        }
    }

    void Awake()
    {
        prfabPos = GameObject.FindGameObjectWithTag("Respawn").transform.position;
        //XmlDocument typeDoc = new XmlDocument();
        //typeDoc.Load(Application.dataPath + "/Resources/TypeInfo.xml");
        //foreach (XmlNode node in typeDoc.ChildNodes)
        //{
        //    FillTypePrefabDict(node);
        //}
    }

    void Start()
    {
        if (GameObject.FindObjectOfType<Style>() != null)
        {
            skin = GameObject.FindObjectOfType<Style>().skin;
            button = skin.customStyles[2];
            textField = skin.customStyles[3];
            box = skin.customStyles[4];
            label = skin.customStyles[6];
        }
        else
        {
            Debug.LogError("Style object is not found!");
            button = new GUIStyle();
        }
        foreach(GameObject g in GameObject.FindGameObjectsWithTag("Waypoint"))
        {
            wayPoints.Add(g.transform.position);
        }
        ChangePrefab();
    }

    void OnGUI()
    {
        //GUI.Label(new Rect(Screen.width / 8 * 5, Screen.height / 20, Screen.width / 8, Screen.height / 20), "Id: ", label);
        //GUI.Label(new Rect(Screen.width / 8 * 5, Screen.height / 20 * 2, Screen.width / 4, Screen.height / 20), "Name: ", label);
        //GUI.Label(new Rect(Screen.width / 8 * 5, Screen.height / 20 * 3, Screen.width / 4, Screen.height / 20), "Health: ", label);
        //GUI.Label(new Rect(Screen.width / 8 * 5, Screen.height / 20 * 4, Screen.width / 4, Screen.height / 20), "Energy: ", label);
        
        GUI.BeginGroup(new Rect(Screen.width / 2 + Screen.width / 7, Screen.height / 8, 350, 570));
        GUI.Label(new Rect(0, 0, 230, 30), "Id: ", label);
        GUI.Label(new Rect(0, 30, 230, 30), "Name: ", label);
        GUI.Label(new Rect(0, 60, 230, 30), "Health: ", label);
        GUI.Label(new Rect(0, 90, 230, 30), "Energy: ", label);
        GUI.Label(new Rect(250, 0, 230, 30), currentType.id.ToString(), label);
        newName = GUI.TextField(new Rect(250, 30, 100, 25), newName, textField);
        newHealth = GUI.TextField(new Rect(250, 60, 100, 25), newHealth, textField);
        newEnergy = GUI.TextField(new Rect(250, 90, 100, 25), newEnergy, textField);
        GUI.Label(new Rect(0, 120, 230, 30), "Energy Regeneration Speed: ", label);
        GUI.Label(new Rect(0, 150, 230, 30), "Self Destroy Probability: ", label);
        GUI.Label(new Rect(0, 180, 230, 30), "Shoot Probability: ", label);
        GUI.Label(new Rect(0, 210, 230, 30), "Hurt Cooldown: ", label);
        GUI.Label(new Rect(0, 240, 230, 30), "Amplitude X: ", label);
        GUI.Label(new Rect(0, 270, 230, 30), "Amplitude Y: ", label);
        GUI.Label(new Rect(0, 300, 230, 30), "Damping X: ", label);
        GUI.Label(new Rect(0, 330, 230, 30), "Damping Y: ", label);
        GUI.Label(new Rect(0, 360, 230, 30), "Move Speed: ", label);
        GUI.Label(new Rect(0, 390, 230, 30), "Drop Probability: ", label);
        GUI.Label(new Rect(0, 420, 230, 30), "Move To Waypoint: ", label);
        newEnergyRegen = GUI.TextField(new Rect(250, 120, 100, 25), newEnergyRegen, textField);
        newSelfDestroyProb = GUI.HorizontalScrollbar(new Rect(250, 150, 100, 25), newSelfDestroyProb, 10, 0, 100);
        newShootProb = GUI.HorizontalScrollbar(new Rect(250, 180, 100, 25), newShootProb, 10, 0, 100);
        newHurtCooldown = GUI.TextField(new Rect(250, 210, 100, 25), newHurtCooldown, textField);
        newAmplitudeX = GUI.TextField(new Rect(250, 240, 100, 25), newAmplitudeX, textField);
        newAmplitudeY = GUI.TextField(new Rect(250, 270, 100, 25), newAmplitudeY, textField);
        newDampingX = GUI.TextField(new Rect(250, 300, 100, 25), newDampingX, textField);
        newDampingY = GUI.TextField(new Rect(250, 330, 100, 25), newDampingY, textField);
        newMoveSpeed = GUI.TextField(new Rect(250, 360, 100, 25), newMoveSpeed, textField);
        newDropProb = GUI.HorizontalScrollbar(new Rect(250, 390, 100, 25), newDropProb, 10, 0, 100);
        newMoveToWaypoint = GUI.Toggle(new Rect(250, 420, 100, 25), newMoveToWaypoint, "");
        if (GUI.Button(new Rect(0, 450, 85, 30), "Previous", button))
        {
            Previous();
        }
        if (GUI.Button(new Rect(85, 450, 85, 30), "Next", button))
        {
            Next();
        }
        if (GUI.Button(new Rect(170, 450, 175, 30), "Delete", button))
        {
            if (!currentType.locked)
            {
                EnemyTypes.Remove(currentType.id);
                EnemyTypes.Remove(currentType.id);
                currentType = EnemyTypeDict[EnemyTypes[0]];
                index = 0;
                ChangePrefab();
            }
            else
            {
                Debug.LogError("This type is protected!");
            }
        }
        if (GUI.Button(new Rect(0, 480, 170, 30), "Apply", button))
        {
            Apply();
        }
        if (GUI.Button(new Rect(170, 480, 175, 30), "Save as new", button))
        {
            TypeInfo type = new TypeInfo();
            type.Clone(currentType);
            type.locked = false;
            for (int i = 0; i < 100; i++)
            {
                if (!EnemyTypes.Contains(i))
                {
                    type.id = i;
                    break;
                }
            }
            EnemyTypes.Add(type.id);
            if (EnemyTypeDict.ContainsKey(type.id))
            {
                EnemyTypeDict.Remove(type.id);
            }
            EnemyTypeDict.Add(type.id, type);
            currentType = type;
            Apply();
        }
        if (GUI.Button(new Rect(0, 510, 350, 30), "Save", button))
        {
            WriteToXML();
        }
        if (GUI.Button(new Rect(0, 540, 175, 30), "Hit", button))
        {
            currentDummy.GetComponent<Enemy>().Hit();
        }
        if (GUI.Button(new Rect(175, 540, 175, 30), "Kill", button))
        {
            currentDummy.GetComponent<Enemy>().Die();
        }
        GUI.EndGroup();

        GUI.Box(new Rect(Screen.width / 2 - 500, 20, 585, 125), "");
        GUI.BeginGroup(new Rect(Screen.width / 2 - 500, Screen.height / 30, 585, 125));
        GUI.Box(new Rect(0, 0, 340, 30), "Inventory", box);
        GUI.Box(new Rect(360, 0, 220, 30), "Weapon", box);
        hasHealth = GUI.Toggle(new Rect(0, 30, 120, 25), hasHealth, "Health");
        healthAmount = float.Parse(GUI.TextField(new Rect(120, 30, 40, 20), healthAmount.ToString()));
        hasShield = GUI.Toggle(new Rect(0, 60, 120, 25), hasShield, "Shield");
        shileldDuration = float.Parse(GUI.TextField(new Rect(120, 60, 40, 20), shileldDuration.ToString()));
        hasShotgunAmmo = GUI.Toggle(new Rect(0, 90, 120, 25), hasShotgunAmmo, "Shotgun Ammo");
        shotGunAmmoCount = float.Parse(GUI.TextField(new Rect(120, 90, 40, 20), shotGunAmmoCount.ToString()));
        hasLaserAmmo = GUI.Toggle(new Rect(170, 30, 130, 25), hasLaserAmmo, "Laser Ammo");
        laserAmmoCount = float.Parse(GUI.TextField(new Rect(300, 30, 40, 20), laserAmmoCount.ToString()));
        hasRocketAmmo = GUI.Toggle(new Rect(170, 60, 130, 25), hasRocketAmmo, "Rocket Ammo");
        rocketAmmoCount = float.Parse(GUI.TextField(new Rect(300, 60, 40, 20), rocketAmmoCount.ToString()));
        hasDevineIntervention = GUI.Toggle(new Rect(170, 90, 130, 25), hasDevineIntervention, "Devine Intervention");
        divineInterventionAmmoCount = float.Parse(GUI.TextField(new Rect(300, 90, 40, 20), divineInterventionAmmoCount.ToString()));
        Shotgun = GUI.Toggle(new Rect(360, 30, 75, 25), Shotgun, "Shotgun");
        Laser = GUI.Toggle(new Rect(360, 60, 75, 25), Laser, "Laser");
        Rocket = GUI.Toggle(new Rect(360, 90, 75, 25), Rocket, "Rocket");
        GUI.Label(new Rect(435, 30, 100, 25), "Ammo Count", label);
        ammoCount = GUI.TextField(new Rect(535, 30, 40, 20), ammoCount, textField);
        GUI.Label(new Rect(435, 60, 100, 25), "Spread", label);
        spread = int.Parse(GUI.TextField(new Rect(535, 60, 40, 20), spread.ToString(), textField));
        GUI.Label(new Rect(435, 90, 100, 25), "Bullet Speed", label);
        bulletSpeed = float.Parse(GUI.TextField(new Rect(535, 90, 40, 20), bulletSpeed.ToString(), textField));
        GUI.EndGroup();

        if (GUI.Button(new Rect(Screen.width / 2 + Screen.width / 7 + 250, Screen.height / 30, 100, 50), "Main menu", button))
        {
            Application.LoadLevel("MainMenu");
        }
    }

    void Apply()
    {
        EnemyTypeDict[currentType.id].name = newName;
        EnemyTypeDict[currentType.id].maxHealth = float.Parse(newHealth);
        EnemyTypeDict[currentType.id].maxEnergy = float.Parse(newEnergy);
        EnemyTypeDict[currentType.id].engRegenSpeed = float.Parse(newEnergyRegen);
        EnemyTypeDict[currentType.id].selfDestroyProbability = newSelfDestroyProb;
        EnemyTypeDict[currentType.id].shootProbability = newShootProb;
        EnemyTypeDict[currentType.id].hurtCooldown = float.Parse(newHurtCooldown);
        EnemyTypeDict[currentType.id].amplitudeX = float.Parse(newAmplitudeX);
        EnemyTypeDict[currentType.id].amplitudeY = float.Parse(newAmplitudeY);
        EnemyTypeDict[currentType.id].dampingX = float.Parse(newDampingX);
        EnemyTypeDict[currentType.id].dampingY = float.Parse(newDampingY);
        EnemyTypeDict[currentType.id].moveSpeed = float.Parse(newMoveSpeed);
        EnemyTypeDict[currentType.id].dropProbability = newDropProb;
        EnemyTypeDict[currentType.id].moveToWaypoint = newMoveToWaypoint;
        EnemyTypeDict[currentType.id].ammoCount = int.Parse(ammoCount);
        EnemyTypeDict[currentType.id].bulletSpeed = bulletSpeed;
        EnemyTypeDict[currentType.id].spread = spread;

        EnemyTypeDict[currentType.id].inventoryItems = new Dictionary<int, float>();
        if (hasHealth)
        {
            EnemyTypeDict[currentType.id].inventoryItems.Add(100, healthAmount);
        }
        if (hasShield)
        {
            EnemyTypeDict[currentType.id].inventoryItems.Add(105, shileldDuration);
        } if (hasShotgunAmmo)
        {
            EnemyTypeDict[currentType.id].inventoryItems.Add(102, shotGunAmmoCount);
        } if (hasLaserAmmo)
        {
            EnemyTypeDict[currentType.id].inventoryItems.Add(103, laserAmmoCount);
        } if (hasRocketAmmo)
        {
            EnemyTypeDict[currentType.id].inventoryItems.Add(104, rocketAmmoCount);
        } if (hasDevineIntervention)
        {
            EnemyTypeDict[currentType.id].inventoryItems.Add(101, divineInterventionAmmoCount);
        }
        if (Shotgun)
        {
            EnemyTypeDict[currentType.id].weapon = 106;
        }
        else if (Laser)
        {
            EnemyTypeDict[currentType.id].weapon = 107;
        }
        else if (Rocket)
        {
            EnemyTypeDict[currentType.id].weapon = 108;
        }
        else
        {
            EnemyTypeDict[currentType.id].weapon = -1;
        }
        updateEnemy = true;
    }

    void UpdateEnemy()
    {
        currentDummy.transform.position = prfabPos;
        Enemy e = currentDummy.GetComponent<Enemy>();
        e.name = currentType.name;
        e.maxHealth = currentType.maxHealth;
        e.health = e.maxHealth;
        e.maxEnergy = currentType.maxEnergy;
        e.energy = e.maxEnergy;
        e.engRegenSpeed = currentType.engRegenSpeed;
        e.selfDestroyProbability = currentType.selfDestroyProbability;
        e.shootProbability = currentType.shootProbability;
        e.hurtCooldown = currentType.hurtCooldown;
        e.amplitudeX = currentType.amplitudeX;
        e.amplitudeY = currentType.amplitudeY;
        e.dampingX = currentType.dampingX;
        e.dampingY = currentType.dampingY;
        e.moveSpeed = currentType.moveSpeed;
        e.moveToWaypoint = currentType.moveToWaypoint;
        e.inventory = new List<Item>();
        foreach(int i in currentType.inventoryItems.Keys)
        {
            GameObject newItem = Resources.Load(ItemTypeDict[i].prefab) as GameObject;
            newItem.GetComponent<Item>().durability = EnemyTypeDict[CurrentType.id].inventoryItems[i];
            e.inventory.Add(newItem.GetComponent<Item>());
        }

        if (currentType.weapon != -1)
        {
            TypeInfo type = ItemTypeDict[currentType.weapon];
            GameObject newItem = Resources.Load(type.prefab) as GameObject;
            GameObject weapon = Instantiate(newItem, e.transform.position, e.transform.rotation) as GameObject;
            weapon.name = type.name;
            weapon.GetComponent<Weapon>().ammoCount = currentType.ammoCount;
            e.EquipWeapon(weapon.GetComponent<Weapon>());
        }
    }

    void WriteToXML()
    {
        XmlWriterSettings settings = new XmlWriterSettings();
        settings.Indent = true;
        settings.IndentChars = "\t";
        using (XmlWriter writer = XmlWriter.Create(Application.dataPath + "/Resources/TypeInfo.xml", settings))
        {
            writer.WriteStartDocument();
            writer.WriteStartElement("Types");
            foreach (int i in EnemyTypes)
            {
                TypeInfo t = EnemyTypeDict[i];
                writer.WriteStartElement("Type");
                writer.WriteAttributeString("type", t.id.ToString());
                writer.WriteAttributeString("name", t.name);
                writer.WriteAttributeString("prefab", t.prefab);
                writer.WriteAttributeString("locked", t.locked.ToString());
                writer.WriteAttributeString("spread", t.spread.ToString());
                writer.WriteAttributeString("bulletSpeed", t.bulletSpeed.ToString());
                writer.WriteStartElement("Amplitude");
                writer.WriteAttributeString("x", t.amplitudeX.ToString());
                writer.WriteAttributeString("y", t.amplitudeY.ToString());
                writer.WriteEndElement(); //Amplitude
                writer.WriteStartElement("ShootProbability");
                writer.WriteAttributeString("value", t.shootProbability.ToString());
                writer.WriteEndElement(); //ShootProbability
                writer.WriteStartElement("SelfDestroyProbability");
                writer.WriteAttributeString("value", t.selfDestroyProbability.ToString());
                writer.WriteEndElement(); //SelfDestroyProbability
                writer.WriteStartElement("Damping");
                writer.WriteAttributeString("x", t.dampingX.ToString());
                writer.WriteAttributeString("y", t.dampingY.ToString());
                writer.WriteEndElement(); //Damping
                writer.WriteStartElement("Health");
                writer.WriteAttributeString("value", t.maxHealth.ToString());
                writer.WriteAttributeString("maxValue", t.maxHealth.ToString());
                writer.WriteEndElement(); //Health
                writer.WriteStartElement("Energy");
                writer.WriteAttributeString("value", t.maxHealth.ToString());
                writer.WriteAttributeString("maxValue", t.maxHealth.ToString());
                writer.WriteAttributeString("regenSpeed", t.engRegenSpeed.ToString());
                writer.WriteEndElement(); //Energy
                writer.WriteStartElement("MoveSpeed");
                writer.WriteAttributeString("value", t.moveSpeed.ToString());
                writer.WriteEndElement(); //MoveSpeed
                writer.WriteStartElement("HurtCooldown");
                writer.WriteAttributeString("value", t.hurtCooldown.ToString());
                writer.WriteEndElement(); //HurtCooldown
                writer.WriteStartElement("DropProbability");
                writer.WriteAttributeString("value", t.dropProbability.ToString());
                writer.WriteEndElement(); //DropProbability
                writer.WriteStartElement("MoveToWaypoint");
                writer.WriteAttributeString("value", t.moveToWaypoint.ToString());
                writer.WriteEndElement(); //MoveToWaypoint
                writer.WriteStartElement("Weapon");
                writer.WriteAttributeString("value", t.weapon.ToString());
                writer.WriteAttributeString("ammoCount", t.ammoCount.ToString());

                writer.WriteEndElement(); //Weapon
                foreach(int j in t.inventoryItems.Keys)
                {
                    writer.WriteStartElement("InventoryItem");
                    writer.WriteAttributeString("value", j.ToString());
                    writer.WriteAttributeString("durability", t.inventoryItems[j].ToString());
                    writer.WriteEndElement(); //InventoryItem
                }
                writer.WriteEndElement(); //Type
            }
            foreach (int i in ItemTypes)
            {
                TypeInfo t = ItemTypeDict[i];
                writer.WriteStartElement("Type");
                writer.WriteAttributeString("type", t.id.ToString());
                writer.WriteAttributeString("name", t.name.ToString());
                writer.WriteAttributeString("prefab", t.prefab.ToString());
                writer.WriteAttributeString("durability", t.durability.ToString());
                writer.WriteAttributeString("slotName", ((int)t.slotName).ToString());
                writer.WriteEndElement(); //Type
            }
            writer.WriteEndElement(); //Types
            writer.WriteEndDocument();
        }

    }

    void ChangePrefab()
    {
        if (currentDummy != null)
        {
            Destroy(currentDummy);
        }
        currentType = EnemyTypeDict[EnemyTypes[index]];
        GameObject go = Resources.Load(currentType.prefab) as GameObject;
        currentDummy = Instantiate(go, prfabPos, go.transform.rotation) as GameObject;
        currentDummy.name = currentType.name;
        newName = currentType.name;
        newHealth = currentType.maxHealth.ToString();
        newEnergy = currentType.maxEnergy.ToString();
        newEnergyRegen = currentType.engRegenSpeed.ToString();
        newSelfDestroyProb = currentType.selfDestroyProbability;
        newShootProb = currentType.shootProbability;
        newHurtCooldown = currentType.hurtCooldown.ToString();
        newAmplitudeX = currentType.amplitudeX.ToString();
        newAmplitudeY = currentType.amplitudeY.ToString();
        newDampingX = currentType.dampingX.ToString();
        newDampingY = currentType.dampingY.ToString();
        newMoveSpeed = currentType.moveSpeed.ToString();
        newMoveToWaypoint = currentType.moveToWaypoint;
        newDropProb = currentType.dropProbability;
        ammoCount = currentType.ammoCount.ToString();
        if (currentType.inventoryItems.ContainsKey(100))
        {
            hasHealth = true;
            healthAmount = currentType.inventoryItems[100];
        }
        else hasHealth = false;
        if (currentType.inventoryItems.ContainsKey(101))
        {
            hasDevineIntervention = true;
            divineInterventionAmmoCount = currentType.inventoryItems[101];
        }
        else hasDevineIntervention = false;
        if (currentType.inventoryItems.ContainsKey(102))
        {
            hasShotgunAmmo = true;
            shotGunAmmoCount = currentType.inventoryItems[102];
        }
        else hasShotgunAmmo = false;
        if (currentType.inventoryItems.ContainsKey(103))
        {
            hasLaserAmmo = true;
            laserAmmoCount = currentType.inventoryItems[103];
        }
        else hasLaserAmmo = false;
        if (currentType.inventoryItems.ContainsKey(104))
        {
            hasRocketAmmo = true;
            rocketAmmoCount = currentType.inventoryItems[104];
        }
        else hasRocketAmmo = false;
        if (currentType.inventoryItems.ContainsKey(105))
        {
            hasShield = true;
            shileldDuration = currentType.inventoryItems[105];
        }
        else hasShield = false;
        if(currentType.weapon == 106)
        {
            Shotgun = true;
        }
        else if(currentType.weapon == 107)
        {
            Laser = true;
        }
        else if(currentType.weapon == 108)
        {
            Rocket = true;
        }
        else
        {
            Shotgun = false;
            Laser = false;
            Rocket = false;
        }
        currentDummy.AddComponent<PlayerGUI>();
        updateEnemy = true;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Previous();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Next();
        }
    }

    void Next()
    {
        if (index < EnemyTypes.Count - 1)
        {
            index++;
        }
        else
        {
            index = 0;
        }
        ChangePrefab();
    }

    void Previous()
    {
        if (index > 0)
        {
            index--;
        }
        else
        {
            index = EnemyTypes.Count - 1;
        }
        ChangePrefab();
    }

    void LateUpdate()
    {
        if (currentDummy == null)
        {
            ChangePrefab();
        }
        if(updateEnemy)
        {
            updateEnemy = false;
            UpdateEnemy();
        }
    }
}
