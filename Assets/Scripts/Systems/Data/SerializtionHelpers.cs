using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveDataSerialization
{
    public Vector2IntSerialization worldSize;//地图大小

    public List<StructureDataSerialization> structuresData = new List<StructureDataSerialization>();//地面建筑物体数据

    public List<StructureDataSerialization> groundsData=new List<StructureDataSerialization> ();//地形物体数据

    //添加建筑
    public void AddStructureData(string prefabId, Vector2Int position, int rotationValue)
    {
        structuresData.Add(new StructureDataSerialization(prefabId, position, rotationValue));
    }

    //添加地形
    public void AddGroundData(string prefabId,Vector2Int position, int rotationValue)
    {
        groundsData.Add(new StructureDataSerialization(prefabId, position, rotationValue));
    }
}

[Serializable]
public class StructureDataSerialization
{
    public string prefabId;
    public Vector2IntSerialization position;
    public int rotationValue;

    public StructureDataSerialization(string prefabId,Vector2Int position,int rotationValue)
    {
        this.prefabId = prefabId;
        this.position = new Vector2IntSerialization(position);
        this.rotationValue = rotationValue;
    }
}

[Serializable]
public class Vector2IntSerialization
{
    public int x, y;

    public Vector2IntSerialization(Vector2Int position)
    {
        this.x = position.x;
        this.y = position.y;
    }

    public Vector2Int GetValue()
    {
        return new Vector2Int(x, y);
    }
}
