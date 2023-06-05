using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
    public static class VectorTool
    {
        public static Vector2 AngleRadiusToPosition(float angle,float radius)
        {
            float x = radius * Mathf.Cos(angle * 3.14f / 180f);
            float y = radius * Mathf.Sin(angle * 3.14f / 180f);
            return new Vector2(x, y);
        }
    }
}

