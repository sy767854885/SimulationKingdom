using GameSys.Structure;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameSys.UI
{
    public class UnitsPanel : MonoBehaviour
    {
        public Transform buttonsParent;
        public GameObject buttonPrefab;
        private List<GameObject> buttons = new List<GameObject>();
        private List<CatalogueItem> catalogueItems = new List<CatalogueItem>();
        [Space]
        public ItemsPanel itemsPanel;

        private int selectedIndex;
        public void Open()
        {
            gameObject.SetActive(true);
            LoadMainCatalogues();
        }

        public void Close()
        {
            gameObject.SetActive(false);
            itemsPanel.gameObject.SetActive(false);
            Clear();
        }

        //¶ÁÈ¡Ö÷Ä¿Â¼
        private void LoadMainCatalogues()
        {
            Clear();
            catalogueItems = StructuresManager.Instance.GetCatalogues();
            for (int i = 0; i < catalogueItems.Count; i++)
            {
                GameObject o = Instantiate(buttonPrefab, buttonsParent);
                o.SetActive(true);
                o.transform.GetChild(0).GetComponent<Image>().sprite = catalogueItems[i].sprite;
                buttons.Add(o);
                int index = i;
                o.GetComponent<Button>().onClick.AddListener(() => { Select(index); });
                buttonsParent.GetComponent<ItemsContainer>().AddItem(o);
            }
        }

        private void Clear()
        {
            for (int i = 0; i < buttons.Count; i++) Destroy(buttons[i].gameObject);
            buttons.Clear();
            catalogueItems.Clear();
            selectedIndex = -1;
            buttonsParent.GetComponent<ItemsContainer>().Clear();
        }



        private void Select(int index)
        {
            if (selectedIndex == index)
            {
                itemsPanel.gameObject.SetActive(false);
                selectedIndex = -1;
                buttonsParent.GetComponent<ItemsContainer>().UnSelect(buttons[index]);
                return;
            }
            buttonsParent.GetComponent<ItemsContainer>().Select(buttons[index]);
            selectedIndex = index;
            itemsPanel.gameObject.SetActive(true);
            itemsPanel.ShowItems(catalogueItems[index].id);
        }
    }
}