using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Storage : MonoBehaviour
{
    public List<PropertyValue> propertyDatas = new List<PropertyValue>();
    //public bool useTotalMaxLimit;//�Ƿ�ʹ���ܵ����ֵ�޶�
    //public int totalMaxLimit;//�ܵ����ֵ

    //����ֵ
    public int Add(PropertyType type,int addtion)
    {
        var property = propertyDatas.Find(x => x.type == type);
        if (property != null)
        {
            return property.AddValue(addtion);
        }
        return 0;
    }

    //����ֵ
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

    //��ȡĳ����Դ����ֵ
    public int GetPropertyValue(PropertyType type)
    {
        var pd = propertyDatas.Find(x => x.type == type);
        if(pd==null)return 0;
        return pd.value;
    }

    //��Դ�Ƿ�ﵽ���ֵ
    public bool ArrivedMaxValue(PropertyType type)
    {
        var pd = propertyDatas.Find(x => x.type == type);
        if (pd == null) return true;
        return pd.ArrivedMaxValue();
    }

    //�Ƿ���ĳ�����͵���Դ
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

    //��ȡ���Է�������
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
    public bool useMaxLimit;//�Ƿ������ֵ�޶�
    public int maxValue;
    public bool useUnLimit;//�Ƿ�������ֵ

    public int GetValue()
    {
        return value;
    }

    public int AddValue(int addtion)
    {
        if (useUnLimit) return addtion;
        int preValue = value;
        value += addtion;
        if (useMaxLimit && value > maxValue) value = maxValue;//�����Ҫ�޶����ֵ����ô�޶����ֵ
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

    //�Ƿ�ﵽ���ֵ
    public bool ArrivedMaxValue()
    {
        return useMaxLimit && value >= maxValue;
    }
}
