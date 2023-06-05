using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Components.Grid
{
    public class GridPlacement : MonoBehaviour
    {
        public GridSystem gridSystem;

        public Dictionary<Vector2Int, GridTransform> structureDictionary = new Dictionary<Vector2Int, GridTransform>();



        //获取位置下的物体
        public GridTransform GetStructure(Vector2Int indexPos)
        {
            if (structureDictionary.ContainsKey(indexPos)) return structureDictionary[indexPos];
            return null;
        }

        //获取所有对象
        public List<GridTransform> GetAllStructures()
        {
            List<GridTransform> list = new List<GridTransform>();
            foreach (GridTransform g in structureDictionary.Values)
            {
                list.Add(g);
            }
            return list;
        }

        //获取网格范围的所有物体
        public List<GridTransform> GetRangeStructures(Vector2Int indexPos,Vector2Int rangeSize)
        {
            List<GridTransform> list = new List<GridTransform>();
            var arr = Grid.GetSquareIndexArr(indexPos, rangeSize);
            foreach(var v in arr)
            {
                GridTransform g= GetStructure(v);
                if (!g || list.Contains(g)) continue;
                list.Add(g);
            }
            return list;
        }

        //==================================================================================

        //放置物体到地图上，同时重置位置和旋转
        public bool RegistObjectOnTheMap(GameObject obj)
        {
            GridTransform gridTransform = obj.GetComponent<GridTransform>();
            if (!CheckSquareCanPlace(gridTransform.gridIndex, gridTransform.LocalGridSize)) return false;
            gridSystem.PlaceGridTransform(gridTransform);
            RegistGridTransform(gridTransform);
            return true;
        }

        //放置物体
        internal bool CreateObjectOnTheMap(Vector2Int indexPos, GameObject structurePrefab, int rotationValue, out GameObject placedObj)
        {
            placedObj = null;
            Vector2Int gridZoneSize = GridTransform.TransRotatedSize(structurePrefab.GetComponent<GridTransform>().gridSize, rotationValue);
            if (!CheckSquareCanPlace(indexPos, gridZoneSize)) return false;

            GameObject obj = Instantiate(structurePrefab);
            GridTransform gridTransform = obj.GetComponent<GridTransform>();
            gridTransform.gridIndex = indexPos;
            gridTransform.SetDir(rotationValue);

            gridSystem.PlaceGridTransform(gridTransform);

            RegistGridTransform(gridTransform);
            placedObj = obj;
            return true;
        }

        //从地图上销毁物体
        public bool DestroyObjectFromTheMap(Vector2Int indexPos,Action<GameObject> onStartDestroy=null)
        {
            if (!structureDictionary.ContainsKey(indexPos)) return false;
            GridTransform gridTransform = structureDictionary[indexPos];
            onStartDestroy?.Invoke(gridSystem.gameObject);
            UnRegistGridTransform(gridTransform);
            Destroy(gridTransform.gameObject);
            return true;
        }

        //从地图上销毁物体
        public bool DestroyObjectFromTheMap(GridTransform gridTransform, Action<GameObject> onStartDestroy = null)
        {
            if (!structureDictionary.ContainsValue(gridTransform)) return false;
            onStartDestroy?.Invoke(gridSystem.gameObject);
            UnRegistGridTransform(gridTransform);
            Destroy(gridTransform.gameObject);
            return true;
        }

        //清空
        public void Clear()
        {
            foreach (var o in structureDictionary.Values)
            {
                if (o) Destroy(o.gameObject);
            }
            structureDictionary.Clear();
        }


        //将物体注册到网格字典里
        private void RegistGridTransform(GridTransform gridTransform)
        {
            Vector2Int[,] arr = gridSystem.GetGridTransformHoldArr(gridTransform);
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                for (int j = 0; j < arr.GetLength(1); j++)
                {
                    structureDictionary.Add(arr[i, j], gridTransform);
                }
            }
        }

        //从网格字典里面注销物体
        private void UnRegistGridTransform(GridTransform gridTransform)
        {
            Vector2Int[,] arr = gridSystem.GetGridTransformHoldArr(gridTransform);
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                for (int j = 0; j < arr.GetLength(1); j++)
                {
                    structureDictionary.Remove(arr[i, j]);
                }
            }
        }



        //检查方块区域内是否可以放置
        private bool CheckSquareCanPlace(Vector2Int indexPos, Vector2Int count)
        {
            if (!gridSystem.grid.RangeInBounds(indexPos, count)) return false;
            for (int i = 0; i < count.x; i++)
            {
                for (int j = 0; j < count.y; j++)
                {
                    Vector2Int index = indexPos + new Vector2Int(i, j);
                    if (structureDictionary.ContainsKey(index)) return false;
                }
            }
            return true;
        }

        //根据类型获取相邻得格子上的物体
        //TODO 更改此代码，将StructureType移除，此类中不再依赖 type，换成条件委托
        internal List<GridTransform> GetNeighboursOfTypeFor(Vector2Int indexPos, StructureType type)
        {
            var neighbourVertices = Components.Grid.Grid.GetAllAdjacentIndexsNoBound(indexPos.x, indexPos.y);
            List<GridTransform> list = new List<GridTransform>() { null, null, null, null };
            for (int i = 0; i < neighbourVertices.Count; i++)
            {
                GridTransform t = GetStructure(neighbourVertices[i]);
                list[i] = t != null && t.GetComponent<StructureModel>().structureType == type ? t : null;
            }
            return list;
        }

        //重置物体
        public bool ModifyStructureModel(Vector2Int indexPos, GameObject newPrefab, int rotationValue, out GameObject placedObj)
        {
            placedObj = null;
            if (!structureDictionary.ContainsKey(indexPos)) return false;
            Vector2Int gridZoneSize = GridTransform.TransRotatedSize(newPrefab.GetComponent<GridTransform>().gridSize, rotationValue);
            GridTransform originalT = GetStructure(indexPos);
            if (gridZoneSize.x <= originalT.LocalGridSize.x && gridZoneSize.y <= originalT.LocalGridSize.y)
            {
                DestroyObjectFromTheMap(indexPos);
                return CreateObjectOnTheMap(indexPos, newPrefab, rotationValue, out placedObj);
            }
            return false;
        }
    }
}

