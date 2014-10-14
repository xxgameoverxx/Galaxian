using UnityEngine;
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

    #region New Values
    string ammoCount = "0";
    string newName = "NOPE";
    string newHealth = "NOPE";
    string newEnergy = "NOPE";
    string newEnergyRegen = "NOPE";
    float newSelfDestroyProb = 0;
    float newShootProb = 0;
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
        ChangePrefab();
    }

    void Start()
    {
        foreach(GameObject g in GameObject.FindGameObjectsWithTag("Waypoint"))
        {
            wayPoints.Add(g.transform.position);
        }
    }

    //void FillTypePrefabDict(XmlNode node)
    //{
    //    foreach(XmlNode child in node.ChildNodes)
    //    {
    //        int typeId = int.Parse(child.Attributes["type"].Value);
    //        TypeInfo type = new TypeInfo();
    //        type.ReadInfo(child);
    //        if (typeId < 100)
    //        {
    //            enemyTypeDict.Add(typeId, type);
    //            enemyTypes.Add(typeId);
    //        }
    //        else
    //        {
    //            itemTypeDict.Add(typeId, type);
    //            itemTypes.Add(typeId);
    //        }
    //    }
    //}

    void OnGUI()
    {
        GUI.Label(new Rect(Screen.width / 2, Screen.height / 20, Screen.width / 4, Screen.height / 20), "Id: ");
        GUI.Label(new Rect(Screen.width / 2, Screen.height / 10, Screen.width / 4, Screen.height / 20), "Name: ");
        GUI.Label(new Rect(Screen.width / 2, Screen.height / 20 * 3, Screen.width / 4, Screen.height / 20), "Health: ");
        GUI.Label(new Rect(Screen.width / 2, Screen.height / 5, Screen.width / 4, Screen.height / 20), "Energy: ");
        GUI.Label(new Rect(Screen.width / 2, Screen.height / 4, Screen.width / 4, Screen.height / 20), "Energy Regeneration Speed: ");
        GUI.Label(new Rect(Screen.width / 2, Screen.height / 10 * 3, Screen.width / 4, Screen.height / 20), "Self Destroy Probability: ");
        GUI.Label(new Rect(Screen.width / 2, Screen.height / 20 * 7, Screen.width / 4, Screen.height / 20), "Shoot Probability: ");
        GUI.Label(new Rect(Screen.width / 2, Screen.height / 5 * 2, Screen.width / 4, Screen.height / 20), "Hurt Cooldown: ");
        GUI.Label(new Rect(Screen.width / 2, Screen.height / 20 * 9, Screen.width / 4, Screen.height / 20), "Amplitude X: ");
        GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, Screen.width / 4, Screen.height / 20), "Amplitude Y: ");
        GUI.Label(new Rect(Screen.width / 2, Screen.height / 20 * 11, Screen.width / 4, Screen.height / 20), "Damping X: ");
        GUI.Label(new Rect(Screen.width / 2, Screen.height / 5 * 3, Screen.width / 4, Screen.height / 20), "Damping Y: ");
        GUI.Label(new Rect(Screen.width / 2, Screen.height / 20 * 13, Screen.width / 4, Screen.height / 20), "Move Speed: ");
        GUI.Label(new Rect(Screen.width / 2, Screen.height / 10 * 7, Screen.width / 4, Screen.height / 20), "Drop Probability: ");
        GUI.Label(new Rect(Screen.width / 2, Screen.height / 4 * 3, Screen.width / 4, Screen.height / 20), "Move To Waypoint: ");
        GUI.Box(new Rect(Screen.width / 30, Screen.height / 20, Screen.width / 4 - Screen.width / 30, Screen.height / 30), "Inventory");
        GUI.Box(new Rect(Screen.width / 60 + Screen.width / 4, Screen.height / 20, Screen.width / 4 - Screen.width / 30, Screen.height / 30), "Weapon");

        GUI.Label(new Rect(Screen.width / 4 * 3, Screen.height / 20, Screen.width / 4, Screen.height / 20), currentType.id.ToString());
        newName = GUI.TextField(new Rect(Screen.width / 4 * 3, Screen.height / 10, Screen.width / 4, Screen.height / 20), newName);
        newHealth = GUI.TextField(new Rect(Screen.width / 4 * 3, Screen.height / 20 * 3, Screen.width / 4, Screen.height / 20), newHealth);
        newEnergy = GUI.TextField(new Rect(Screen.width / 4 * 3, Screen.height / 5, Screen.width / 4, Screen.height / 20), newEnergy);
        newEnergyRegen = GUI.TextField(new Rect(Screen.width / 4 * 3, Screen.height / 4, Screen.width / 4, Screen.height / 20), newEnergyRegen);
        newSelfDestroyProb = GUI.HorizontalScrollbar(new Rect(Screen.width / 4 * 3, Screen.height / 10 * 3, Screen.width / 4, Screen.height / 20), newSelfDestroyProb, 10, 0, 100);
        newShootProb = GUI.HorizontalScrollbar(new Rect(Screen.width / 4 * 3, Screen.height / 20 * 7, Screen.width / 4, Screen.height / 20), newShootProb, 10, 0, 100);
        newHurtCooldown = GUI.TextField(new Rect(Screen.width / 4 * 3, Screen.height / 5 * 2, Screen.width / 4, Screen.height / 20), newHurtCooldown);
        newAmplitudeX = GUI.TextField(new Rect(Screen.width / 4 * 3, Screen.height / 20 * 9, Screen.width / 4, Screen.height / 20), newAmplitudeX);
        newAmplitudeY = GUI.TextField(new Rect(Screen.width / 4 * 3, Screen.height / 2, Screen.width / 4, Screen.height / 20), newAmplitudeY);
        newDampingX = GUI.TextField(new Rect(Screen.width / 4 * 3, Screen.height / 20 * 11, Screen.width / 4, Screen.height / 20), newDampingX);
        newDampingY = GUI.TextField(new Rect(Screen.width / 4 * 3, Screen.height / 5 * 3, Screen.width / 4, Screen.height / 20), newDampingY);
        newMoveSpeed = GUI.TextField(new Rect(Screen.width / 4 * 3, Screen.height / 20 * 13, Screen.width / 4, Screen.height / 20), newMoveSpeed);
        newDropProb = GUI.HorizontalScrollbar(new Rect(Screen.width / 4 * 3, Screen.height / 10 * 7, Screen.width / 4, Screen.height / 20), newDropProb, 10, 0, 100);
        newMoveToWaypoint = GUI.Toggle(new Rect(Screen.width / 4 * 3, Screen.height / 4 * 3, Screen.width / 4, Screen.height / 20), newMoveToWaypoint, "");
        hasHealth = GUI.Toggle(new Rect(Screen.width / 30, Screen.height / 20 * 2f, Screen.width / 12, Screen.height / 40), hasHealth, "Health");
        hasShield = GUI.Toggle(new Rect(Screen.width / 30, Screen.height / 20 * 2.5f, Screen.width / 12, Screen.height / 40), hasShield, "Shield");
        hasShotgunAmmo = GUI.Toggle(new Rect(Screen.width / 30, Screen.height / 20 * 3f, Screen.width / 12, Screen.height / 40), hasShotgunAmmo, "Shotgun Ammo");
        hasLaserAmmo = GUI.Toggle(new Rect(Screen.width / 30 * 4, Screen.height / 20 * 2f, Screen.width / 12, Screen.height / 40), hasLaserAmmo, "Laser Ammo");
        hasRocketAmmo = GUI.Toggle(new Rect(Screen.width / 30 * 4, Screen.height / 20 * 2.5f, Screen.width / 12, Screen.height / 40), hasRocketAmmo, "Rocket Ammo");
        hasDevineIntervention = GUI.Toggle(new Rect(Screen.width / 30 * 4, Screen.height / 20 * 3f, Screen.width / 12, Screen.height / 40), hasDevineIntervention, "Devine Intervention");
        Shotgun = GUI.Toggle(new Rect(Screen.width / 15 * 4, Screen.height / 20 * 2f, Screen.width / 12, Screen.height / 40), Shotgun, "Shotgun");
        Laser = GUI.Toggle(new Rect(Screen.width / 15 * 4, Screen.height / 20 * 2.5f, Screen.width / 12, Screen.height / 40), Laser, "Laser");
        Rocket = GUI.Toggle(new Rect(Screen.width / 15 * 4, Screen.height / 20 * 3f, Screen.width / 12, Screen.height / 40), Rocket, "Rocket");
        ammoCount = GUI.TextField(new Rect(Screen.width / 5 * 2, Screen.height / 20 * 2.5f, Screen.width / 12, Screen.height / 20), ammoCount);
        GUI.Label(new Rect(Screen.width / 5 * 2, Screen.height / 20 * 2f, Screen.width / 12, Screen.height / 20), "Ammo Count");

        if (GUI.Button(new Rect(Screen.width / 5 * 3, Screen.height / 20 * 16, Screen.width / 10, Screen.height / 20), "Previous"))
        {
            Previous();
        }
        if (GUI.Button(new Rect(Screen.width / 10 * 7, Screen.height / 20 * 16, Screen.width / 10, Screen.height / 20), "Next"))
        {
            Next();
        }
        if (GUI.Button(new Rect(Screen.width / 5 * 4, Screen.height / 20 * 16, Screen.width / 5, Screen.height / 20), "Delete"))
        {
            EnemyTypes.Remove(currentType.id);
            EnemyTypes.Remove(currentType.id);
            currentType = EnemyTypeDict[EnemyTypes[0]];
            index = 0;
            ChangePrefab();
            //Apply();
        }
        if (GUI.Button(new Rect(Screen.width / 5 * 3, Screen.height / 20 * 17, Screen.width / 5, Screen.height / 20), "Apply"))
        {
            Apply();
        }
        if (GUI.Button(new Rect(Screen.width / 5 * 4, Screen.height / 20 * 17, Screen.width / 5, Screen.height / 20), "Save as new"))
        {
            TypeInfo type = new TypeInfo();
            type.Clone(currentType);
            for(int i = 0; i < 100; i++)
            {
                if (!EnemyTypes.Contains(i))
                {
                    type.id = i;
                    break;
                }
            }
            EnemyTypes.Add(type.id);
            EnemyTypeDict.Add(type.id, type);
            currentType = type;
            Apply();
        }
        if (GUI.Button(new Rect(Screen.width / 5 * 2, Screen.height / 20 * 17, Screen.width / 5, Screen.height / 20), "Save"))
        {
            WriteToXML();
        }
        if (GUI.Button(new Rect(0, Screen.height / 20 * 17, Screen.width / 5, Screen.height / 20), "Hit"))
        {
            currentDummy.GetComponent<Enemy>().Hit();
        }
        if (GUI.Button(new Rect(Screen.width / 5, Screen.height / 20 * 17, Screen.width / 5, Screen.height / 20), "Kill"))
        {
            currentDummy.GetComponent<Enemy>().Die();
        }
        if(GUI.Button(new Rect(Screen.width / 4 * 3.5f, 10, Screen.width / 10, Screen.height / 20), "Main menu"))
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

        EnemyTypeDict[currentType.id].inventoryItems = new List<int>();
        if (hasHealth)
        {
            EnemyTypeDict[currentType.id].inventoryItems.Add(100);
        }
        if (hasShield)
        {
            EnemyTypeDict[currentType.id].inventoryItems.Add(105);
        } if (hasShotgunAmmo)
        {
            EnemyTypeDict[currentType.id].inventoryItems.Add(102);
        } if (hasLaserAmmo)
        {
            EnemyTypeDict[currentType.id].inventoryItems.Add(103);
        } if (hasRocketAmmo)
        {
            EnemyTypeDict[currentType.id].inventoryItems.Add(104);
        } if (hasDevineIntervention)
        {
            EnemyTypeDict[currentType.id].inventoryItems.Add(101);
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
        foreach(int i in currentType.inventoryItems)
        {
            GameObject newItem = Resources.Load(ItemTypeDict[i].prefab) as GameObject;
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
        using (XmlWriter writer = XmlWriter.Create("Assets/Resources/TypeInfo.xml", settings))
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
                foreach(int j in t.inventoryItems)
                {
                    writer.WriteStartElement("InventoryItem");
                    writer.WriteAttributeString("value", j.ToString());
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
        if(currentDummy != null)
        {
            Destroy(currentDummy);
        }
        Debug.Log(index);
        Debug.Log("enemy dict " + EnemyTypes.Count);
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
        if(currentType.inventoryItems.Contains(100)) hasHealth = true;
        else hasHealth = false;
        if (currentType.inventoryItems.Contains(101)) hasDevineIntervention = true;
        else hasDevineIntervention = false;
        if (currentType.inventoryItems.Contains(102)) hasShotgunAmmo = true;
        else hasShotgunAmmo = false;
        if (currentType.inventoryItems.Contains(103)) hasLaserAmmo = true;
        else hasLaserAmmo = false;
        if (currentType.inventoryItems.Contains(104)) hasRocketAmmo = true;
        else hasRocketAmmo = false;
        if (currentType.inventoryItems.Contains(105)) hasShield = true;
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
