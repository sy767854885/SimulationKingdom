using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PropertiesContainer
{
    private Dictionary<string, int> propertyValues = new Dictionary<string, int>();

    //建立字典
    public void Build(List<string> propertyIds)
    {
        propertyValues.Clear();
        foreach (var id in propertyIds)
        {
            if (!propertyValues.ContainsKey(id))
                propertyValues.Add(id, 0);
        }
    }

    //获取值
    public int GetValue(string id)
    {
        if (!propertyValues.ContainsKey(id)) return 0;
        return propertyValues[id];
    }

    //设置值
    public bool SetValue(string id, int value)
    {
        if (!propertyValues.ContainsKey(id)) return false;
        propertyValues[id] = value;
        return true;
    }

    //增加值
    public bool AddValue(string id, int addtionValue)
    {
        if (addtionValue < 0) return false;
        if (!propertyValues.ContainsKey(id)) return false;
        propertyValues[id] += addtionValue;
        return true;
    }

    //减少值
    public int SubValue(string id, int substractValue)
    {
        if (substractValue < 0) return 0;
        int actuallySub = substractValue;
        if (propertyValues[id] < substractValue)
            actuallySub = propertyValues[id];

        propertyValues[id] -= actuallySub;
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

    //返回财产列表
    public List<(string id,int value)> GetProperties()
    {
        return propertyValues.Select(x => (x.Key,x.Value)).ToList();
    }
}
