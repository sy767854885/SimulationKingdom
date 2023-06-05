using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameSys.UI
{
    public class ReadyPlayPanel : PanelBasic
    {
        public GameObject savePrefab;
        public Transform savesParent;
        private List<DataItem> dataItems = new List<DataItem>();
        private List<GameObject> saveItemObjs = new List<GameObject>();
        private int index;
        [Space]
        public Button backButton;

        public Button playButton;
        public Button createMapButton;
        public Button editMapButton;
        public Button deleteMapButton;


        private void Start()
        {
            backButton.onClick.AddListener(() => { Back(); });
            playButton.onClick.AddListener(() => { Play(); });
            createMapButton.onClick.AddListener(() => { CreateMap(); });
            editMapButton.onClick.AddListener(() => { EditMap(); });
            deleteMapButton.onClick.AddListener(() => { DeleteSave(index); });
        }

        public override void Open()
        {
            base.Open();
            UpdateItems();
        }

        private void Back()
        {
            UIManager.Instance.CloseAllOpeninigPanels();
            UIManager.Instance.OpenSinglePanel<MainPanel>();
        }

        private void Play()
        {
            if (dataItems.Count == 0) return;
            GameManager.Instance.PlayGameWithMap(dataItems[index].name);
        }

        private void CreateMap()
        {
            GameManager.Instance.PlayMapEdit();
        }

        private void EditMap()
        {
            if (dataItems.Count == 0) return;
            GameManager.Instance.LoadEdit(dataItems[index].name);
        }

        //==================================================================================

        private void UpdateItems()
        {
            ClearSaves();
            LoadSaves();
            SelectSave(0);
        }

        //读取存档列表
        private void LoadSaves()
        {
            dataItems = SaveSystem.Instance.LoadEditDatas();
            foreach (var d in dataItems)
            {
                GameObject o = Instantiate(savePrefab, savesParent);
                o.SetActive(true);
                o.transform.GetChild(0).GetComponent<Text>().text = d.name;
                saveItemObjs.Add(o);
                int index = dataItems.IndexOf(d);
                o.GetComponent<Button>().onClick.AddListener(() => { SelectSave(index); });
                savesParent.GetComponent<ItemsContainer>().AddItem(o);
            }
        }

        //选择
        private void SelectSave(int index)
        {
            this.index = index;
            if (saveItemObjs.Count > 0)
                savesParent.GetComponent<ItemsContainer>().Select(saveItemObjs[index]);
        }

        //删除数据
        private void DeleteSave(int index)
        {
            if (index >= 0 && index < dataItems.Count && SaveSystem.Instance.DeleteEditData(dataItems[index].name))
            {
                UpdateItems();
            }
        }

        //清除列表
        private void ClearSaves()
        {
            foreach (GameObject o in saveItemObjs)
            {
                Destroy(o);
            }
            saveItemObjs.Clear();
            savesParent.GetComponent<ItemsContainer>().Clear();
        }
    }
}

