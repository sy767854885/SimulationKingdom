using GameSys.Build;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameSys.UI
{
    public class MapHelperPanel : PanelBasic
    {
        public Action closeAction;
        public Button closeButton;

        public Button showIndexPosButton;
        public Button showPlaceButton;
        public Button hideObjsButton;

        private void Start()
        {
            closeButton.onClick.AddListener(() => { CloseWithAction(); });

            showIndexPosButton.onClick.AddListener(() => { ShowIndexPos(); });
            showPlaceButton.onClick.AddListener(() => { ShowPlace(); });
            hideObjsButton.onClick.AddListener(() => { HideObjs(); });
        }

        public override void Open()
        {
            base.Open();
            UpdateInfos();
            BuildManager.Instance.placedAction = (g) => { MapHelper.Instance.UpdateShow(); };
            BuildManager.Instance.fixedAction = (g) => { MapHelper.Instance.UpdateShow(); };
            BuildManager.Instance.destroyObjAction = () => { MapHelper.Instance.UpdateShow(); };
            BuildManager.Instance.destroyObjsAction = () => { MapHelper.Instance.UpdateShow(); };
            BuildManager.Instance.registAction = () => { MapHelper.Instance.UpdateShow(); };
        }

        public override void Close()
        {
            base.Close();
            MapHelper.Instance.Clear();
            closeAction = null;
        }

        public void CloseWithAction()
        {
            base.Close();
            MapHelper.Instance.Clear();
            closeAction?.Invoke();
            closeAction = null;
            BuildManager.Instance.placedAction = null;
            BuildManager.Instance.fixedAction = null;
            BuildManager.Instance.destroyObjAction = null;
            BuildManager.Instance.destroyObjsAction = null;
            BuildManager.Instance.registAction = null;
        }

        private void ShowIndexPos()
        {
            bool b = MapHelper.Instance.IsShowIndexPos;
            MapHelper.Instance.ShowIndexPos(!b);
            showIndexPosButton.GetComponent<UIFocus>().IsFocus = MapHelper.Instance.IsShowIndexPos;
        }

        private void ShowPlace()
        {
            bool b = MapHelper.Instance.IsShowPlace;
            MapHelper.Instance.ShowPlace(!b);
            showPlaceButton.GetComponent<UIFocus>().IsFocus = MapHelper.Instance.IsShowPlace;
        }

        private void HideObjs()
        {
            bool b=MapHelper.Instance.IsHideObjs;
            MapHelper.Instance.HideObjs(!b);
            hideObjsButton.GetComponent<UIFocus>().IsFocus = MapHelper.Instance.IsHideObjs;
        }

        private void UpdateInfos()
        {
            showIndexPosButton.GetComponent<UIFocus>().IsFocus = MapHelper.Instance.IsShowIndexPos;
            showPlaceButton.GetComponent<UIFocus>().IsFocus = MapHelper.Instance.IsShowPlace;
            hideObjsButton.GetComponent<UIFocus>().IsFocus = MapHelper.Instance.IsHideObjs;
        }
    }
}

