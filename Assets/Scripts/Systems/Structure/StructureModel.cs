using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureModel : MonoBehaviour
{
    public string prefabId;
    public GameObject modelObj;
    public Transform showIdAnchor;
    public StructureType structureType;

    public void SetShow(bool active)
    {
        modelObj.SetActive(active);
    }
}
