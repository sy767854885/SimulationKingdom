using Components.Grid;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorldManager : PersistentSingleton<WorldManager>
{
    public GridSystem gridSystem;
    public GridPlacement gridPlacementForStructure;
    public GridPlacement gridPlacementForGround;

    [Space]
    public Transform checkPlane;

    [Space]
    public Vector2 borderSectionSize;
    public Transform bordersParent;
    public GameObject leftBorder;
    public GameObject upBorder;
    public GameObject rightBorder;
    public GameObject downBorder;

    private Dictionary<string, GameObject> residents = new Dictionary<string, GameObject>();

    //设置世界的尺寸,重新设置尺寸的时候，世界的所有物体都会被清空
    public void SetWorldSize(Vector2Int size)
    {
        gridSystem.SetSize(size.x, size.y, 1);
        Clear();
        SetCheckPlane();
    }

    public void Clear()
    {
        ClearResidents();
        gridPlacementForStructure.Clear();
        gridPlacementForGround.Clear();
    }

    //==================================================================================

    //设置检测平面
    public void SetCheckPlane()
    {
        Vector2 size = gridSystem.grid.GetSize();
        checkPlane.localScale = new Vector3(size.x, 1, size.y);
        checkPlane.localPosition = gridSystem.grid.GetCenterPosition();
    }


    //==================================================================================
    //显示边框
    public void ShowBorder(bool isShow)
    {
        bordersParent.gameObject.SetActive(isShow);
        UpdateBorderSize();
    }

    private void UpdateBorderSize()
    {
        SetBorder(gridSystem.grid.GetCenterPosition(), gridSystem.grid.GetSize(),borderSectionSize);
    }

    private void SetBorder(Vector3 centerPos,Vector2 rangeSize,Vector2 sectionSize)
    {
        bordersParent.position = centerPos;
        leftBorder.transform.localScale = new Vector3(sectionSize.x, sectionSize.y, rangeSize.y+sectionSize.x*2);
        leftBorder.transform.localPosition = new Vector3(-(rangeSize.x / 2+sectionSize.x/2), 0, 0);

        rightBorder.transform.localScale = new Vector3(sectionSize.x, sectionSize.y, rangeSize.y+sectionSize.x*2);
        rightBorder.transform.localPosition = new Vector3(rangeSize.x / 2+sectionSize.x/2, 0, 0);

        upBorder.transform.localScale = new Vector3(rangeSize.x+sectionSize.x*2, sectionSize.y, sectionSize.x);
        upBorder.transform.localPosition = new Vector3(0, 0, rangeSize.y / 2+sectionSize.x/2);

        downBorder.transform.localScale = new Vector3(rangeSize.x+sectionSize.x*2, sectionSize.y, sectionSize.x);
        downBorder.transform.localPosition = new Vector3(0, 0, -rangeSize.y / 2-sectionSize.x/2);
    }




    //==================================================================================

    //获取位置上的物体
    public GridTransform GetGridTransformInPos(Vector2Int indexPos, out bool isStructure)
    {
        isStructure = false;
        GridTransform gridTransform = gridPlacementForStructure.GetStructure(indexPos);
        if (gridSystem)
        {
            isStructure = true;
            return gridTransform;
        }
        gridTransform = gridPlacementForGround.GetStructure(indexPos);
        return gridTransform;
    }


    //根据Id获取物体
    public GridTransform GetGridTransformWithId(string id)
    {
        return gridPlacementForStructure.GetAllStructures().Find(x => {
            IdComponent idComponent = x.GetComponent<IdComponent>();
            if(!idComponent)return false;
            if(idComponent.id==id)return true;
            return false;
        });
    }

    //==================================================================================
    //添加居民,同时分配房子
    public void AddResident(GameObject resident)
    {
        residents.Add(resident.GetComponent<IdComponent>().id, resident);
        List<GridTransform> gridTransforms = gridPlacementForStructure.GetAllStructures();
        List<GridTransform> houses = gridTransforms.Where(x => x.gameObject.tag == "House").ToList();
        foreach(GridTransform gt in houses)
        {
            if (gt.GetComponent<House>().AddOwners(resident)) return;
        }
    }

    //根据ID返回居民
    public GameObject GetResidentWithId(string id)
    {
        if(residents.Keys.Contains(id))return residents[id];
        else return null;
    }

    //获取居民列表
    public List<GameObject> GetResidents()
    {
        return residents.Values.Select(x => x).ToList();
    }

    public int ResidentCount()
    {
        return residents.Values.Count;
    }

    private void ClearResidents()
    {
        foreach(var res in residents.Values)
        {
            if (res) continue;
            Destroy(res);
        }
        residents.Clear();
    }
    //==================================================================================

    //导出数据
    public SaveDataSerialization ExportData()
    {
        SaveDataSerialization saveData = new SaveDataSerialization();

        //保存世界的大小
        saveData.worldSize = new Vector2IntSerialization(gridSystem.grid.GridCount);

        //地上单位
        foreach (var gridTransform in gridPlacementForStructure.GetAllStructures())
        {
            saveData.AddStructureData(gridTransform.GetComponent<StructureModel>().prefabId, gridTransform.gridIndex, gridTransform.RotationValue);
        }

        //地形单位
        foreach(var gridTransform in gridPlacementForGround.GetAllStructures())
        {
            saveData.AddGroundData(gridTransform.GetComponent<StructureModel>().prefabId, gridTransform.gridIndex, gridTransform.RotationValue);
        }

        return saveData;
    }
}
