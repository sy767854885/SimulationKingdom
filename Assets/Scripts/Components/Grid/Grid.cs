using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Components.Grid
{
    public class Grid
    {
        private int width;//���
        private int height;//�߶�
        private float unitSize;//��Ԫ���С
        private Vector3 originalPosition;//���½�λ��

        public Vector2Int GridCount { get { return new Vector2Int(width, height); } }
        public Vector3 OriginalPosition { get { return originalPosition; } }

        public Grid(int width,int height,float unitSize,Vector3 originalPosition)
        {
            this.width = width;
            this.height = height;
            this.unitSize = unitSize;
            this.originalPosition = originalPosition;
        }
        
        //��ȡ����ĸ������ߴ�
        public Vector2 GetSize() { return new Vector2(width * unitSize, height * unitSize); }

        //��ȡ���������λ��
        public Vector3 GetCenterPosition() { return originalPosition + new Vector3(width * unitSize / 2, 0, height * unitSize / 2); }

        //��ȡ�����������½�λ��
        public Vector3 GetUnitOriginalPosition(int x, int y)
        {
            return new Vector3(x, 0, y) * unitSize + originalPosition;
        }


        //��ȡ������������λ��
        public Vector3 GetUnitCenterPosition(int x, int y)
        {
            return GetUnitOriginalPosition(x, y) + new Vector3(unitSize / 2, 0, unitSize / 2);
        }


        //��ȡλ�����ڵĸ����±�
        public void GetXY(Vector3 worldPosition, out int x, out int y)
        {
            x = Mathf.FloorToInt((worldPosition - originalPosition).x / unitSize);
            y = Mathf.FloorToInt((worldPosition - originalPosition).z / unitSize);
        }

        //��ȡλ�����ڵĸ����±�
        public Vector2Int GetPositionIndex(Vector3 worldPosition)
        {
            GetXY(worldPosition, out int x, out int y);
            return new Vector2Int(x, y);
        }

        //������λ��ת��Ϊ����λ�ã�����y��ֵ�������yֵ
        public Vector3 GetPosition(Vector3 worldPosition)
        {
            GetXY(worldPosition, out int x, out int y);
            return new Vector3(x, originalPosition.y, y);
        }

        //==================================================================================
        //BOOL�ж�

        //�����Ƿ�������Χ��
        public bool IndexInBound(int x,int y)
        {
            return x >= 0 && y >= 0 && x < width && y < height;
        }

        //һ�����������Ƿ�������Χ��
        public bool RangeInBounds(Vector2Int indexPos,Vector2Int count)
        {
            return count.x>0&&count.y>0
                &&IndexInBound(indexPos.x, indexPos.y)
                &&IndexInBound(indexPos.x+count.x-1,indexPos.y+count.y-1);
        }

        //==================================================================================


        //��ȡһ�������µ������ڽ�����(����߿�ʼ˳ʱ��)
        public List<Vector2Int> GetAllAdjacentIndexs(int x,int y)
        {
            List<Vector2Int> adjacentUnits = new List<Vector2Int>();
            if(IndexInBound(x,y)==false)return adjacentUnits;
            if (x > 0)
            {
                adjacentUnits.Add(new Vector2Int(x - 1, y));
            }
            if (y < height - 1)
            {
                adjacentUnits.Add(new Vector2Int(x, y + 1));
            }
            if (x < width - 1)
            {
                adjacentUnits.Add(new Vector2Int(x + 1, y));
            }
            if (y > 0)
            {
                adjacentUnits.Add(new Vector2Int(x, y - 1));
            }
            
            return adjacentUnits;
        }

        //����һ�������б������������б���Χ�ĸ���,���еĽ�����޶�������Χ��
        public List<Vector2Int> GetListAllAdjacentWithClamp(List<Vector2Int> targetList)
        {
            List<Vector2Int> list = new List<Vector2Int>();
            for(int i = 0; i < targetList.Count; i++)
            {
                List<Vector2Int> adjacents = GetAllAdjacentIndexsNoBound(targetList[i].x,targetList[i].y);
                for(int adIndex=0;adIndex<adjacents.Count;adIndex++)
                {
                    //���ڵĵ��������:����Ŀ���б��У�������Χ��
                    if (targetList.Contains(adjacents[adIndex]) || !IndexInBound(adjacents[adIndex].x, adjacents[adIndex].y) || list.Contains(adjacents[adIndex]))
                        continue;
                    list.Add(adjacents[adIndex]);
                }
            }
            return list;
        }

        //==================================================================================

        //��ȡһ���������λ��
        public Vector3 GetSquareCenter(Vector2Int leftDownIndex, Vector2Int count)
        {
            Vector3 leftDownPos = GetUnitCenterPosition(leftDownIndex.x, leftDownIndex.y);
            Vector3 rightUpPos = GetUnitCenterPosition(leftDownIndex.x + count.x - 1, leftDownIndex.y + count.y - 1);
            return (leftDownPos + rightUpPos) / 2;
        }

        //��������λ�ã�������������ȡ���������ʼ���������½ǣ������棬���������ߴ���ڴ�������ô����ֻ�޶��±߽磬�ϱ߽�ᳬ����Χ
        public Vector2Int GetRangeStartIndexWithClamp(Vector2Int centerIndexPos, int countX, int countY)
        {
            int startIndexX = Utilities.MathTool.GetRangeStartIntWithClamp(centerIndexPos.x, countX, 0, GridCount.x - 1);
            int startIndexY = Utilities.MathTool.GetRangeStartIntWithClamp(centerIndexPos.y, countY, 0, GridCount.y - 1);
            return new Vector2Int(startIndexX, startIndexY);
        }

        //��������λ�ã�������������ȡ������
        public Vector2Int GetRangeStartIndexWithClamp(Vector3 position, int countX, int countY)
        {
            Vector2Int pointIndex = GetPositionIndex(position);
            return GetRangeStartIndexWithClamp(pointIndex, countX, countY);
        }

        //==================================================================================

        //��ȡһ�����η�Χ�ڵ����нڵ�
        public static Vector2Int[,] GetSquareIndexArr(Vector2Int leftDownIndex, Vector2Int count)
        {
            Vector2Int[,] arr = new Vector2Int[count.x, count.y];
            for (int i = 0; i < count.x; i++)
            {
                for (int j = 0; j < count.y; j++)
                {
                    Vector2Int indexPos = leftDownIndex + new Vector2Int(i, j);
                    arr[i, j] = indexPos;
                }
            }
            return arr;
        }

        //��ȡһ�����η�Χ�ڵ����нڵ�,����Ϊ���·��ĵ�����Ϸ��ĵ�
        public static Vector2Int[,] GetSquareIndexArrByCorner(Vector2Int startIndex, Vector2Int endIndex)
        {
            Vector2Int leftDown = new Vector2Int(startIndex.x <= endIndex.x ? startIndex.x : endIndex.x, startIndex.y <= endIndex.y ? startIndex.y : endIndex.y);
            Vector2Int rightUp = new Vector2Int(startIndex.x > endIndex.x ? startIndex.x : endIndex.x, startIndex.y > endIndex.y ? startIndex.y : endIndex.y);
            Vector2Int count = rightUp - leftDown + Vector2Int.one;
            return GetSquareIndexArr(leftDown, count);
        }

        //==================================================================================

        //��ȡһ�������µ������ڽ�����(����߿�ʼ˳ʱ��),�����ȡ����һȦ��������Χֵ֮��ĵ㣬����������0����ô����ߵĵ����-1�������count-1����ô���ұ߾���count��
        //��Ȼ��x,yҲ�����ڷ�Χ֮��
        public static List<Vector2Int> GetAllAdjacentIndexsNoBound(int x, int y)
        {
            List<Vector2Int> adjacentUnits = new List<Vector2Int>();
            adjacentUnits.Add(new Vector2Int(x - 1, y));
            adjacentUnits.Add(new Vector2Int(x, y + 1));
            adjacentUnits.Add(new Vector2Int(x + 1, y));
            adjacentUnits.Add(new Vector2Int(x, y - 1));
            return adjacentUnits;
        }

        //==================================================================================

        //����������󣬸�������ԭ�㣨���½�λ�ã�
        public static Grid BuildGridLeftDown(int width, int height, float unitSize, Vector3 originalPosition)
        {
            return new Grid(width,height,unitSize,originalPosition);
        }

        //����������󣬸�����������
        public static Grid BuildGridCenter(int width, int height, float unitSize, Vector3 centerPosition)
        {
            Vector3 originalPosition = centerPosition - new Vector3(width * unitSize / 2, 0, height * unitSize / 2);
            return new Grid(width, height, unitSize, originalPosition);
        }
    }
}


