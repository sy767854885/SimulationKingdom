using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Components.Grid
{
    public class GridTransform : MonoBehaviour
    {
        [HideInInspector]
        public Vector2Int gridIndex;//ռ�ݵ����������½ǣ�
        public Vector2Int gridSize;//ռ�ݵ������С

        private int rotation = 0;

        public Vector2Int LocalGridSize { get {return TransRotatedSize(gridSize, rotation); } }//��ת֮�������ߴ�
        public int RotationValue { get { return rotation; } }//��תֵ


        //���÷���
        public void SetDir(int dir)
        {
            Vector3 forward=TransDir(dir);
            transform.forward = forward;
            rotation=TransRotationValue(dir);
            //localGridSize = TransRotatedSize(gridSize,dir);
        }


        //��ת
        public void Rotate(bool isRight) => SetDir(isRight ? rotation + 1 : rotation - 1);


        public Vector2Int[,] GetPlacedIndexArr()
        {
            return Grid.GetSquareIndexArr(gridIndex, LocalGridSize);
        }


        //��ȡ��ת֮��ĸ��ӳߴ磬Ҳ������ת��ʱ��ռ�ݵ�����ΧҲ����ת,������ת90�ȣ���ôԭ����x��y��Ҫ����
        public static Vector2Int TransRotatedSize(Vector2Int originalSize,int dir)
        {
            dir = TransRotationValue(dir);
            if(dir==0||dir==2)return originalSize;
            else return new Vector2Int(originalSize.y,originalSize.x);
        }

        //ת������Ϊ����ֵ
        public static int TransRotationValue(int value)=> value % 4;


        //������ת���ɷ���
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

