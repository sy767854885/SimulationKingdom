using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDetector : PersistentSingleton<ObjectDetector>
{
    public LayerMask groundMask;

    //ºÏ≤‚µÿ√Ê
    public Vector3? RaycastGround()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundMask))
        {
            return hit.point;
        }

        return null;
    }

    public Vector3? RaycastGround(Ray ray)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundMask))
        {
            return hit.point;
        }
        return null;
    }

    public GameObject RaycastAll(Ray ray)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            return hit.transform.gameObject;
        }
        return null;
    }
}
