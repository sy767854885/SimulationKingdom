using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSys.UI
{
    public class ItemsContainer : MonoBehaviour
    {
        private List<GameObject> items = new List<GameObject>();

        public void AddItem(GameObject o)
        {
            if (items.Contains(o)) return;
            items.Add(o);
        }

        public void RemoveItem(GameObject o)
        {
            items.Remove(o);
        }

        public void Clear()
        {
            items.Clear();
        }

        public void Select(GameObject o)
        {
            foreach (GameObject go in items)
            {
                if (go == o) go.GetComponent<UIFocus>().IsFocus = true;
                else go.GetComponent<UIFocus>().IsFocus = false;
            }
        }

        public void UnSelect(GameObject o)
        {
            if (!items.Contains(o)) return;
            o.GetComponent<UIFocus>().IsFocus = false;
        }
    }
}

