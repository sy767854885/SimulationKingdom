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
        
        //��������ĳߴ�
        public void SetSize(int width,int height,float unitSize)
        {
            if (unitSize <= 0) unitSize = 1;
            if(width<=0)width = 1;
            if(height<=0)height = 1;
            this.width = width;
            this.height = height;
            grid=Grid.BuildGridCenter(width, height, unitSize, transform.position);
        }

        //��������Ƿ�����������Χ��
        public bool IndexPosInBound(Vector2Int indexPos)
        {
            if (indexPos.x >= 0 && indexPos.x < width && indexPos.y>=0&&indexPos.y<height)
            {
                return true;
            }
            return false;
        }

        //�����ȡ��ȡ������ռ�ݵ����������
        internal Vector2Int[,] GetGridTransformHoldArr(GridTransform gridTransform)
        {
            return Grid.GetSquareIndexArr(gridTransform.gridIndex, gridTransform.LocalGridSize);
        }

        //������λ��ת��Ϊ����λ��
        public Vector3 TransformPosition(Vector3 position)
        {
            Vector3 pos=grid.GetPosition(position);
            pos.y=position.y;
            return pos;
        }

        //��ȡ�����ʵ������
        public Vector3 GetGridTransformPosition(GridTransform gridTransform)
        {
            return grid.GetSquareCenter(gridTransform.gridIndex, gridTransform.LocalGridSize);
        }

        //������ƽ���Ϸ�������
        public void PlaceGridTransform(GridTransform gridTransform)
        {
            gridTransform.transform.position = GetGridTransformPosition(gridTransform);
        }

        //��������λ��������ƽ���Ϸ�������
        //��ע�⣬����������λ�ã��������½�λ�ã���GridTransfom�ĸ��ӳߴ�������ʱ��ȡ�м�λ�ã�ż��ȡ��ߣ�����ߴ���4�������б�Ϊ0123���м�ֵΪ1����
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

