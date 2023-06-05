using UnityEngine;
using Utilities;

namespace Components.Math
{
    //最大值和最小值数据
    [System.Serializable]
    public struct ClampData
    {
        public float min;
        public float max;

        //获取百分比值
        public float GetPercent(float v)=> MathTool.GetNormalizedValue(v, min, max);

        //获取计算值
        public float GetCalculateValue(float percent)=> MathTool.GetNormalizedRealValue(percent, min, max);

        //获取约束值
        public float GetClampValue(float v)=> Mathf.Clamp(v, min, max);
    }

}
