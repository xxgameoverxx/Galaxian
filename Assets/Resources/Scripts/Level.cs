using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class Level
{
    public string name;
    public string endGameMessage;
    public string gameOverMessage;
    public Dictionary<int, Wave> waveDict;
    public List<int> wavesIds;
    public Vector2 leftBorder;
    public Vector2 rightBorder;
    public Vector2 topBorder;
    public Vector2 bottomBorder;


    public Level()
    {
        name = "Enter a name";
        endGameMessage = "Enter an end game message";
        gameOverMessage = "Enter a game over message";
        wavesIds = new List<int>();
        waveDict = new Dictionary<int, Wave>();
    }

    public void ReadXML(string path)
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(path);
        foreach(XmlNode node in doc)
        {
            name = node.Attributes["name"].Value;
            foreach(XmlNode child in node.ChildNodes)
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
        else if(node.Name == "Wave")
        {
            Wave wave = new Wave();
            wave.name = node.Attributes["name"].Value;
            foreach(XmlNode childNode in node.ChildNodes)
            {
                if (childNode.Name == "Enemy")
                {
                    wave.Add(childNode);
                }
                else if (childNode.Name == "Description")
                {
                    wave.description = node.InnerText;
                }
                else
                {
                    Debug.LogError("Unknown node: " + node.Name);
                }
            }
            waveDict.Add(int.Parse(node.Attributes["val"].Value), wave);
            wavesIds.Add(int.Parse(node.Attributes["val"].Value));
        }
    }
}
