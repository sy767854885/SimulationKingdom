using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    public int ownCount;//ӵ�и÷��ӵľ�����������
    public int capacity;//�����ɵ��������
    private List<string> ownResidentIds=new List<string>();//��ǰӵ�и÷��Ӿ���Id
    private List<string> stayIds=new List<string>();

    //���뷿��
    public bool GetInto(GameObject resident)
    {
        if(IsFull())return false;
        IdComponent idC = resident.GetComponent<IdComponent>();
        stayIds.Add(idC.id);
        return true;
    }

    //��ӷ��ݾ�ס��
    public bool AddOwners(GameObject resident)
    {
        if(IsFullOwners()) return false;
        IdComponent idC=resident.GetComponent<IdComponent>();
        ownResidentIds.Add(idC.id);
        return true;
    }

    //��ǰӵ�и÷��Ӿ�סȨ�ľ����Ƿ���Ա
    public bool IsFullOwners() => ownResidentIds.Count >= ownCount;

    //��ǰ����÷��ӵ����Ƿ���Ա
    public bool IsFull() => stayIds.Count >= capacity;
}
