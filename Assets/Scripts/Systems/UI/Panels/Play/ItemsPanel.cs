using GameSys.Structure;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameSys.UI
{
    public class ItemsPanel : MonoBehaviour
    {
        public GameObject itemsParent;
        public GameObject itemPrefab;
        private List<GameObject> items = new List<GameObject>();
        private List<StructureData> structureDatas = new List<StructureData>();

        public void ShowItems(string cId)
        {
            Clear();
            structureDatas = StructuresManager.Instance.GetCatalogueStructureDatas(cId);
            for (int i = 0; i < structureDatas.Count; i++)
            {
                GameObject o = Instantiate(itemPrefab, itemsParent.transform);
                items.Add(o);
                o.SetActive(true);
                o.transform.GetChild(0).GetComponent<Image>().sprite = structureDatas[i].sprite;
                string id = structureDatas[i].id;
                o.GetComponent<Button>().onClick.AddListener(() => { GameManager.Instance.CreateBuildController(id); });
            }
        }

        public void Clear()
        {
            foreach (GameObject o in items) Destroy(o);
            items.Clear();
            structureDatas.Clear();
        }
    }

}
