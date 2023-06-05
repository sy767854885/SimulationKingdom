using GameSys.Structure;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameSys.UI
{
    public class EditMapPanel : PanelBasic
    {
        public Button saveButton;//保存
        public Button saveNewButton;//保存为新的存档
        public Button quitButton;//退出

        [Space]
        public Button structuresButton;
        public Button groundsButton;
        public GameObject itemPrefab;
        public Transform parent;
        private List<StructureData> structureDatas = new List<StructureData>();
        private List<GameObject> itemsList=new List<GameObject>();

        [Space]
        public InputField sizeInput;
        public Button setSizeButton;
        public Button destroyButton;

        [Space]
        public Button clearButton;
        public Button randomButton;

        private void Start()
        {
            structuresButton.onClick.AddListener(() => { ShowStructures(); });
            groundsButton.onClick.AddListener(() => { ShowGrounds(); });

            setSizeButton.onClick.AddListener(() => { SetSize(); });

            clearButton.onClick.AddListener(() => { Clear(); });
            randomButton.onClick.AddListener(() => { Random(); });
            destroyButton.onClick.AddListener(() => { DestoryObj(); });

            saveButton.onClick.AddListener(() => { Save(); });
            saveNewButton.onClick.AddListener(() => { SaveAsNew(); });
            quitButton.onClick.AddListener(() => { Quit(); });
        }

        public override void Open()
        {
            base.Open();
            ShowStructures();
        }

        //==================================================================================

        private void ShowStructures()
        {
            structureDatas = StructuresManager.Instance.GetEditableStructureDatas();
            ShowList(structureDatas, (data, obj) => {
                obj.GetComponent<Button>().onClick.AddListener(() => { GameManager.Instance.CreateEditController(data.id, false); });
                });
        }

        private void ShowGrounds()
        {
            
            structureDatas = StructuresManager.Instance.GetGroundStructureDatas();
            ShowList(structureDatas, (data, obj) => {
                obj.GetComponent<Button>().onClick.AddListener(() => { GameManager.Instance.CreateEditController(data.id, true); });
            });
        }

        private void ShowList(List<StructureData> list,Action<StructureData, GameObject> itemAction)
        {
            ClearItems();
            foreach (var data in list)
            {
                GameObject o = Instantiate(itemPrefab, parent);
                o.SetActive(true);
                itemsList.Add(o);
                o.transform.GetChild(0).GetComponent<Image>().sprite = data.sprite;
                itemAction?.Invoke(data, o);
            }
        }

        private void ClearItems()
        {
            foreach(GameObject o in itemsList)
            {
                if(o)Destroy(o);
            }
            itemsList.Clear();
        }
        //==================================================================================

        private void SetSize()
        {
            int size = int.Parse(sizeInput.text);
            EditManager.Instance.SetSize(size);
        }

        private void Clear()
        {
            EditManager.Instance.Clear();
        }

        private void Random()
        {
            EditManager.Instance.SetRandom();
        }

        private void DestoryObj()
        {
            GameManager.Instance.CreateEditDestroyController();
        }

        //==================================================================================

        private void Save()
        {
            GameManager.Instance.SaveEdit(false);
        }

        private void SaveAsNew()
        {
            GameManager.Instance.SaveEdit(true);
        }

        private void Quit()
        {
            GameManager.Instance.QuitMapEdit();
        }
    }
}

