using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PropertiesContainer
{
    private Dictionary<string, int> propertyValues = new Dictionary<string, int>();

    //�����ֵ�
    public void Build(List<string> propertyIds)
    {
        propertyValues.Clear();
        foreach (var id in propertyIds)
        {
            if (!propertyValues.ContainsKey(id))
                propertyValues.Add(id, 0);
        }
    }

    //��ȡֵ
    public int GetValue(string id)
    {
        if (!propertyValues.ContainsKey(id)) return 0;
        return propertyValues[id];
    }

    //����ֵ
    public bool SetValue(string id, int value)
    {
        if (!propertyValues.ContainsKey(id)) return false;
        propertyValues[id] = value;
        return true;
    }

    //����ֵ
    public bool AddValue(string id, int addtionValue)
    {
        if (addtionValue < 0) return false;
        if (!propertyValues.ContainsKey(id)) return false;
        propertyValues[id] += addtionValue;
        return true;
    }

    //����ֵ
    public int SubValue(string id, int substractValue)
    {
        if (substractValue < 0) return 0;
        int actuallySub = substractValue;
        if (propertyValues[id] < substractValue)
            actuallySub = propertyValues[id];

        propertyValues[id] -= actuallySub;
        return actuallySub;
    }

    //���ֵ
    public void Clear()
    {
        foreach (var id in propertyValues.Keys)
        {
            propertyValues[id] = 0;
        }
    }

    //���زƲ��б�
    public List<(string id,int value)> GetProperties()
    {
        return propertyValues.Select(x => (x.Key,x.Value)).ToList();
    }
}
