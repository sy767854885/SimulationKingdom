using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
    public static class MathTool
    {
        /// <summary>
        /// 转换获取实际比例值v = (v-min) / (max - min)
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static float GetNormalizedValue(float value, float min, float max)
        {
            float v = Mathf.Clamp(value, min, max);
            if (min == max) return 1;//防止除以0
            v = (v - min) / (max - min);
            return v;
        }

        /// <summary>
        /// 转换获取获取实际值
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
        /// 获取区间起始索引.P是中心位置，Q是计算出的起始位置。-------------QOOOP0OOOO----------------------
        /// 警告，如果length的值大于区间范围，那么所得出的值将只管下边界，上边界会超出范围
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

