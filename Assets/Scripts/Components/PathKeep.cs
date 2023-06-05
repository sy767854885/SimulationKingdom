using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathKeep
{
    public List<Vector2Int> positions = new List<Vector2Int>();//»º´æÂ·¾¶

    private Vector2Int startPosition;
    private Vector2Int endPosition;
    private bool isUpdating = false;

    public bool Update(Vector2Int position)
    {
        if (Check(position))
        {
            positions.Clear();
            positions = SimplePath.FindZPathAuto(startPosition, position);
            return true;
        }
        return false;
    }

    private bool Check(Vector2Int position)
    {
        if (isUpdating == false)
        {
            isUpdating = true;
            startPosition = position;
            endPosition = position;

            positions.Add(position);
            return true;
        }
        else
        {
            if (endPosition == position) return false;
            return true;
        }
    }

    public void Clear()
    {
        positions.Clear();
        isUpdating = false;
    }
}
