using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System;

public class Level
{
    public string name;
    public string endGameMessage;
    public string gameOverMessage;
    public string description;
    public string directory;
    public Dictionary<int, Wave> waveDict;
    public List<int> wavesIds;
    public Vector3 leftBorder;
    public Vector3 rightBorder;
    public Vector3 topBorder;
    public Vector3 bottomBorder;
    public Vector3 respawnPos;
    public TypeInfo player;
    public int lifeCount = 5;

    public Level()
    {
        name = "Enter a name";
        endGameMessage = "Enter an end game message";
        gameOverMessage = "Enter a game over message";
        description = "Enter a description";
        wavesIds = new List<int>();
        waveDict = new Dictionary<int, Wave>();
    }

    public void ReadXML(string path)
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(path);
        foreach(XmlNode node in doc)
        {
            if (node.Name != "Level")
            {
                continue;
            }
            name = node.Attributes["name"].Value;
            leftBorder = StringToVector3(node.Attributes["leftBorder"].Value);
            rightBorder = StringToVector3(node.Attributes["rightBorder"].Value);
            topBorder = StringToVector3(node.Attributes["topBorder"].Value);
            bottomBorder = StringToVector3(node.Attributes["bottomBorder"].Value);
            respawnPos = StringToVector3(node.Attributes["respawnPos"].Value);
            player = TypeInfoHolder.enemyTypeDict[int.Parse(node.Attributes["player"].Value)];
            lifeCount = int.Parse(node.Attributes["life"].Value);
            foreach (XmlNode child in node.ChildNodes)
            {
                SetAttributes(child);
            }
        }
    }

    private void SetAttributes(XmlNode node)
    {
        if (node.Name == "EndGameText")
        {
            endGameMessage = node.InnerText;
        }
        else if (node.Name == "GameOverText")
        {
            gameOverMessage = node.InnerText;
        }
        else if(node.Name == "Description")
        {
            description = node.InnerText;
        }
        else if(node.Name == "Wave")
        {
            Wave wave = new Wave();
            wave.name = node.Attributes["name"].Value;
            wave.id = int.Parse(node.Attributes["val"].Value);
            BackgoundHolder bg = new BackgoundHolder(node.Attributes["background"].Value, int.Parse(node.Attributes["backgroundId"].Value));
            wave.background = bg;
            foreach (XmlNode childNode in node.ChildNodes)
            {
                if (childNode.Name == "Enemy")
                {
                    wave.Add(childNode);
                }
                else if (childNode.Name == "Description")
                {
                    wave.description = node.InnerText;
                }
                else if(childNode.Name == "Waypoint")
                {
                    wave.waypointsPos.Add(StringToVector3(childNode.Attributes["pos"].Value));
                }
                else
                {
                    Debug.LogError("Unknown node: " + childNode.Name);
                }
            }
            waveDict.Add(wave.id, wave);
            wavesIds.Add(wave.id);
        }
    }

    public void CreateNewWave()
    {
        int index = 0;
        for (int i = 0; i < 100; i++)
        {
            if (!wavesIds.Contains(i))
            {
                index = i;
                break;
            }
        }
        if (index >= 100)
        {
            Debug.LogError("Too many waves");
            return;
        }

        Wave newWave = new Wave();
        newWave.name = "New Wave";
        newWave.description = "This is a new wave";
        wavesIds.Add(index);
        waveDict.Add(index, newWave);
        newWave.id = index;
        newWave.background = new BackgoundHolder();
    }

    public void DeleteWave(Wave w)
    {
        wavesIds.Remove(w.id);
        waveDict.Remove(w.id);
    }

    public void WriteXML(TypeInfo player = null)
    {
        XmlWriterSettings settings = new XmlWriterSettings();
        settings.Indent = true;
        settings.IndentChars = "\t";
        using (XmlWriter writer = XmlWriter.Create(Application.dataPath + "/Resources/Levels/" + name + ".xml", settings))
        {
            writer.WriteStartDocument();
            writer.WriteStartElement("Level");
            writer.WriteAttributeString("name", name);
            writer.WriteAttributeString("leftBorder", GameObject.FindGameObjectWithTag("LeftBorder").transform.position.ToString());
            writer.WriteAttributeString("rightBorder", GameObject.FindGameObjectWithTag("RightBorder").transform.position.ToString());
            writer.WriteAttributeString("topBorder", GameObject.FindGameObjectWithTag("TopBorder").transform.position.ToString());
            writer.WriteAttributeString("bottomBorder", GameObject.FindGameObjectWithTag("BottomBorder").transform.position.ToString());
            writer.WriteAttributeString("respawnPos", GameObject.FindObjectOfType<Player>().transform.position.ToString());
            if (player != null)
            {
                writer.WriteAttributeString("player", player.id.ToString());
                writer.WriteAttributeString("life", lifeCount.ToString());
            }
            else
            {
                writer.WriteAttributeString("player", "1");
                writer.WriteAttributeString("life", "5");
            }
            writer.WriteStartElement("EndGameText");
            writer.WriteString(endGameMessage);
            writer.WriteEndElement(); //EndGameText
            writer.WriteStartElement("GameOverText");
            writer.WriteString(gameOverMessage);
            writer.WriteEndElement(); //GameOverText
            writer.WriteStartElement("Description");
            writer.WriteAttributeString("tag", "level");
            writer.WriteString(description);
            writer.WriteEndElement(); //Description

            int index = 0;
            foreach(int i in wavesIds)
            {
                Wave w = waveDict[i];
                writer.WriteStartElement("Wave");
                writer.WriteAttributeString("name", w.name);
                writer.WriteAttributeString("val", index.ToString());
                writer.WriteAttributeString("background", w.background.path);
                writer.WriteAttributeString("backgroundId", w.background.id.ToString());
                writer.WriteStartElement("Description");
                writer.WriteAttributeString("tag", "wave");
                writer.WriteString(w.description);
                writer.WriteEndElement(); //Description
                foreach (Vector3 g in w.waypointsPos)
                {
                    if(g == null)
                    {
                        continue;
                    }
                    writer.WriteStartElement("Waypoint");
                    writer.WriteAttributeString("pos", g.ToString());
                    writer.WriteEndElement(); //Waypoint
                }
                foreach(EnemyInfo e in w.enemyInfoList)
                {
                    writer.WriteStartElement("Enemy");
                    writer.WriteAttributeString("typeId", e.info.id.ToString());
                    writer.WriteAttributeString("pos", "(" + e.pos.x.ToString() + "," + e.pos.y.ToString() + ")");
                    writer.WriteAttributeString("rot", "(0.0,0.0,180.0)");
                    writer.WriteEndElement(); //Enemy
                }
                writer.WriteEndElement(); //Wave
                index++;
            }
            writer.WriteEndElement(); //Level
            writer.WriteEndDocument();
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

    private TypeInfoHolder typeInfoHolder;
    private TypeInfoHolder TypeInfoHolder
    {
        get
        {
            if (typeInfoHolder == null)
            {
                typeInfoHolder = new TypeInfoHolder();
            }
            return typeInfoHolder;
        }
    }
}
