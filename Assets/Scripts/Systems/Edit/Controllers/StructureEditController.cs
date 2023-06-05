using Components.Grid;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StructureEditController : EditController
{
    public GameObject modelPrefab;
    private GameObject tagModel;

    private void Update()
    {
        InitTagModel();

        if (Input.GetKeyDown(KeyCode.R))
        {
            tagModel.GetComponent<GridTransform>().Rotate(true);
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        {
            Vector3? position = ObjectDetector.Instance.RaycastGround(ray);
            if (position != null)
            {
                GridTransform gridTransform = tagModel.GetComponent<GridTransform>();
                WorldManager.Instance.gridSystem.grid.GetXY(position.Value, out int x, out int y);
                WorldManager.Instance.gridSystem.PlaceGridTansformWithCenter(gridTransform, new Vector2Int(x, y));
            }
        }

        if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject() == false)
        {
            Vector3? position = ObjectDetector.Instance.RaycastGround(ray);
            if (position != null) EditManager.Instance.PlaceStructure(id, tagModel.GetComponent<GridTransform>().gridIndex, tagModel.GetComponent<GridTransform>().RotationValue);
        }
    }

    public void InitTagModel()
    {
        if (tagModel) return;
        tagModel = Instantiate(modelPrefab);
    }

    public void RemoveTagModel()
    {
        if (tagModel) Destroy(tagModel);
    }

    public override void Clear()
    {
        RemoveTagModel();
    }
}
