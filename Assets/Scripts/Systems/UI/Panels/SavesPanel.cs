using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameSys.UI
{
    public class SavesPanel : PanelBasic
    {
        public GameObject savePrefab;
        public Transform savesParent;
        [Space]
        public Button loadButton;
        public Button deleteButton;
        public Button backButton;

        private List<DataItem> dataItems = new List<DataItem>();
        private List<GameObject> saveItemObjs = new List<GameObject>();
        private int index;

        private void Start()
        {
            loadButton.onClick.AddListener(() => { Load(); });
            deleteButton.onClick.AddListener(() => { DeleteSave(index); });
            backButton.onClick.AddListener(() => { Back(); });
        }

        public override void Open()
        {
            base.Open();
            UpdateItems();
        }

        private void UpdateItems()
        {
            ClearSaves();
            LoadSaves();
            SelectSave(0);
        }

        //读取存档列表
        private void LoadSaves()
        {
            dataItems = SaveSystem.Instance.LoadDatas();
            foreach (var d in dataItems)
            {
                GameObject o = Instantiate(savePrefab, savesParent);
                o.SetActive(true);
                o.transform.GetChild(0).GetComponent<Text>().text = d.name;
                saveItemObjs.Add(o);
                int index=dataItems.IndexOf(d);
                o.GetComponent<Button>().onClick.AddListener(() => { SelectSave(index); });
                savesParent.GetComponent<ItemsContainer>().AddItem(o);
            }
        }

        //选择
        private void SelectSave(int index)
        {
            this.index = index;
            if(saveItemObjs.Count>0)
                savesParent.GetComponent<ItemsContainer>().Select(saveItemObjs[index]);
        }

        //删除数据
        private void DeleteSave(int index)
        {
            if (index >= 0 && index < dataItems.Count&& SaveSystem.Instance.DeleteData(dataItems[index].name))
            {
                UpdateItems();
            }
        }

        //清除列表
        private void ClearSaves()
        {
            foreach(GameObject o in saveItemObjs)
            {
                Destroy(o);
            }
            saveItemObjs.Clear();
            savesParent.GetComponent<ItemsContainer>().Clear();
        }

        //加载游戏
        private void Load()
        {
            if (dataItems.Count == 0) return;
            GameManager.Instance.PlayGameWithSave(dataItems[index].name);
        }

        private void Back()
        {
            Close();
            UIManager.Instance.OpenSinglePanel<MainPanel>();
        }
    }
}

