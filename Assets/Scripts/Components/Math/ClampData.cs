using UnityEngine;
using Utilities;

namespace Components.Math
{
    //���ֵ����Сֵ����
    [System.Serializable]
    public struct ClampData
    {
        public float min;
        public float max;

        //��ȡ�ٷֱ�ֵ
        public float GetPercent(float v)=> MathTool.GetNormalizedValue(v, min, max);

        //��ȡ����ֵ
        public float GetCalculateValue(float percent)=> MathTool.GetNormalizedRealValue(percent, min, max);

        //��ȡԼ��ֵ
        public float GetClampValue(float v)=> Mathf.Clamp(v, min, max);
    }

}
