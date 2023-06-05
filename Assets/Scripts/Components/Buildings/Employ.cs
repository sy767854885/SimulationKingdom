using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Employ : MonoBehaviour
{
    public JobType jobType;
    public int maxEmployCount;
    public List<string> emploees=new List<string>();
    public List<PropertyType> propertyTypes = new List<PropertyType>();//可生产的资源类型

    public bool AddEmployee(string id)
    {
        if (emploees.Count >= maxEmployCount) return false;
        if (emploees.Contains(id)) return false;
        emploees.Add(id);
        return true;
    }

    public bool RemoveEmploee(string id)
    {
        if(emploees.Contains(id))
        {
            emploees.Remove(id);
            return true;
        }
        return false;
    }
}
