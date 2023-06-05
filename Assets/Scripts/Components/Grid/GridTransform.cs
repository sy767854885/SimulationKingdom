using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Components.Grid
{
    public class GridTransform : MonoBehaviour
    {
        [HideInInspector]
        public Vector2Int gridIndex;//占据的索引（左下角）
        public Vector2Int gridSize;//占据的网格大小

        private int rotation = 0;

        public Vector2Int LocalGridSize { get {return TransRotatedSize(gridSize, rotation); } }//旋转之后的网格尺寸
        public int RotationValue { get { return rotation; } }//旋转值


        //设置方向
        public void SetDir(int dir)
        {
            Vector3 forward=TransDir(dir);
            transform.forward = forward;
            rotation=TransRotationValue(dir);
            //localGridSize = TransRotatedSize(gridSize,dir);
        }


        //旋转
        public void Rotate(bool isRight) => SetDir(isRight ? rotation + 1 : rotation - 1);


        public Vector2Int[,] GetPlacedIndexArr()
        {
            return Grid.GetSquareIndexArr(gridIndex, LocalGridSize);
        }


        //获取旋转之后的格子尺寸，也就是旋转的时候，占据的网格范围也会旋转,比如旋转90度，那么原来的x和y就要交换
        public static Vector2Int TransRotatedSize(Vector2Int originalSize,int dir)
        {
            dir = TransRotationValue(dir);
            if(dir==0||dir==2)return originalSize;
            else return new Vector2Int(originalSize.y,originalSize.x);
        }

        //转换整数为方向值
        public static int TransRotationValue(int value)=> value % 4;


        //将整数转换成方向
        public static Vector3 TransDir(int dir)
        {
            dir = TransRotationValue(dir);
            if (dir == 0) return Vector3.forward;
            else if (dir == 1) return Vector3.right;
            else if (dir == 2) return Vector3.back;
            else return Vector3.left;
        }
    }
}

