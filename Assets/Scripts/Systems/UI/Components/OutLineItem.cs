using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameSys.UI
{
    public class OutLineItem : UIFocus
    {
        private Outline outLine;

        public override bool IsFocus { 
            get { if (!outLine) outLine = GetComponent<Outline>();return outLine.enabled; } 
            set { if (!outLine) outLine = GetComponent<Outline>();outLine.enabled = value; }
        }
    }
}

