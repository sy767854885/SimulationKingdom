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



        //��ȡλ���µ�����
        public GridTransform GetStructure(Vector2Int indexPos)
        {
            if (structureDictionary.ContainsKey(indexPos)) return structureDictionary[indexPos];
            return null;
        }

        //��ȡ���ж���
        public List<GridTransform> GetAllStructures()
        {
            List<GridTransform> list = new List<GridTransform>();
            foreach (GridTransform g in structureDictionary.Values)
            {
                list.Add(g);
            }
            return list;
        }

        //��ȡ����Χ����������
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

        //�������嵽��ͼ�ϣ�ͬʱ����λ�ú���ת
        public bool RegistObjectOnTheMap(GameObject obj)
        {
            GridTransform gridTransform = obj.GetComponent<GridTransform>();
            if (!CheckSquareCanPlace(gridTransform.gridIndex, gridTransform.LocalGridSize)) return false;
            gridSystem.PlaceGridTransform(gridTransform);
            RegistGridTransform(gridTransform);
            return true;
        }

        //��������
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

        //�ӵ�ͼ����������
        public bool DestroyObjectFromTheMap(Vector2Int indexPos,Action<GameObject> onStartDestroy=null)
        {
            if (!structureDictionary.ContainsKey(indexPos)) return false;
            GridTransform gridTransform = structureDictionary[indexPos];
            onStartDestroy?.Invoke(gridSystem.gameObject);
            UnRegistGridTransform(gridTransform);
            Destroy(gridTransform.gameObject);
            return true;
        }

        //�ӵ�ͼ����������
        public bool DestroyObjectFromTheMap(GridTransform gridTransform, Action<GameObject> onStartDestroy = null)
        {
            if (!structureDictionary.ContainsValue(gridTransform)) return false;
            onStartDestroy?.Invoke(gridSystem.gameObject);
            UnRegistGridTransform(gridTransform);
            Destroy(gridTransform.gameObject);
            return true;
        }

        //���
        public void Clear()
        {
            foreach (var o in structureDictionary.Values)
            {
                if (o) Destroy(o.gameObject);
            }
            structureDictionary.Clear();
        }


        //������ע�ᵽ�����ֵ���
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

        //�������ֵ�����ע������
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



        //��鷽���������Ƿ���Է���
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

        //�������ͻ�ȡ���ڵø����ϵ�����
        //TODO ���Ĵ˴��룬��StructureType�Ƴ��������в������� type����������ί��
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

        //��������
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

