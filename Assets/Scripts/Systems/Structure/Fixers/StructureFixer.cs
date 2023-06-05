using Components.Grid;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureFixer
{
    public GameObject defaultObj;
    public virtual GameObject GetPrefab(Vector2Int indexPos,GridPlacement placement, out int rotationValue)
    {
        rotationValue = 0;
        return defaultObj;
    }
}
