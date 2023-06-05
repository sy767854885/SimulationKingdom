using Components.Grid;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GroundEditController : EditController
{
    private Vector2Int prePoint;

    public GameObject modelPrefab;
    private GameObject tagModel;

    private void Update()
    {
        InitTagModel();

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        {
            Vector3? position = ObjectDetector.Instance.RaycastGround(ray);
            if (position != null)
            {
                GridTransform gridTransform = tagModel.GetComponent<GridTransform>();
                WorldManager.Instance.gridSystem.grid.GetXY(position.Value, out int x, out int y);
                WorldManager.Instance.gridSystem.PlaceGridTansformWithCenter(gridTransform, new Vector2Int(x, y));
                gridTransform.transform.position = gridTransform.transform.position + Vector3.up * 0.3f;
            }
        }

        if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject() == false)
        {
            var position = ObjectDetector.Instance.RaycastGround(ray);
            if (position != null)
            {
                Vector2Int point = WorldManager.Instance.gridSystem.grid.GetPositionIndex(position.Value);
                EditManager.Instance.PlaceGroundWithCheck(id, point);
            }
        }

        bool isDraw = false;
        if (Input.GetMouseButton(0) && EventSystem.current.IsPointerOverGameObject() == false)
        {
            var position = ObjectDetector.Instance.RaycastGround(ray);
            if (position != null)
            {
                isDraw = true;
                Vector2Int point = WorldManager.Instance.gridSystem.grid.GetPositionIndex(position.Value);
                if (prePoint != point) EditManager.Instance.PlaceGroundWithCheck(id, point);
            }
        }

        tagModel.SetActive(!isDraw);
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
