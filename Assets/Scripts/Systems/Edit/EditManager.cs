using Components.Grid;
using GameSys.Structure;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditManager : PersistentSingleton<EditManager>
{
    public int minSize;
    public int maxSize;
    public int defaultSize;
    private const string baseGroundId="01";//��������Id
    [Space]
    public Texture2D destroyCursorTexture;

    //����Ĭ�ϳߴ�
    public void SetDefaultSize()
    {
        SetSize(defaultSize);
    }


    //��������ĳߴ�
    public void SetSize(int size)
    {
        size=Mathf.Clamp(size, minSize, maxSize);
        WorldManager.Instance.SetWorldSize(new Vector2Int(size, size));
        WorldManager.Instance.ShowBorder(true);
        SetBaseGround();
    }

    //���
    public void Clear()
    {
        WorldManager.Instance.gridPlacementForStructure.Clear();
        SetBaseGround();
    }

    public void SetRandom()
    {
        Vector2Int count = WorldManager.Instance.gridSystem.grid.GridCount;
        float[,] randomValueArr = GetRandomHeight(count, 0.08f);
        for(int i = 0; i < count.x; i++)
        {
            for(int j = 0; j < count.y; j++)
            {
                if (randomValueArr[i, j] >= 0.4f) PlaceGround("00", new Vector2Int(i, j));
                else PlaceGround(baseGroundId, new Vector2Int(i, j));
            }
        }
    }

    private void SetBaseGround()
    {
        Vector2Int worldSize = WorldManager.Instance.gridSystem.grid.GridCount;
        for(int i = 0; i < worldSize.x; i++)
        {
            for(int j = 0; j < worldSize.y; j++)
            {
                PlaceGround(baseGroundId, new Vector2Int(i, j));
            }
        }
    }



    //���õ�������
    public void PlaceStructure(string id, Vector2Int indexPos, int rotationValue)
    {
        StructureData structureData = StructuresManager.Instance.GetStructureData(id);
        GridTransform prefabGridTransform=structureData.prefab.GetComponent<GridTransform>();
        if (!WorldManager.Instance.gridSystem.grid.RangeInBounds(indexPos, GridTransform.TransRotatedSize(prefabGridTransform.gridSize, rotationValue))) return;

        //�ݻ�ԭ��λ���ϵ�����
        List<GridTransform> desList = WorldManager.Instance.gridPlacementForStructure.GetRangeStructures(indexPos, GridTransform.TransRotatedSize(prefabGridTransform.gridSize, rotationValue));
        foreach(GridTransform gridTransform in desList)
        {
            if (gridTransform) WorldManager.Instance.gridPlacementForStructure.DestroyObjectFromTheMap(gridTransform.gridIndex);
        }
        
        if (WorldManager.Instance.gridPlacementForStructure.CreateObjectOnTheMap(indexPos, structureData.prefab, rotationValue, out GameObject placedObj))
        {
            placedObj.GetComponent<StructureModel>().prefabId = id;
            CheckSetStructureGround(placedObj);
        }
    }

    //���ݵ������������õ�������
    private void CheckSetStructureGround(GameObject obj)
    {
        var arr = obj.GetComponent<GridTransform>().GetPlacedIndexArr();
        List<StructureWithGroundData> datas = StructuresManager.Instance.GetStructureWithGroundData(obj.GetComponent<StructureModel>().prefabId);
        if (datas == null||datas.Count==0) return;
        foreach(var indexPos in arr)
        {
            PlaceGround(datas[0].groundId, indexPos);
        }
    }

    //���õ������壬ͬʱ���н������
    public void PlaceGroundWithCheck(string id, Vector2Int indexPos)
    {
        if (PlaceGround(id, indexPos))
        {
            //���λ�����н��������������壬��ô�ݻٸý�������
            GridTransform gridTransform = WorldManager.Instance.gridPlacementForStructure.GetStructure(indexPos);
            if (gridTransform) WorldManager.Instance.gridPlacementForStructure.DestroyObjectFromTheMap(indexPos);
        }
    }

    //�������ط�������
    public bool PlaceGround(string id, Vector2Int indexPos)
    {
        StructureData structureData = StructuresManager.Instance.GetGroundStructureData(id);
        GridTransform gridTransform =  WorldManager.Instance.gridPlacementForGround.GetStructure(indexPos);
        if(gridTransform) WorldManager.Instance.gridPlacementForGround.DestroyObjectFromTheMap(indexPos);
        if (WorldManager.Instance.gridPlacementForGround.CreateObjectOnTheMap(indexPos, structureData.prefab, 0, out GameObject placedObj))
        {
            placedObj.GetComponent<StructureModel>().prefabId = id;
            return true;
        }
        return false;
    }




    //�ݻ�λ���ϵ�����,��������������壬��ô�ݻٵ����ϵ����壬����ݻٵ�������(�ݻٵķ�ʽ���滻Ϊ�����������壬ͨ����ˮ����)
    public void DestroyObjWithPos(Vector2Int indexPos)
    {
        GridTransform gridTransform1 = WorldManager.Instance.gridPlacementForStructure.GetStructure(indexPos);
        if (gridTransform1 != null)
        {
            WorldManager.Instance.gridPlacementForStructure.DestroyObjectFromTheMap(indexPos);
            return;
        }
        PlaceGround(baseGroundId,indexPos);
    }

    //==================================================================================

    //�����༭������
    public EditController CreateEditController(string id, Transform parent,bool isGround)
    {
        if (isGround) return CreateGroundEditController(id, parent);
        else return CreateStructureEditController(id, parent);
    }

    public EditController CreateGroundEditController(string id, Transform parent)
    {
        GameObject obj = new GameObject("editor");
        obj.transform.parent = parent;
        GroundEditController controller = obj.AddComponent<GroundEditController>();
        controller.id = id;
        StructureData structureData=StructuresManager.Instance.GetGroundStructureData(id);
        controller.modelPrefab = structureData.prefab;
        return controller;
    }

    public EditController CreateStructureEditController(string id, Transform parent)
    {
        GameObject obj = new GameObject("editor");
        obj.transform.parent = parent;
        StructureEditController controller = obj.AddComponent<StructureEditController>();
        controller.id = id;
        StructureData structureData = StructuresManager.Instance.GetStructureData(id);
        controller.modelPrefab=structureData.prefab;
        return controller;
    }

    //==================================================================================

    //�����ݻٿ�����
    public EditDestroyController CreateDestroyController()
    {
        GameObject destroyObj = new GameObject("Destroyer");
        EditDestroyController destroyController = destroyObj.AddComponent<EditDestroyController>();
        destroyController.texture = destroyCursorTexture;
        return destroyController;
    }


    //��ȡ���ֵ
    public float[,] GetRandomHeight(Vector2Int size,float scale)
    {
        float[,] noiseMap = new float[size.x, size.y];
        (float xOffset, float yOffset) = (Random.Range(-10000f, 10000f), Random.Range(-10000f, 10000f));
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                float noiseValue = Mathf.PerlinNoise(x * scale + xOffset, y * scale + yOffset);
                noiseMap[x, y] = noiseValue;
            }
        }

        return noiseMap;
    }
}
