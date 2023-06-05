using Components.Grid;
using GameSys.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameSys
{
    public class SelectController : ActionController
    {
        private GridTransform currentGridTransform;

        private void Update()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject() == false)
            {
                Vector3? position = ObjectDetector.Instance.RaycastGround(ray);
                if (position != null)
                {
                    Vector2Int indexPos = WorldManager.Instance.gridSystem.grid.GetPositionIndex(position.Value);
                    GridTransform gridTransform = WorldManager.Instance.GetGridTransformInPos(indexPos, out bool isStructure);
                    SelectGridTransform(gridTransform,isStructure);
                }
            }
        }

        private void SelectGridTransform(GridTransform gridTransform,bool isStructure)
        {
            if (gridTransform && isStructure)
            {
                currentGridTransform = gridTransform;
                UIManager.Instance.GetPanel<PlayPanel>().structureInfoPanel.Open(currentGridTransform.transform);
            }
            else
            {
                UIManager.Instance.GetPanel<PlayPanel>().structureInfoPanel.Close();
            }
        }

        public override void Clear()
        {
            UIManager.Instance.GetPanel<PlayPanel>().structureInfoPanel.Close();
        }
    }
}

