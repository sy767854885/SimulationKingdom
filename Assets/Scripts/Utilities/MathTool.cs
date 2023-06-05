using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
    public static class MathTool
    {
        /// <summary>
        /// ת����ȡʵ�ʱ���ֵv = (v-min) / (max - min)
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static float GetNormalizedValue(float value, float min, float max)
        {
            float v = Mathf.Clamp(value, min, max);
            if (min == max) return 1;//��ֹ����0
            v = (v - min) / (max - min);
            return v;
        }

        /// <summary>
        /// ת����ȡ��ȡʵ��ֵ
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static float GetNormalizedRealValue(float percent, float min, float max)
        {
            float v = Mathf.Clamp01(percent);
            v = (max - min) * v + min;
            return v;
        }

        /// <summary>
        /// ��ȡ������ʼ����.P������λ�ã�Q�Ǽ��������ʼλ�á�-------------QOOOP0OOOO----------------------
        /// ���棬���length��ֵ�������䷶Χ����ô���ó���ֵ��ֻ���±߽磬�ϱ߽�ᳬ����Χ
        /// </summary>
        /// <param name="point"></param>
        /// <param name="length"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int GetRangeStartIntWithClamp(int point,int length,int min,int max)
        {
            int index = Mathf.Clamp(point, min, max);
            int offsetIndex = length / 2;
            int start = index - offsetIndex;
            start = Mathf.Clamp(start, min, max - length + 1);
            return start;
        }

        public static int MoveTowardsInt(int value, int target,int deltaValue)
        {
            deltaValue=Mathf.Abs(deltaValue);
            if (value > target) value -= target;
            else if (value < target) value += target;
            if (Mathf.Abs(value - target) <= deltaValue) value = target;
            return value;
        }
    }
}

