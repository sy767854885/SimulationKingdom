using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameSys.Structure
{
    public class StructuresManager : PersistentSingleton<StructuresManager>
    {
        public List<CatalogueItem> catalogueItems = new List<CatalogueItem>();
        public List<StructureData> structureDatas = new List<StructureData>();
        [Space]
        public List<RoadFixerData> roadFixerDatas = new List<RoadFixerData>();
        [Header("------------------------------------")]
        public List<StructureWithGroundData> structureWithGroundDatas = new List<StructureWithGroundData>();
        public List<StructureData> groundStructureDatas = new List<StructureData>();

        //获取目录列表
        public List<CatalogueItem> GetCatalogues()
        {
            return catalogueItems
                .Select(x => new CatalogueItem() { id = x.id, name = x.name, description = x.description, sprite = x.sprite }).ToList();
        }

        //获取目录下的结构列表
        public List<StructureData> GetCatalogueStructureDatas(string cId)
        {
            return structureDatas.Where(x => x.cId == cId)
                .Select(x => new StructureData() { id = x.id, name = name, prefab = x.prefab, sprite = x.sprite, cId = x.cId }).ToList();
        }

        public List<StructureData> GetStructureDatas()
        {
            return structureDatas.Select(x => new StructureData() { id = x.id, name = name, prefab = x.prefab, sprite = x.sprite, cId = x.cId }).ToList();
        }

        //获取可以用来编辑的物体
        public List<StructureData> GetEditableStructureDatas()
        {
            return structureDatas.Where(x=>x.editable).Select(x => new StructureData() { id = x.id, name = name, prefab = x.prefab, sprite = x.sprite, cId = x.cId }).ToList();
        }

        public StructureData GetStructureData(string id)
        {
            return structureDatas.Find(x => x.id == id);
        }

        //获取适应器
        public StructureFixer GetFixer(string id)
        {
            var roadFixerData = roadFixerDatas.Find(x => x.sId == id);
            if (roadFixerData != null) return roadFixerData.roadFixer;
            return null;
        }

        //==================================================================================
        public List<StructureData> GetGroundStructureDatas()
        {
            return groundStructureDatas
                .Select(x => new StructureData() { id = x.id, name = name, prefab = x.prefab, sprite = x.sprite, cId = x.cId }).ToList();
        }

        public List<StructureWithGroundData> GetStructureWithGroundData(string id)
        {
            return structureWithGroundDatas.Where(x => x.structureId == id).Select(x => new StructureWithGroundData() { structureId = x.structureId, groundId = x.groundId }).ToList();
        }

        public StructureData GetGroundStructureData(string id)
        {
            return groundStructureDatas.Find(x => x.id == id);
        }
    }

    /// <summary>
    /// 建筑目录
    /// </summary>
    [System.Serializable]
    public class CatalogueItem
    {
        public string id;
        public string name;
        public string description;
        public Sprite sprite;
    }

    //建造列表
    [System.Serializable]
    public class StructureData
    {
        public string id;
        public string name;
        public string description;
        public GameObject prefab;
        public Sprite sprite;
        public string cId;
        public bool editable;
    }

    //道路匹配器
    [System.Serializable]
    public class RoadFixerData
    {
        public string sId;
        public RoadFixer roadFixer;
    }

    //地形物体配对，编辑时，如果放置该物体，那么该物体下的地形物体必须是groundId对应的物体
    [System.Serializable]
    public class StructureWithGroundData
    {
        public string structureId;
        public string groundId;
    }
}
