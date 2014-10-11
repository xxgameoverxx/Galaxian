using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class TypeInfoHolder : MonoBehaviour {

    public Dictionary<int, TypeInfo> typeInfoDict = new Dictionary<int, TypeInfo>();

    void Awake()
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
                }
                catch
                {
                    Debug.LogError("Same id already exists in dictionary: " + typeInfo.id);
                }
            }
        }
    }
}
