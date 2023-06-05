using Components.Grid;
using GameSys.Build;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EditDestroyController : ActionController
{
    public Texture2D texture;

    private void Start()
    {
        Cursor.SetCursor(texture, new Vector2(25, 75), CursorMode.Auto);
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && EventSystem.current.IsPointerOverGameObject() == false)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3? position = ObjectDetector.Instance.RaycastGround(ray);
            if (position != null) DestroyObject(position.Value);
        }
    }

    private void DestroyObject(Vector3 positioin)
    {
        Vector2Int indexPos = WorldManager.Instance.gridSystem.grid.GetPositionIndex(positioin);
        EditManager.Instance.DestroyObjWithPos(indexPos);
    }

    public override void Clear()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}
