using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePath
{
    //获取Z字形路径，x和y，比较长的先行
    public static List<Vector2Int> FindZPathAuto(Vector2Int startIndex, Vector2Int endIndex)
    {
        Vector2Int offset = endIndex - startIndex;
        if (Mathf.Abs(offset.x) >= Mathf.Abs(offset.y)) return FindZPathX(startIndex, endIndex);
        else return FindZPathY(startIndex, endIndex);
    }

    //获取Z字形路径，x先行
    public static List<Vector2Int> FindZPathX(Vector2Int startIndex, Vector2Int endIndex)
    {
        Vector2Int currentIndex = startIndex;
        Vector2Int offset = endIndex - startIndex;
        Vector2Int half = offset / 2;

        List<Vector2Int> path = new List<Vector2Int>() { currentIndex };
        while (currentIndex != endIndex)
        {
            bool isHalf = (currentIndex.x == startIndex.x + half.x) && currentIndex.y != startIndex.y + offset.y;
            if (currentIndex.x != endIndex.x && !isHalf) currentIndex.x += (int)Mathf.Sign(offset.x);
            if (currentIndex.y != endIndex.y && isHalf) currentIndex.y += (int)Mathf.Sign(offset.y);
            path.Add(currentIndex);
        }
        return path;
    }

    //获取Z字形路径，y先行
    public static List<Vector2Int> FindZPathY(Vector2Int startIndex, Vector2Int endIndex)
    {
        Vector2Int currentIndex = startIndex;
        Vector2Int offset = endIndex - startIndex;
        Vector2Int half = offset / 2;

        List<Vector2Int> path = new List<Vector2Int>() { currentIndex };
        while (currentIndex != endIndex)
        {
            bool isHalf = (currentIndex.y == startIndex.y + half.y) && currentIndex.x != startIndex.x + offset.x;
            if (currentIndex.x != endIndex.x && isHalf) currentIndex.x += (int)Mathf.Sign(offset.x);
            if (currentIndex.y != endIndex.y && !isHalf) currentIndex.y += (int)Mathf.Sign(offset.y);
            path.Add(currentIndex);
        }
        return path;
    }
}
