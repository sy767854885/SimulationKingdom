using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSys.UI
{
    public class PanelBasic : MonoBehaviour
    {
        public GameObject panel;

        public virtual bool IsOpened { get { return panel.activeSelf; } }

        //´ò¿ªPanel
        public virtual void Open()
        {
            if (!panel) panel = gameObject;
            panel.SetActive(true);
        }

        //¹Ø±ÕPanel
        public virtual void Close()
        {
            if (!panel) panel = gameObject;
            panel.SetActive(false);
        }
    }
}

