using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System;

public class EnemyInfo
{
    private TypeInfoHolder typeInfoHolder;
    private Dictionary<int, TypeInfo> typeIdDict;
    private Dictionary<int, TypeInfo> TypeIdDict
    {
        get
        {
            if(typeInfoHolder == null)
            {
                typeInfoHolder = new TypeInfoHolder();
            }
            if(typeIdDict == null)
            {
                typeIdDict = typeInfoHolder.typeInfoDict;
            }
            return typeIdDict;
        }
    }


    public Vector2 pos;
    public Quaternion rot;
    public TypeInfo info;
    
    public void ReadXml(XmlNode node)
    {
        info = TypeIdDict[int.Parse(node.Attributes["typeId"].Value)];
        pos = StringToVector2(node.Attributes["pos"].Value);
        rot = Quaternion.Euler(StringToVector3(node.Attributes["rot"].Value));
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
}
