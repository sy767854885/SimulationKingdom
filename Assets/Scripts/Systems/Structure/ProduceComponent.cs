using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//生产组件
public class ProduceComponent : MonoBehaviour
{
    public List<ProductAndConsum> productAndConsums=new List<ProductAndConsum>();
    private Dictionary<string, ProduceStation> produceStations = new Dictionary<string, ProduceStation>();
    private Storage storage;

    //List<string> keys=new List<string>();
    
    private void Start()
    {
        storage=GetComponent<Storage>();

        //如果是自动产生的车间可能会用到此自动生产代码
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
        //如果是自动产生的车间可能会用到此自动生产代码
        //foreach(string key in keys)
        //{
        //    if(Produce(key, out bool canProduce, out int count, out PropertyType type))
        //    {
        //        Debug.Log($"生产了{count}个{type}产品");
        //    }
        //}
    }

    public List<PropertyType> GetPropertyTypes()
    {
        return productAndConsums.Select(x => x.propertyType).ToList();
    }

    //获取一个可以用的生产位置
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

    //创建一个生产位置
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

    //生产产品
    public bool Produce(string key, out bool canProduce, out int count, out PropertyType type)
    {
        count = 0;
        canProduce = false;

        var station = produceStations[key];
        type=station.type;

        if (!station.IsProducing)//如果工位还没有开始，那么开始进行作业
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
        if (station.Update())//工位是否完成生产
        {
            count = productAndConsums.Find(x => x.propertyType == station.type).count;
            //将生产成功得资源存入仓库
            storage.Add(station.type, count);
            return true;
        }

        return false;
    }

    //开始生产
    private bool StartProduce(ProduceStation station)
    {
        var pac = productAndConsums.Find(x => x.propertyType == station.type);
        if (storage.ArrivedMaxValue(station.type)) return false;
        if (!CustomForProduce(pac)) return false;
        station.SetStart(pac.time);
        return true;
    }

    //是否消耗资源成功
    private bool CustomForProduce(ProductAndConsum consum)
    {
        List<PropertyCount> consums = consum.consumsNecessary;
        if(!StorageEnoughConsum(consums))return false;
        foreach(PropertyCount pc in consums)
        {
            storage.Sub(pc.type, pc.count);
            Debug.Log($"消耗{pc.count}份{pc.type}");
        }

        return true;
    }

    //是否有足够的库存来消耗一组资源
    private bool StorageEnoughConsum(List<PropertyCount> list)
    {
        foreach (PropertyCount pc in list)
        {
            if (storage.GetPropertyValue(pc.type) < pc.count) return false;
        }
        return true;
    }


    //判断资源是否足够消耗，并返回所需的资源
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
    public PropertyType propertyType;//生产的资源
    public int count;//一次可以生产的资源数量
    public List<PropertyCount> consumsNecessary = new List<PropertyCount>();//需要消耗的资源
    public float time;//生产一次资源需要的时间
}


[System.Serializable]
public class PropertyCount//资源消耗
{
    public PropertyType type;
    public int count;
}

[System.Serializable]
public class ProduceStation//生产工位
{
    public PropertyType type;
    private float timer = 0;
    private bool isProducing = false;

    public bool IsProducing{ get { return isProducing; } }

    public bool isLocked = false;//加锁，表示有单位正在使用

    public void SetStart(float time)
    {
        this.timer = time;
        isProducing = true;
    }

    public bool Update()
    {
        if (!isProducing)return false;
        timer -= Time.deltaTime;
        if (timer <= 0)//一次生产完成
        {
            timer = 0;
            isProducing = false;
            return true;
        }
        return false;
    }
}