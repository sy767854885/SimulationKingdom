using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorsManager : PersistentSingleton<ActorsManager>
{

    public GameObject residentPrefab;

    //ְҵ��������
    public List<JobTypeData> jobTypeDatas = new List<JobTypeData>();

    private int idIndexer = 0;

    public GameObject CreateResidentInWorld(Vector3 position,Vector3 faceDir)
    {
        GameObject resident = CreateResident(position, faceDir);//��������
        WorldManager.Instance.AddResident(resident);
        return resident;
    }

    //��������
    public GameObject CreateResident(Vector3 position,Vector3 faceDir)
    {
        Vector3 dir = Vector3.ProjectOnPlane(faceDir, Vector3.up);
        GameObject o = Instantiate(residentPrefab, position, Quaternion.LookRotation(dir));
        string id= GetRuntimeId().ToString();
        o.GetComponent<IdComponent>().id = id;
        return o;
    }


    //��ȡ����ʱId��ÿ��ȡһ���Զ�����1
    public int GetRuntimeId()
    {
        int id = idIndexer;
        idIndexer += 1;
        return id;
    }

    //��������ʱId
    public void ResetRuntimeId()
    {
        idIndexer = 0;
    }

    //��Źְҵ��������
    public JobTypeData GetJobTypeInfo(JobType jobType)
    {
        return jobTypeDatas.Find(x => x.jobType == jobType);
    }
}

[System.Serializable]
public class JobTypeData
{
    public JobType jobType;
    public string name;
    public Sprite icon;
}