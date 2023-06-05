using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Components.Grid
{
    public class Grid
    {
        private int width;//宽度
        private int height;//高度
        private float unitSize;//单元格大小
        private Vector3 originalPosition;//左下角位置

        public Vector2Int GridCount { get { return new Vector2Int(width, height); } }
        public Vector3 OriginalPosition { get { return originalPosition; } }

        public Grid(int width,int height,float unitSize,Vector3 originalPosition)
        {
            this.width = width;
            this.height = height;
            this.unitSize = unitSize;
            this.originalPosition = originalPosition;
        }
        
        //获取网格的浮点数尺寸
        public Vector2 GetSize() { return new Vector2(width * unitSize, height * unitSize); }

        //获取网格的中心位置
        public Vector3 GetCenterPosition() { return originalPosition + new Vector3(width * unitSize / 2, 0, height * unitSize / 2); }

        //获取单个格子左下角位置
        public Vector3 GetUnitOriginalPosition(int x, int y)
        {
            return new Vector3(x, 0, y) * unitSize + originalPosition;
        }


        //获取单个格子中心位置
        public Vector3 GetUnitCenterPosition(int x, int y)
        {
            return GetUnitOriginalPosition(x, y) + new Vector3(unitSize / 2, 0, unitSize / 2);
        }


        //获取位置所在的格子下标
        public void GetXY(Vector3 worldPosition, out int x, out int y)
        {
            x = Mathf.FloorToInt((worldPosition - originalPosition).x / unitSize);
            y = Mathf.FloorToInt((worldPosition - originalPosition).z / unitSize);
        }

        //获取位置所在的格子下标
        public Vector2Int GetPositionIndex(Vector3 worldPosition)
        {
            GetXY(worldPosition, out int x, out int y);
            return new Vector2Int(x, y);
        }

        //将世界位置转换为网格位置，并且y的值是网格的y值
        public Vector3 GetPosition(Vector3 worldPosition)
        {
            GetXY(worldPosition, out int x, out int y);
            return new Vector3(x, originalPosition.y, y);
        }

        //==================================================================================
        //BOOL判断

        //索引是否在区域范围内
        public bool IndexInBound(int x,int y)
        {
            return x >= 0 && y >= 0 && x < width && y < height;
        }

        //一个矩形区域是否在网格范围内
        public bool RangeInBounds(Vector2Int indexPos,Vector2Int count)
        {
            return count.x>0&&count.y>0
                &&IndexInBound(indexPos.x, indexPos.y)
                &&IndexInBound(indexPos.x+count.x-1,indexPos.y+count.y-1);
        }

        //==================================================================================


        //获取一个格子下的所有邻近格子(从左边开始顺时针)
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

        //给出一个格子列表，获得这个格子列表周围的格子,所有的结果都限定在网格范围内
        public List<Vector2Int> GetListAllAdjacentWithClamp(List<Vector2Int> targetList)
        {
            List<Vector2Int> list = new List<Vector2Int>();
            for(int i = 0; i < targetList.Count; i++)
            {
                List<Vector2Int> adjacents = GetAllAdjacentIndexsNoBound(targetList[i].x,targetList[i].y);
                for(int adIndex=0;adIndex<adjacents.Count;adIndex++)
                {
                    //相邻的点必须满足:不在目标列表中，在网格范围内
                    if (targetList.Contains(adjacents[adIndex]) || !IndexInBound(adjacents[adIndex].x, adjacents[adIndex].y) || list.Contains(adjacents[adIndex]))
                        continue;
                    list.Add(adjacents[adIndex]);
                }
            }
            return list;
        }

        //==================================================================================

        //获取一个块的中心位置
        public Vector3 GetSquareCenter(Vector2Int leftDownIndex, Vector2Int count)
        {
            Vector3 leftDownPos = GetUnitCenterPosition(leftDownIndex.x, leftDownIndex.y);
            Vector3 rightUpPos = GetUnitCenterPosition(leftDownIndex.x + count.x - 1, leftDownIndex.y + count.y - 1);
            return (leftDownPos + rightUpPos) / 2;
        }

        //根据中心位置，格子数量，获取子网格的起始索引（左下角），警告，如果子网格尺寸大于此网格，那么将会只限定下边界，上边界会超出范围
        public Vector2Int GetRangeStartIndexWithClamp(Vector2Int centerIndexPos, int countX, int countY)
        {
            int startIndexX = Utilities.MathTool.GetRangeStartIntWithClamp(centerIndexPos.x, countX, 0, GridCount.x - 1);
            int startIndexY = Utilities.MathTool.GetRangeStartIntWithClamp(centerIndexPos.y, countY, 0, GridCount.y - 1);
            return new Vector2Int(startIndexX, startIndexY);
        }

        //根据中心位置，格子数量，获取子网格
        public Vector2Int GetRangeStartIndexWithClamp(Vector3 position, int countX, int countY)
        {
            Vector2Int pointIndex = GetPositionIndex(position);
            return GetRangeStartIndexWithClamp(pointIndex, countX, countY);
        }

        //==================================================================================

        //获取一个矩形范围内的所有节点
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

        //获取一个矩形范围内的所有节点,参数为左下方的点和右上方的点
        public static Vector2Int[,] GetSquareIndexArrByCorner(Vector2Int startIndex, Vector2Int endIndex)
        {
            Vector2Int leftDown = new Vector2Int(startIndex.x <= endIndex.x ? startIndex.x : endIndex.x, startIndex.y <= endIndex.y ? startIndex.y : endIndex.y);
            Vector2Int rightUp = new Vector2Int(startIndex.x > endIndex.x ? startIndex.x : endIndex.x, startIndex.y > endIndex.y ? startIndex.y : endIndex.y);
            Vector2Int count = rightUp - leftDown + Vector2Int.one;
            return GetSquareIndexArr(leftDown, count);
        }

        //==================================================================================

        //获取一个格子下的所有邻近格子(从左边开始顺时针),这里获取完整一圈。包括范围值之外的点，比如坐标是0，那么其左边的点就是-1，如果是count-1，那么其右边就是count。
        //当然，x,y也可以在范围之外
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

        //生成网格对象，根据网格原点（左下角位置）
        public static Grid BuildGridLeftDown(int width, int height, float unitSize, Vector3 originalPosition)
        {
            return new Grid(width,height,unitSize,originalPosition);
        }

        //生成网格对象，根据网格中心
        public static Grid BuildGridCenter(int width, int height, float unitSize, Vector3 centerPosition)
        {
            Vector3 originalPosition = centerPosition - new Vector3(width * unitSize / 2, 0, height * unitSize / 2);
            return new Grid(width, height, unitSize, originalPosition);
        }
    }
}


