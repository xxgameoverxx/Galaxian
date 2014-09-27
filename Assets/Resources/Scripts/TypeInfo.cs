using UnityEngine;
using System.Collections;
using System.Xml;

public class TypeInfo
{
    public float amplitudeX;
    public float amplitudeY;
    public float shootProbability;
    public float selfDestroyProbability;
    public float dampingX;
    public float dampingY;
    public float health;
    public float maxHealt;
    public float energy;
    public float energyRegenSpeed;
    public float maxEnergy;
    public float moveSpeed;
    public float hurtCooldown;
    public float dropProbability;
    public int type;
    public string prefab;
    public string name;
    public bool moveToWaypoint;

    public void ReadInfo(XmlNode node)
    {
        foreach(XmlAttribute att in node.Attributes)
        {
            if(att.Name == "type")
            {
                type = int.Parse(att.Value);
            }
            else if(att.Name == "name")
            {
                name = att.Value;
            }
            else if(att.Name == "prefab")
            {
                prefab = "Prefabs/Enemies/" + att.Value;
            }
            else
            {
                Debug.LogError("Attribute of a node is not found! " + att.Name);
            }
        }
        foreach(XmlNode xnode in node.ChildNodes)
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
            else if(xnode.Name == "ShootProbability")
            {
                shootProbability = float.Parse(xnode.Attributes["value"].Value);
            }
            else if(xnode.Name == "Damping")
            {
                dampingX = float.Parse(xnode.Attributes["x"].Value);
                dampingY = float.Parse(xnode.Attributes["y"].Value);
            }
            else if(xnode.Name == "Health")
            {
                health = float.Parse(xnode.Attributes["value"].Value);
                maxHealt = float.Parse(xnode.Attributes["maxValue"].Value);
            }
            else if (xnode.Name == "Energy")
            {
                energy = float.Parse(xnode.Attributes["value"].Value);
                maxEnergy = float.Parse(xnode.Attributes["maxValue"].Value);
                energyRegenSpeed = float.Parse(xnode.Attributes["regenSpeed"].Value);
            }
            else if(xnode.Name == "MoveSpeed")
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
            else
            {
                Debug.LogError("Unknown node: " + xnode.Name + ". Child of " + node.Name);
            }
        }
    }
}
