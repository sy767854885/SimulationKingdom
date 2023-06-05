using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmployManager : PersistentSingleton<EmployManager>
{
    public bool EmployForStructor(string buildingId)
    {
        GameObject building = WorldManager.Instance.gridPlacementForStructure.GetAllStructures().Find(x => x.GetComponent<IdComponent>().id == buildingId).gameObject;
        if (building == null) return false;

        GameObject resident = FindIdleActor();
        if (resident==null)return false;

        Employ employ = building.GetComponent<Employ>();
        ActorBase actor=resident.GetComponent<ActorBase>();

        if (!employ.AddEmployee(resident.GetComponent<IdComponent>().id))return false;
        actor.SetJob(employ.jobType);
        actor.SetWorkForId(building.GetComponent<IdComponent>().id);

        if (employ.propertyTypes.Count > 0)
            actor.SetWorkPropertyType(employ.propertyTypes[0]);

        return true;
    }


    //从建筑中解雇actor
    public bool UnEmployFromStructure(string buildingId,string actorId)
    {
        GameObject building = WorldManager.Instance.gridPlacementForStructure.GetAllStructures().Find(x => x.GetComponent<IdComponent>().id == buildingId).gameObject;
        if (building == null) return false;

        GameObject resident = WorldManager.Instance.GetResidentWithId(actorId);
        if (resident==null) return false;

        if (building.GetComponent<Employ>().RemoveEmploee(actorId))
        {
            resident.GetComponent<ActorBase>().UnEmployed();
            return true;
        }
        return false;
    }

    //切换工作类型
    public bool SwitchWork(string buildingId, string actorId)
    {
        GameObject building = WorldManager.Instance.gridPlacementForStructure.GetAllStructures().Find(x => x.GetComponent<IdComponent>().id == buildingId).gameObject;
        if (building == null) return false;

        GameObject resident = WorldManager.Instance.GetResidentWithId(actorId);
        if (resident == null) return false;

        Employ employ=building.GetComponent<Employ>();
        if (employ.jobType == JobType.None || employ.jobType == JobType.Porter || employ.jobType == JobType.Builder) return false;

        if (employ && employ.propertyTypes.Count > 1)
        {
            List<PropertyType> list = employ.propertyTypes;
            int index = list.IndexOf(resident.GetComponent<ActorBase>().WorkForProperty);
            index += 1;
            if (index >= list.Count) index = 0;
            resident.GetComponent<ActorBase>().SetWorkPropertyType(list[index]);
            return true;
        }

        return false;
    }


    //找到一个空闲的角色
    public GameObject FindIdleActor()
    {
        List<GameObject> residents = WorldManager.Instance.GetResidents();
        foreach (GameObject o in residents)
        {
            if ( string.IsNullOrEmpty(o.GetComponent<ActorBase>().WorkForId))
            {
                return o;
            }
        }
        return null;
    }

    //获取一个角色的职业信息
    public (string buildingId,JobType jobType,PropertyType propertyType) GetResidentWorkInfo(string id)
    {
        GameObject resident = WorldManager.Instance.GetResidentWithId(id);
        ActorBase actorBase=resident.GetComponent<ActorBase>();
        return (actorBase.WorkForId, actorBase.JobType, actorBase.WorkForProperty);
    }
}
