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

    //��������ĳߴ�,�������óߴ��ʱ��������������嶼�ᱻ���
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

    //���ü��ƽ��
    public void SetCheckPlane()
    {
        Vector2 size = gridSystem.grid.GetSize();
        checkPlane.localScale = new Vector3(size.x, 1, size.y);
        checkPlane.localPosition = gridSystem.grid.GetCenterPosition();
    }


    //==================================================================================
    //��ʾ�߿�
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

    //��ȡλ���ϵ�����
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


    //����Id��ȡ����
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
    //��Ӿ���,ͬʱ���䷿��
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

    //����ID���ؾ���
    public GameObject GetResidentWithId(string id)
    {
        if(residents.Keys.Contains(id))return residents[id];
        else return null;
    }

    //��ȡ�����б�
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

    //��������
    public SaveDataSerialization ExportData()
    {
        SaveDataSerialization saveData = new SaveDataSerialization();

        //��������Ĵ�С
        saveData.worldSize = new Vector2IntSerialization(gridSystem.grid.GridCount);

        //���ϵ�λ
        foreach (var gridTransform in gridPlacementForStructure.GetAllStructures())
        {
            saveData.AddStructureData(gridTransform.GetComponent<StructureModel>().prefabId, gridTransform.gridIndex, gridTransform.RotationValue);
        }

        //���ε�λ
        foreach(var gridTransform in gridPlacementForGround.GetAllStructures())
        {
            saveData.AddGroundData(gridTransform.GetComponent<StructureModel>().prefabId, gridTransform.gridIndex, gridTransform.RotationValue);
        }

        return saveData;
    }
}
