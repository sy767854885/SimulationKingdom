using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Storage : MonoBehaviour
{
    public List<PropertyValue> propertyDatas = new List<PropertyValue>();
    //public bool useTotalMaxLimit;//是否使用总的最大值限定
    //public int totalMaxLimit;//总的最大值

    //增加值
    public int Add(PropertyType type,int addtion)
    {
        var property = propertyDatas.Find(x => x.type == type);
        if (property != null)
        {
            return property.AddValue(addtion);
        }
        return 0;
    }

    //减少值
    public int Sub(PropertyType type,int sub)
    {
        var property = propertyDatas.Find(x => x.type == type);
        if (property != null)
        {
            return property.SubValue(sub);
        }else
        {
            return 0;
        }
    }

    //获取某项资源的数值
    public int GetPropertyValue(PropertyType type)
    {
        var pd = propertyDatas.Find(x => x.type == type);
        if(pd==null)return 0;
        return pd.value;
    }

    //资源是否达到最大值
    public bool ArrivedMaxValue(PropertyType type)
    {
        var pd = propertyDatas.Find(x => x.type == type);
        if (pd == null) return true;
        return pd.ArrivedMaxValue();
    }

    //是否有某个类型的资源
    public bool HasPropertyType(PropertyType type)
    {
        return propertyDatas.Find(x => x.type == type) != null;
    }

    public int GetMaxValue(PropertyType type)
    {
        var pd = propertyDatas.Find(x => x.type == type);
        if (pd == null) return 0;
        return pd.maxValue;
    }

    //获取可以放置数量
    public int CanAddtionValue(PropertyType type)
    {
        var pd = propertyDatas.Find(x => x.type == type);
        if (pd == null) return 0;
        return pd.maxValue - pd.value;
    }
}

[System.Serializable]
public class PropertyValue
{
    public PropertyType type;
    public int value;
    public bool useMaxLimit;//是否有最大值限定
    public int maxValue;
    public bool useUnLimit;//是否是无限值

    public int GetValue()
    {
        return value;
    }

    public int AddValue(int addtion)
    {
        if (useUnLimit) return addtion;
        int preValue = value;
        value += addtion;
        if (useMaxLimit && value > maxValue) value = maxValue;//如果需要限定最大值，那么限定最大值
        int addedValue = value - preValue;
        return addedValue;
    }

    public int SubValue(int sub)
    {
        if(useUnLimit)return sub;
        int preValue = value;
        value-=sub;
        if(value<=0)value = 0;
        return preValue - value;
    }

    //是否达到最大值
    public bool ArrivedMaxValue()
    {
        return useMaxLimit && value >= maxValue;
    }
}
