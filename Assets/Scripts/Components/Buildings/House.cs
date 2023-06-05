using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    public int ownCount;//拥有该房子的居民的最大数量
    public int capacity;//能容纳的最大数量
    private List<string> ownResidentIds=new List<string>();//当前拥有该房子居民Id
    private List<string> stayIds=new List<string>();

    //进入房间
    public bool GetInto(GameObject resident)
    {
        if(IsFull())return false;
        IdComponent idC = resident.GetComponent<IdComponent>();
        stayIds.Add(idC.id);
        return true;
    }

    //添加房屋居住者
    public bool AddOwners(GameObject resident)
    {
        if(IsFullOwners()) return false;
        IdComponent idC=resident.GetComponent<IdComponent>();
        ownResidentIds.Add(idC.id);
        return true;
    }

    //当前拥有该房子居住权的居民是否满员
    public bool IsFullOwners() => ownResidentIds.Count >= ownCount;

    //当前进入该房子的人是否满员
    public bool IsFull() => stayIds.Count >= capacity;
}
