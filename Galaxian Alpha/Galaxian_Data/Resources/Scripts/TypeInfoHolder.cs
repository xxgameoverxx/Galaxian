using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class TypeInfoHolder
{

    public Dictionary<int, TypeInfo> typeInfoDict = new Dictionary<int, TypeInfo>();
    public Dictionary<int, TypeInfo> enemyTypeDict = new Dictionary<int, TypeInfo>();
    public Dictionary<int, TypeInfo> itemTypeDict = new Dictionary<int, TypeInfo>();
    public List<int> enemyIds = new List<int>();
    public List<int> itemIds = new List<int>();
    public List<Vector3> waypoints = new List<Vector3>();

    public TypeInfoHolder()
    {
        XmlDocument typeDoc = new XmlDocument();
        typeDoc.Load(Application.dataPath + "/Resources/TypeInfo.xml");
        foreach (XmlNode node in typeDoc.ChildNodes)
        {
            foreach (XmlNode childNode in node.ChildNodes)
            {
                TypeInfo typeInfo = new TypeInfo();
                typeInfo.ReadInfo(childNode);

                try
                {
                    typeInfoDict.Add(typeInfo.id, typeInfo);
                    if(typeInfo.id < 100)
                    {
                        enemyTypeDict.Add(typeInfo.id, typeInfo);
                        enemyIds.Add(typeInfo.id);
                    }
                    else
                    {
                        itemTypeDict.Add(typeInfo.id, typeInfo);
                        itemIds.Add(typeInfo.id);
                    }
                }
                catch
                {
                    Debug.LogError("Same id already exists in dictionary: " + typeInfo.id);
                }
            }
        }
    }

    public void UpdateWaypoints(List<Vector3> wp = null)
    {
        waypoints.Clear();
        if(wp != null)
        {
            waypoints = wp;
        }
        else
        {
            foreach(GameObject g in GameObject.FindGameObjectsWithTag("Waypoint"))
            {
                waypoints.Add(g.transform.position);
            }
        }
    }
}
