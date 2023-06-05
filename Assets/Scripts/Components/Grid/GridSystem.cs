using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Components.Grid
{
    public class GridSystem : MonoBehaviour
    {
        public int width;
        public int height;
        public Grid grid;

        private void Start()
        {
            grid = Grid.BuildGridCenter(width, height, 1, transform.position);
        }
        
        //设置网格的尺寸
        public void SetSize(int width,int height,float unitSize)
        {
            if (unitSize <= 0) unitSize = 1;
            if(width<=0)width = 1;
            if(height<=0)height = 1;
            this.width = width;
            this.height = height;
            grid=Grid.BuildGridCenter(width, height, unitSize, transform.position);
        }

        //检查索引是否在网格区域范围内
        public bool IndexPosInBound(Vector2Int indexPos)
        {
            if (indexPos.x >= 0 && indexPos.x < width && indexPos.y>=0&&indexPos.y<height)
            {
                return true;
            }
            return false;
        }

        //计算获取获取物体所占据的网格的索引
        internal Vector2Int[,] GetGridTransformHoldArr(GridTransform gridTransform)
        {
            return Grid.GetSquareIndexArr(gridTransform.gridIndex, gridTransform.LocalGridSize);
        }

        //将世界位置转换为网格位置
        public Vector3 TransformPosition(Vector3 position)
        {
            Vector3 pos=grid.GetPosition(position);
            pos.y=position.y;
            return pos;
        }

        //获取网格的实际坐标
        public Vector3 GetGridTransformPosition(GridTransform gridTransform)
        {
            return grid.GetSquareCenter(gridTransform.gridIndex, gridTransform.LocalGridSize);
        }

        //在网格平面上放置物体
        public void PlaceGridTransform(GridTransform gridTransform)
        {
            gridTransform.transform.position = GetGridTransformPosition(gridTransform);
        }

        //给出中心位置在网格平面上放置物体
        //（注意，这里是中心位置，不是左下角位置，当GridTransfom的格子尺寸是奇数时，取中间位置，偶数取左边，比如尺寸是4，索引列表为0123，中间值为1），
        public void PlaceGridTansformWithCenter(GridTransform gridTransform,Vector2Int centerIndexPos)
        {
            Vector2Int startIndex = grid.GetRangeStartIndexWithClamp(centerIndexPos, gridTransform.LocalGridSize.x, gridTransform.LocalGridSize.y);
            gridTransform.gridIndex = startIndex;
            PlaceGridTransform(gridTransform);
        }

        private void OnDrawGizmosSelected()
        {
            if (grid == null) return;
            Vector2Int gridCount = grid.GridCount;
            for(int i = 0; i < gridCount.x; i++)
            {
                for(int j = 0; j < gridCount.y; j++)
                {
                    Gizmos.color = Color.white;
                    Gizmos.DrawWireCube(grid.GetUnitCenterPosition(i, j), new Vector3(1, 0, 1));
                }
            }
        }
    }
}

