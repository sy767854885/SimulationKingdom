using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameSys.UI 
{
    public class UIManager : PersistentSingleton<UIManager>
    {
        [SerializeField] private GameObject canvasObj;

        List<PanelBasic> panels= new List<PanelBasic>();

        public GameObject CanvasObj { get { return canvasObj; } }

        protected override void Awake()
        {
            base.Awake();
            InitPanels();
        }

        private void InitPanels()
        {
            if (!canvasObj) canvasObj = transform.GetChild(0).gameObject;
            canvasObj.GetComponentsInChildren<PanelBasic>(true, panels);
        }

        /// <summary>
        /// 获取组件对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetPanel<T>() where T : PanelBasic
        {
            foreach(PanelBasic p in panels)
            {
                T o = p as T;
                if (o != null) return o;
            }
            return null;
        }

        public T OpenSinglePanel<T>()where T : PanelBasic
        {
            T p = GetPanel<T>();
            p.Open();
            return p;
        }

        public void CloseSinglePanel<T>() where T : PanelBasic
        {
            T p = GetPanel<T>();
            p.Close();
        }

        public void CloseAllOpeninigPanels()
        {
            foreach(PanelBasic p in panels)
            {
                if (p.IsOpened) p.Close();
            }
        }
    }
}


