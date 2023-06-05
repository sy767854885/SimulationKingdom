using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePath
{
    //��ȡZ����·����x��y���Ƚϳ�������
    public static List<Vector2Int> FindZPathAuto(Vector2Int startIndex, Vector2Int endIndex)
    {
        Vector2Int offset = endIndex - startIndex;
        if (Mathf.Abs(offset.x) >= Mathf.Abs(offset.y)) return FindZPathX(startIndex, endIndex);
        else return FindZPathY(startIndex, endIndex);
    }

    //��ȡZ����·����x����
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

    //��ȡZ����·����y����
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
