using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;

public class Manager : MonoBehaviour {

    Dictionary<int, TypeInfo> enemyTypeDict = new Dictionary<int, TypeInfo>();
    Dictionary<int, TypeInfo> itemTypeDict = new Dictionary<int, TypeInfo>();
    List<int> enemyTypes = new List<int>();
    List<int> itemTypes = new List<int>();
    GameObject currentDummy;
    int index = 0;
    Vector3 prfabPos;

    public List<Vector3> wayPoints;

    #region New Values
    string newName = "NOPE";
    string newHealth = "NOPE";
    string newEnergy = "NOPE";
    string newEnergyRegen = "NOPE";
    string newSelfDestroyProb = "NOPE";
    string newShootProb = "NOPE";
    string newHurtCooldown = "NOPE";
    string newAmplitudeX = "NOPE";
    string newAmplitudeY = "NOPE";
    string newDampingX = "NOPE";
    string newDampingY = "NOPE";
    string newMoveSpeed = "NOPE";
    string newDropProb = "NOPE";
    bool newMoveToWaypoint = false;
    #endregion

    public Properties currentProp;

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

    void Awake()
    {
        prfabPos = GameObject.FindGameObjectWithTag("Respawn").transform.position;
        XmlDocument typeDoc = new XmlDocument();
        typeDoc.Load(Application.dataPath + "/Resources/TypeInfo.xml");
        foreach (XmlNode node in typeDoc.ChildNodes)
        {
            FillTypePrefabDict(node);
        }
        ChangePrefab();
    }

    void Start()
    {
        foreach(GameObject g in GameObject.FindGameObjectsWithTag("Waypoint"))
        {
            wayPoints.Add(g.transform.position);
        }
    }

    void FillTypePrefabDict(XmlNode node)
    {
        foreach(XmlNode child in node.ChildNodes)
        {
            int typeId = int.Parse(child.Attributes["type"].Value);
            TypeInfo type = new TypeInfo();
            type.ReadInfo(child);
            if (typeId < 100)
            {
                enemyTypeDict.Add(typeId, type);
                enemyTypes.Add(typeId);
            }
            else
            {
                itemTypeDict.Add(typeId, type);
                itemTypes.Add(typeId);
            }
        }
    }

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

        GUI.Label(new Rect(Screen.width / 4 * 3, Screen.height / 20, Screen.width / 4, Screen.height / 20), currentType.id.ToString());
        newName = GUI.TextField(new Rect(Screen.width / 4 * 3, Screen.height / 10, Screen.width / 4, Screen.height / 20), newName);
        newHealth = GUI.TextField(new Rect(Screen.width / 4 * 3, Screen.height / 20 * 3, Screen.width / 4, Screen.height / 20), newHealth);
        newEnergy = GUI.TextField(new Rect(Screen.width / 4 * 3, Screen.height / 5, Screen.width / 4, Screen.height / 20), newEnergy);
        newEnergyRegen = GUI.TextField(new Rect(Screen.width / 4 * 3, Screen.height / 4, Screen.width / 4, Screen.height / 20), newEnergyRegen);
        newSelfDestroyProb = GUI.TextField(new Rect(Screen.width / 4 * 3, Screen.height / 10 * 3, Screen.width / 4, Screen.height / 20), newSelfDestroyProb);
        newShootProb = GUI.TextField(new Rect(Screen.width / 4 * 3, Screen.height / 20 * 7, Screen.width / 4, Screen.height / 20), newShootProb);
        newHurtCooldown = GUI.TextField(new Rect(Screen.width / 4 * 3, Screen.height / 5 * 2, Screen.width / 4, Screen.height / 20), newHurtCooldown);
        newAmplitudeX = GUI.TextField(new Rect(Screen.width / 4 * 3, Screen.height / 20 * 9, Screen.width / 4, Screen.height / 20), newAmplitudeX);
        newAmplitudeY = GUI.TextField(new Rect(Screen.width / 4 * 3, Screen.height / 2, Screen.width / 4, Screen.height / 20), newAmplitudeY);
        newDampingX = GUI.TextField(new Rect(Screen.width / 4 * 3, Screen.height / 20 * 11, Screen.width / 4, Screen.height / 20), newDampingX);
        newDampingY = GUI.TextField(new Rect(Screen.width / 4 * 3, Screen.height / 5 * 3, Screen.width / 4, Screen.height / 20), newDampingY);
        newMoveSpeed = GUI.TextField(new Rect(Screen.width / 4 * 3, Screen.height / 20 * 13, Screen.width / 4, Screen.height / 20), newMoveSpeed);
        newDropProb = GUI.TextField(new Rect(Screen.width / 4 * 3, Screen.height / 10 * 7, Screen.width / 4, Screen.height / 20), newDropProb);
        newMoveToWaypoint = GUI.Toggle(new Rect(Screen.width / 4 * 3, Screen.height / 4 * 3, Screen.width / 4, Screen.height / 20), newMoveToWaypoint, "");
        //newName = GUI.TextField(new Rect(Screen.width / 4 * 3, Screen.height / 10, Screen.width / 4, Screen.height / 20), newName);
        
        if (GUI.Button(new Rect(Screen.width / 2, Screen.height / 20 * 18, Screen.width / 4, Screen.height / 20), "Save"))
        {
            enemyTypeDict[currentType.id].name = newName;
            enemyTypeDict[currentType.id].maxHealth = float.Parse(newHealth);
            enemyTypeDict[currentType.id].maxEnergy = float.Parse(newEnergy);
            enemyTypeDict[currentType.id].engRegenSpeed = float.Parse(newEnergyRegen);
            enemyTypeDict[currentType.id].selfDestroyProbability = float.Parse(newSelfDestroyProb);
            enemyTypeDict[currentType.id].shootProbability = float.Parse(newShootProb);
            enemyTypeDict[currentType.id].hurtCooldown = float.Parse(newHurtCooldown);
            enemyTypeDict[currentType.id].amplitudeX = float.Parse(newAmplitudeX);
            enemyTypeDict[currentType.id].amplitudeY = float.Parse(newAmplitudeY);
            enemyTypeDict[currentType.id].dampingX = float.Parse(newDampingX);
            enemyTypeDict[currentType.id].dampingY = float.Parse(newDampingY);
            enemyTypeDict[currentType.id].moveSpeed = float.Parse(newMoveSpeed);
            enemyTypeDict[currentType.id].dropProbability = float.Parse(newDropProb);
            enemyTypeDict[currentType.id].moveToWaypoint = newMoveToWaypoint;
            UpdateEnemy();
        }
        if (GUI.Button(new Rect(Screen.width / 4 * 3, Screen.height / 20 * 18, Screen.width / 4, Screen.height / 20), "Write To XML"))
        {
            WriteToXML();
        }
        if (GUI.Button(new Rect(0, Screen.height / 20 * 18, Screen.width / 4, Screen.height / 20), "Hit"))
        {
            currentDummy.GetComponent<Enemy>().Hit();
        }
        if (GUI.Button(new Rect(Screen.width / 4, Screen.height / 20 * 18, Screen.width / 4, Screen.height / 20), "Kill"))
        {
            currentDummy.GetComponent<Enemy>().Die();
        }
    }

    void UpdateEnemy()
    {
        currentDummy.transform.position = prfabPos;
        Enemy e = currentDummy.GetComponent<Enemy>();
        e.name = currentType.name;
        e.maxHealth = currentType.maxHealth;
        e.maxEnergy = currentType.maxEnergy;
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
            foreach(int i in enemyTypes)
            {
                TypeInfo t = enemyTypeDict[i];
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
            foreach(int i in itemTypes)
            {
                TypeInfo t = itemTypeDict[i];
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
        currentType = enemyTypeDict[enemyTypes[index]];
        GameObject go = Resources.Load(currentType.prefab) as GameObject;
        currentDummy = Instantiate(go, prfabPos, go.transform.rotation) as GameObject;
        //currentDummy.GetComponent<Actor>().enabled = false;
        currentDummy.name = currentType.name;
        newName = currentType.name;
        newHealth = currentType.maxHealth.ToString();
        newEnergy = currentType.maxEnergy.ToString();
        newEnergyRegen = currentType.engRegenSpeed.ToString();
        newSelfDestroyProb = currentType.selfDestroyProbability.ToString();
        newShootProb = currentType.shootProbability.ToString();
        newHurtCooldown = currentType.hurtCooldown.ToString();
        newAmplitudeX = currentType.amplitudeX.ToString();
        newAmplitudeY = currentType.amplitudeY.ToString();
        newDampingX = currentType.dampingX.ToString();
        newDampingY = currentType.dampingY.ToString();
        newMoveSpeed = currentType.moveSpeed.ToString();
        newMoveToWaypoint = currentType.moveToWaypoint;
        newDropProb = currentType.dropProbability.ToString();
        UpdateEnemy();

    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if(index > 0)
            {
                index--;
            }
            else
            {
                index = enemyTypes.Count - 1;
            }
            ChangePrefab();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (index < enemyTypes.Count - 1)
            {
                index++;
            }
            else
            {
                index = 0;
            }
            ChangePrefab();
        }
    }

    void LateUpdate()
    {
        if (currentDummy == null)
        {
            ChangePrefab();
        }
    }
}
