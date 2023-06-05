using GameSys.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameSys.UI
{
    public class PlayPanel : PanelBasic
    {
        [Space]
        public Button buildButton;
        public Button destroyButton;
        [Space]
        public UnitsPanel unitsPanel;
        [Space]
        public Button openHelperButton;
        [Space]
        public GameObject propertiesPanel;
        public GameObject propertyPrefab;
        private List<GameObject> propertiesObjList=new List<GameObject>();
        [Space]
        public Button releaseResidentButton;
        [Space]
        public StructureInfoPanel structureInfoPanel;
        [Space]
        public Text totalResidentCountText;
        public Text workerCountText;
        private void Start()
        {
            buildButton.onClick.AddListener(() => { BuildStructure(); });
            destroyButton.onClick.AddListener(() => { DestroyStructure(); });

            openHelperButton.onClick.AddListener(() => { OpenMapHelper(); });
            releaseResidentButton.onClick.AddListener(() => { ActorsManager.Instance.CreateResidentInWorld(Vector3.zero, Vector3.forward);ShowResidentCount(); });
        }

        public override void Open()
        {
            base.Open();
            ShowPropertiesPanel();
            ShowResidentCount();
        }

        public override void Close()
        {
            base.Close();
            unitsPanel.Close();
            openHelperButton.gameObject.SetActive(true);
        }

        //==================================================================================

        private void BuildStructure()
        {
            if (!unitsPanel.gameObject.activeSelf)
                unitsPanel.Open();
            else
                unitsPanel.Close();
        }

        private void DestroyStructure()
        {
            GameManager.Instance.CreateDestroyController();
        }

        //==================================================================================


        private void OpenMapHelper()
        {
           MapHelperPanel helperPanel = UIManager.Instance.OpenSinglePanel<MapHelperPanel>();
           openHelperButton.gameObject.SetActive(false);
            helperPanel.closeAction = () => { openHelperButton.gameObject.SetActive(true); };
        }

        //==================================================================================
        public void ShowResidentCount()
        {
            totalResidentCountText.text = WorldManager.Instance.ResidentCount().ToString();
        }
        //==================================================================================
        private void ShowPropertiesPanel()
        {
            ClearPropertiesObj();
            var list = PropertiesManager.Instance.GetProperties();
            foreach(var d in list)
            {
                GameObject o = Instantiate(propertyPrefab, propertiesPanel.transform);
                o.SetActive(true);
                o.transform.Find("Image").GetComponent<Image>().sprite = d.icon;
                o.transform.Find("NumberText").GetComponent<Text>().text = PropertiesManager.Instance.GetValue(d.type).ToString();
                o.transform.Find("NameText").GetComponent<Text>().text = d.name;
                propertiesObjList.Add(o);
            }
        }

        private void ClearPropertiesObj()
        {
            foreach(GameObject o in propertiesObjList)
            {
                Destroy(o);
            }
            propertiesObjList.Clear();
        }
    }
}

