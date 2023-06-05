using GameSys.Structure;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameSys.UI
{
    public class StructureInfoPanel : MonoBehaviour
    {
        public Button closeButton;
        public Image iconImage;
        public Text nameText;
        [Space]
        public Transform propertysParent;
        public GameObject propertyItemPrefab;
        private List<GameObject> storageList=new List<GameObject>();
        [Space]
        public GameObject employPanel;
        public Text employCountText;
        public Button employButton;
        public Transform emplyeeParent;
        public GameObject emplyeeItemPrefab;
        private List<GameObject> employeeList=new List<GameObject>();

        [Space]
        private GameObject targetObj;

        private void Start()
        {
            closeButton.onClick.AddListener(Close);
            employButton.onClick.AddListener(EmployResident);
        }

        public void Open(Transform target)
        {
            targetObj = target.gameObject;
            Clear();
            //��ʾIcon
            StructureModel sModel=target.GetComponent<StructureModel>();
            ShowTargetIcon(sModel.prefabId);
            gameObject.SetActive(true);
            ShowStorage(target.gameObject);
            ShowEmploy(target.gameObject);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

        private void Clear()
        {
            ClearStorageObjs();
            ClearEmploeeObjs();
        }

        //��ʾͼ�� 
        private void ShowTargetIcon(string id)
        {
            var mData = StructuresManager.Instance.GetStructureData(id);
            iconImage.sprite = mData.sprite;
            gameObject.SetActive(true);
            nameText.text = mData.name;
        }

        //==================================================================================

        //��ʾ�洢
        private void ShowStorage(GameObject target)
        {
            ClearStorageObjs();
            Storage storage = target.GetComponent<Storage>();
            if (!storage) { propertysParent.gameObject.SetActive(false);  return; }
            propertysParent.gameObject.SetActive(true);
            foreach (var d in storage.propertyDatas)
            {
                GameObject o = Instantiate(propertyItemPrefab, propertysParent);
                storageList.Add(o);
                o.SetActive(true);
                var config = PropertiesManager.Instance.GetPropertyConfigData(d.type);
                o.transform.Find("Image").GetComponent<Image>().sprite = config.icon;
                string numberText = d.useUnLimit ? "u" : d.value.ToString() + (d.useMaxLimit ? "/" + d.maxValue : "");
                o.transform.Find("NumberText").GetComponent<Text>().text = numberText;
                o.transform.Find("NameText").GetComponent<Text>().text = config.name;
            }
        }

        //��մ洢����
        private void ClearStorageObjs()
        {
            foreach (GameObject o in storageList)
            {
                Destroy(o);
            }
            storageList.Clear();
        }

        //==================================================================================

        //��ʾ��Ӷ
        private void ShowEmploy(GameObject target)
        {
            Employ employ = target.GetComponent<Employ>();
            if (!employ) { employPanel.SetActive(false); return; }
            employPanel.SetActive(true);
            UpdateEmployView(employ);
        }

        //��ӹ�Ա
        public void EmployResident()
        {
            if (EmployManager.Instance.EmployForStructor(targetObj.GetComponent<IdComponent>().id))
            {
                UpdateEmployView(targetObj.GetComponent<Employ>());
            }
        }

        //���
        public void UnEmployResident(string emploeeId)
        {
            if (EmployManager.Instance.UnEmployFromStructure(targetObj.GetComponent<IdComponent>().id,emploeeId))
            {
                UpdateEmployView(targetObj.GetComponent<Employ>());
            }
        }

        //���Ĺ�������
        public void SwitchWorkType(string emploeeId)
        {
            if (EmployManager.Instance.SwitchWork(targetObj.GetComponent<IdComponent>().id,emploeeId))
            {
                UpdateEmployView(targetObj.GetComponent<Employ>());
            }
        }

        //������ʾ
        private void UpdateEmployView(Employ employ)
        {
            employCountText.text = employ.emploees.Count.ToString() + "/" + employ.maxEmployCount.ToString();
            ShowEmployItems(employ);
        }

        //��ʾ��Ա
        private void ShowEmployItems(Employ employ)
        {
            ClearEmploeeObjs();
            foreach (string id in employ.emploees)
            {
                GameObject o = Instantiate(emplyeeItemPrefab, emplyeeParent);
                o.SetActive(true);
                employeeList.Add(o);
                
                //��ʾְҵͼ��
                var workInfo=EmployManager.Instance.GetResidentWorkInfo(id);
                Sprite workForIcon = null;
                if (workInfo.jobType == JobType.Porter)
                {
                    JobTypeData jobTypeData=ActorsManager.Instance.GetJobTypeInfo(workInfo.jobType);
                    workForIcon = jobTypeData.icon;
                }
                else
                {
                    var pData = PropertiesManager.Instance.GetPropertyConfigData(workInfo.propertyType);
                    workForIcon = pData.icon;
                }
                o.transform.Find("WorkImage").GetComponent<Image>().sprite = workForIcon;
                string tempId = id;
                o.transform.Find("UnEmployButton").GetComponent<Button>().onClick.AddListener(() => { UnEmployResident(tempId); });
                o.transform.Find("ChangeButton").GetComponent<Button>().onClick.AddListener(() => { SwitchWorkType(tempId); });
            }
        }

        //��չ�ԱUI�б�
        private void ClearEmploeeObjs()
        {
            foreach(GameObject o in employeeList)
            {
                Destroy(o);
            }
        }
    }
}

