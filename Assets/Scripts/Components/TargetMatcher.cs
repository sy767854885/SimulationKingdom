using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TargetMatcher
{
    public GameObject target;
    public int rotationValue;
    [Space]
    [Space]
    public bool left, up, right, down;

    /// <summary>
    /// 000
    /// 0A0
    /// 000
    /// </summary>
    /// <param name="matchList"></param>
    /// <returns></returns>
    public bool IsMatchWithExist(List<bool> matchList)
    {
        for(int i = 0; i < matchList.Count; i++)
        {
            if (i == 0 && (left == matchList[i])) continue;
            else if (i == 1 && (up == matchList[i])) continue;
            else if (i == 2 && (right == matchList[i])) continue;
            else if(i == 3 && (down == matchList[i])) continue;
            return false;
        }

        return true;
    }
}
