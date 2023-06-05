using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//�������
public class ProduceComponent : MonoBehaviour
{
    public List<ProductAndConsum> productAndConsums=new List<ProductAndConsum>();
    private Dictionary<string, ProduceStation> produceStations = new Dictionary<string, ProduceStation>();
    private Storage storage;

    //List<string> keys=new List<string>();
    
    private void Start()
    {
        storage=GetComponent<Storage>();

        //������Զ������ĳ�����ܻ��õ����Զ���������
        //List<PropertyType> typeList = GetPropertyTypes();
        //foreach(PropertyType type in typeList)
        //{
        //    if(CreateProduceStation(type, out string key))
        //    {
        //        keys.Add(key);
        //    }
        //}
    }

    private void Update()
    {
        //������Զ������ĳ�����ܻ��õ����Զ���������
        //foreach(string key in keys)
        //{
        //    if(Produce(key, out bool canProduce, out int count, out PropertyType type))
        //    {
        //        Debug.Log($"������{count}��{type}��Ʒ");
        //    }
        //}
    }

    public List<PropertyType> GetPropertyTypes()
    {
        return productAndConsums.Select(x => x.propertyType).ToList();
    }

    //��ȡһ�������õ�����λ��
    public bool GetProduceStation(PropertyType propertyType, out string key)
    {
        foreach(string k in produceStations.Keys)
        {
            if (!produceStations[k].isLocked&&produceStations[k].type==propertyType)
            {
                key = k;
                return true;
            }
        }
        return CreateProduceStation(propertyType,out key);
    }

    //����һ������λ��
    private bool CreateProduceStation(PropertyType propertyType,out string key)
    {
        key = null;
        var pac=productAndConsums.Find(x => x.propertyType == propertyType);
        if (pac == null) return false;
        key = Guid.NewGuid().ToString("N");
        produceStations.Add(key,new ProduceStation() { type=propertyType});
        return true;
    }

    public bool RemoveProduceStation(string key)
    {
        if (produceStations.ContainsKey(key))
        {
            if (produceStations[key].isLocked) return false;
            if (produceStations[key].IsProducing) return false;
            produceStations.Remove(key);
            return true;
        }
        return false;
    }

    public void SetLockStation(string key, bool isLock)
    {
        produceStations[key].isLocked = isLock;
    }

    //==================================================================================

    //������Ʒ
    public bool Produce(string key, out bool canProduce, out int count, out PropertyType type)
    {
        count = 0;
        canProduce = false;

        var station = produceStations[key];
        type=station.type;

        if (!station.IsProducing)//�����λ��û�п�ʼ����ô��ʼ������ҵ
        {
            canProduce = StartProduce(station);
            if(!canProduce)return false;
        }
        else
        {
            canProduce = !storage.ArrivedMaxValue(station.type);
            if (!canProduce) return false;
        }

        canProduce = true;
        if (station.Update())//��λ�Ƿ��������
        {
            count = productAndConsums.Find(x => x.propertyType == station.type).count;
            //�������ɹ�����Դ����ֿ�
            storage.Add(station.type, count);
            return true;
        }

        return false;
    }

    //��ʼ����
    private bool StartProduce(ProduceStation station)
    {
        var pac = productAndConsums.Find(x => x.propertyType == station.type);
        if (storage.ArrivedMaxValue(station.type)) return false;
        if (!CustomForProduce(pac)) return false;
        station.SetStart(pac.time);
        return true;
    }

    //�Ƿ�������Դ�ɹ�
    private bool CustomForProduce(ProductAndConsum consum)
    {
        List<PropertyCount> consums = consum.consumsNecessary;
        if(!StorageEnoughConsum(consums))return false;
        foreach(PropertyCount pc in consums)
        {
            storage.Sub(pc.type, pc.count);
            Debug.Log($"����{pc.count}��{pc.type}");
        }

        return true;
    }

    //�Ƿ����㹻�Ŀ��������һ����Դ
    private bool StorageEnoughConsum(List<PropertyCount> list)
    {
        foreach (PropertyCount pc in list)
        {
            if (storage.GetPropertyValue(pc.type) < pc.count) return false;
        }
        return true;
    }


    //�ж���Դ�Ƿ��㹻���ģ��������������Դ
    public bool EnoughConsum(PropertyType type,out List<PropertyCount> forNecessaryCount)
    {
        forNecessaryCount=new List<PropertyCount>();
        var pac = productAndConsums.Find(x => x.propertyType == type);
        bool isEnough = true;
        foreach (PropertyCount pc in pac.consumsNecessary)
        {
            if (storage.GetPropertyValue(pc.type) < pc.count)
            {
                isEnough = false;
                forNecessaryCount.Add(new PropertyCount { type = pc.type, count = pc.count-storage.GetPropertyValue(type) });
            }
        }
        return isEnough;
    }

    
}


[System.Serializable]
public class ProductAndConsum
{
    public PropertyType propertyType;//��������Դ
    public int count;//һ�ο�����������Դ����
    public List<PropertyCount> consumsNecessary = new List<PropertyCount>();//��Ҫ���ĵ���Դ
    public float time;//����һ����Դ��Ҫ��ʱ��
}


[System.Serializable]
public class PropertyCount//��Դ����
{
    public PropertyType type;
    public int count;
}

[System.Serializable]
public class ProduceStation//������λ
{
    public PropertyType type;
    private float timer = 0;
    private bool isProducing = false;

    public bool IsProducing{ get { return isProducing; } }

    public bool isLocked = false;//��������ʾ�е�λ����ʹ��

    public void SetStart(float time)
    {
        this.timer = time;
        isProducing = true;
    }

    public bool Update()
    {
        if (!isProducing)return false;
        timer -= Time.deltaTime;
        if (timer <= 0)//һ���������
        {
            timer = 0;
            isProducing = false;
            return true;
        }
        return false;
    }
}