using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorsManager : PersistentSingleton<ActorsManager>
{

    public GameObject residentPrefab;

    //职业类型数据
    public List<JobTypeData> jobTypeDatas = new List<JobTypeData>();

    private int idIndexer = 0;

    public GameObject CreateResidentInWorld(Vector3 position,Vector3 faceDir)
    {
        GameObject resident = CreateResident(position, faceDir);//创建居民
        WorldManager.Instance.AddResident(resident);
        return resident;
    }

    //创建居民
    public GameObject CreateResident(Vector3 position,Vector3 faceDir)
    {
        Vector3 dir = Vector3.ProjectOnPlane(faceDir, Vector3.up);
        GameObject o = Instantiate(residentPrefab, position, Quaternion.LookRotation(dir));
        string id= GetRuntimeId().ToString();
        o.GetComponent<IdComponent>().id = id;
        return o;
    }


    //获取运行时Id，每获取一次自动增加1
    public int GetRuntimeId()
    {
        int id = idIndexer;
        idIndexer += 1;
        return id;
    }

    //重置运行时Id
    public void ResetRuntimeId()
    {
        idIndexer = 0;
    }

    //互殴职业配置数据
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