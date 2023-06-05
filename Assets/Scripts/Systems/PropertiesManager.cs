using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PropertiesManager : PersistentSingleton<PropertiesManager>
{
    public List<PropertyData> properties = new List<PropertyData>();

    private Dictionary<PropertyType,int> propertyValues=new Dictionary<PropertyType,int>();

    public PropertyData GetPropertyConfigData(PropertyType type)
    {
        return properties.Find(x => x.type == type);
    }




    //建立字典
    public void Build()
    {
        propertyValues.Clear();
        foreach (var data in properties)
        {
            propertyValues.Add(data.type, 0);
        }
    }

    //获取值
    public int GetValue(PropertyType type)
    {
        if(!propertyValues.ContainsKey(type))return 0;
        return propertyValues[type];
    }

    //设置值
    public void SetValue(PropertyType type,int value)
    {
        if (!propertyValues.ContainsKey(type)) return;
        propertyValues[type] = value;
    }

    //增加值
    public void AddValue(PropertyType type,int addtionValue)
    {
        if (addtionValue < 0) return;
        if (!propertyValues.ContainsKey(type)) return;
        propertyValues[type] += addtionValue;
    }

    //减少值
    public int SubValue(PropertyType type,int substractValue)
    {
        if (substractValue < 0) return 0;
        int actuallySub = substractValue;
        if (propertyValues[type] < substractValue) 
            actuallySub = propertyValues[type];

        propertyValues[type] -= actuallySub;
        return actuallySub;
    }

    //清空值
    public void Clear()
    {
        foreach (var id in propertyValues.Keys)
        {
            propertyValues[id] = 0;
        }
    }

    public List<PropertyData> GetProperties()
    {
        return properties.Select(x => new PropertyData { type = x.type, name = x.name, description = x.description, icon = x.icon }).ToList();
    }
}

[System.Serializable]
public class PropertyData
{
    public PropertyType type;
    public string name;
    public string description;
    public Sprite icon;
}