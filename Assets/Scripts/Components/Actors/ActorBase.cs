using Components.Grid;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorBase : MonoBehaviour
{
    private bool isHoldProperty=false;
    private PropertyType holdProperty;//Я������Դ����
    private int holdPropertyCount=0;//Я������Դ����

    private string workForId = "";       //�������
    private JobType jobType=JobType.None;//������װ
    private PropertyType workForProperty;//��������ģʽ

    private int state = 0;
    private int subState = 0;
    private float stateTimer = 0;

    private float randomStayTimer;
    private Vector3 randomTarget= Vector3.zero;

    
    private GameObject workforBuidingObj;//�����Ľ���
    private Building building;

    private string produceStationKey;//�ڽ����е�����λ��key
    private PropertyType propertyForCounsum;
    private int propertyMaxCount;//�ܴ��ص���Դ���������
    private GameObject targetBuildingObj;
    private Building targetBuilding;
    

    private GameObject treeObj;

    

    public JobType JobType { get { return jobType; } }
    public string WorkForId { get { return workForId; } }
    public PropertyType WorkForProperty { get { return workForProperty; } }


    //�Ƿ��Ѿ�����Ӷ
    public bool IsEmployed { get { return !string.IsNullOrEmpty(WorkForId); } }

    private void Update()
    {
        stateTimer += Time.deltaTime;
        if (state == 0)
        {
            RandomMoveState();
        }else if (state == 1)
        {
            MoveForConfirmJobState();
        }else if (state == 2)
        {
            ProduceJobState();
        }else if (state == 3)
        {
            ToCutTreeJobState();
        }else if (state == 4)
        {
            TransCollectedProductJobState();
        }else if (state == 5)
        {
            GoFindMaterialsForProduceJobState();
        }
    }

    public void SetWorkForId(string workId)
    {
        this.workForId = workId;
    }

    //���ù���
    public void SetJob(JobType jobType)
    {
        this.jobType = jobType;
    }

    //���ù����Ķ���
    public void SetWorkPropertyType(PropertyType  propertyType)
    {
        this.workForProperty = propertyType;
    }

    //�����
    public void UnEmployed()
    {
        SetWorkForId(null);
        SetJob(JobType.None);
    }

    //�ӽ����л�ȡ��Դ
    public bool GetPropertyFromStorage(PropertyType type,int count,Storage storage)
    {
        if (isHoldProperty) return false;
        if (count == 0) return false;
        if (storage.HasPropertyType(type))
        {
            holdPropertyCount = storage.Sub(type, count);
            if (holdPropertyCount == 0) return false;
            isHoldProperty = true;
            holdProperty = type;
            return true;
        }
        return false;
    }

    //����ɫ���ϵ���Դ�ŵ�������
    public bool SendPropertyToStorage(Storage storage)
    {
        if (!isHoldProperty) return false;
        if (holdPropertyCount==0) return false;
        if (storage.HasPropertyType(holdProperty))
        {
            int preHoldCount = holdPropertyCount;
            holdPropertyCount -= storage.Add(holdProperty, holdPropertyCount);
            if(holdProperty==0)isHoldProperty = false;
            return preHoldCount - holdPropertyCount > 0;
        }
        return false;
    }

    public void ThrowProperty()
    {
        isHoldProperty = false;
        holdPropertyCount = 0;
    }

    private void TransState(int targetState)
    {
        state = targetState;
        stateTimer = 0;
        if (state == 0)
        {
            subState = 0;
            randomStayTimer = 0f;
        }else if (state == 1)
        {
            subState = 0;
            workforBuidingObj = WorldManager.Instance.GetGridTransformWithId(WorkForId).gameObject; //�ҵ������Ľ���
            building=workforBuidingObj.GetComponent<Building>();
        }else if (state == 2)//���빤��״̬
        {
            subState = 0;
        }else if (state == 3)//Ѱ�ҺͿ�����ľ״̬
        {
            subState = 0;
        }else if (state == 4)
        {
            subState = 0;
        }else if (state == 5)
        {
            subState = 0;
        }
    }

    //==================================================================================
    //�ƶ���Ŀ��λ��
    private bool MoveToPositionUpdate(Vector3 targetPosition)
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * 3);

        var lookDirection = targetPosition - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDirection), Time.deltaTime * 100);

        return Vector3.Distance(transform.position, targetPosition) <= 0.1f;
    }
    //==================================================================================

    //����˶�״̬������ģ�����¿��������
    private void RandomMoveState()
    {
        if (subState == 0)//ͣ��״̬
        {
            randomStayTimer += Time.deltaTime;
            if (randomStayTimer >= 0.5f)
            {
                GetRandomStayPoint();
                subState = 1;
            }
        }else if (subState == 1)//�ƶ�״̬
        {
            if (MoveToPositionUpdate(randomTarget))
            {
                randomStayTimer = 0;
                subState = 0;
            }
        }
        if (IsEmployed&&stateTimer>=5)//����Ѿ�����Ӷ����ôת�������Ź����ص�ǰ��״̬
        {
            TransState(1);
        }
    }

    private void GetRandomStayPoint()
    {
        Vector3 orginalPos = WorldManager.Instance.gridPlacementForStructure.gridSystem.grid.OriginalPosition;
        Vector2 size = WorldManager.Instance.gridPlacementForStructure.gridSystem.grid.GetSize();
        randomTarget = new Vector3(Random.Range(orginalPos.x, orginalPos.x + size.x), orginalPos.y, Random.Range(orginalPos.z, orginalPos.z + size.y));
    }
    //==================================================================================

    //������ȷ�Ϲ���
    private void MoveForConfirmJobState()
    {
        if (subState == 0)//�ƶ���������λ��
        {
            Vector3 targetPosition = building ? building.doorAnchor.position : workforBuidingObj.transform.position;
            if (MoveToPositionUpdate(targetPosition))
            {
                subState = 1;
            }
        }
        else if (subState == 1)
        {
            if (!IsEmployed) TransState(0);
            else CheckTransForJobState();
        }
    }

    //�����Ҫת��ʲô����״̬
    private void CheckTransForJobState()
    {
        if (jobType == JobType.Porter)
        {
            TransState(0);
            return;
        }

        if (jobType == JobType.Builder)
        {
            TransState(0);
            return;
        }

        if (jobType==JobType.Producer)
        {
            TransState(2);
        }
    }

    //==================================================================================

    //��������״̬
    private void ProduceJobState()
    {
        if (subState == 0)
        {
            //ȷ����������
            ProduceComponent produceComponent=building.GetComponent<ProduceComponent>();
            if (produceComponent && produceComponent.GetPropertyTypes().Contains(WorkForProperty))
            {
                //����������Դ
                if (building.GetComponent<ProduceComponent>().GetProduceStation(workForProperty, out produceStationKey))
                {
                    building.GetComponent<ProduceComponent>().SetLockStation(produceStationKey, true);
                    subState = 1;
                }
                subState = 1;
            }
            else
            {
                //TODO ��Ӽ�飬��Ҫת�����ĸ��ɼ�״̬���ǿ������ǲɼ�С��
                TransState(3);
            }
        }else if (subState == 1)
        {
            //��������
            //������������
            //����޷�������������ô�˳��������̣���ת����0״̬
            ProduceComponent produceComponent = building.GetComponent<ProduceComponent>();
            if (!produceComponent.Produce(produceStationKey, out bool canProduce, out int count, out PropertyType type))
            {
                if (!canProduce)
                {
                    produceComponent.SetLockStation(produceStationKey, false);
                    produceComponent.RemoveProduceStation(produceStationKey);
                    //TODO �����뿪�����ӿ�
                    bool enoughConsum = produceComponent.EnoughConsum(type, out List<PropertyCount> nessaryList);
                    bool typeArrivedMax = building.GetComponent<Storage>().ArrivedMaxValue(type);
                    if (typeArrivedMax)//�����Դ�Ѿ����ˣ���ô�˳�����
                    {
                        TransState(0);
                    }else if (!enoughConsum)//�����û������ԭ�ϣ���ôת��Ѱ������ԭ��
                    {
                        propertyForCounsum = nessaryList[0].type;
                        propertyMaxCount = building.GetComponent<Storage>().GetMaxValue(propertyForCounsum) - building.GetComponent<Storage>().GetPropertyValue(propertyForCounsum);
                        TransState(5);
                    }
                }
            }
        }
    }

    //==================================================================================
    //�ɼ�ľ�Ϲ���״̬
    private void ToCutTreeJobState()
    {
        if (subState == 0)//�ҵ�һ������������ľ�ƶ�
        {
            if (treeObj == null) FindTreeForCut();
            if (treeObj && IsEmployed)
            {
                MoveToPositionUpdate(treeObj.transform.position);
                if (Vector3.Distance(transform.position, treeObj.transform.position) <= 1)
                {
                    subState = 1;
                }
            }
            else
            {
                TransState(0);//����Ҳ�����ľ���п�������ô���뵽�й�״̬
            }
        }
        else if (subState == 1)//����ľ���п���
        {
            if (treeObj)
            {
                //TODO ģ�⿳��
                WorldManager.Instance.gridPlacementForStructure.DestroyObjectFromTheMap(treeObj.GetComponent<GridTransform>());
                TransState(4);
            }
            else
            {
                subState = 0; //���Ҫ��������ľ�����ڣ���ô�����µ���ľ���п���
            }
        }
    }
    //==================================================================================

    //���Ͳɼ�����Դ�ؽ�����״̬
    private void TransCollectedProductJobState()
    {
        // ������������ڣ���ôת����״̬0��
        if (!building)
        {
            SetWorkForId(null); //TODO ���ٽ�����ʱ������Ƚ�ͽ�����Ӧ��������Ա,���潫�˷���ȡ�����ý��������
            TransState(0);
            return;
        }
        Vector3 targetPosition = building ? building.doorAnchor.position : workforBuidingObj.transform.position;
        if (MoveToPositionUpdate(targetPosition))
        {
            subState = 0;//��ԭľ�͵���ľ��֮���ٽ��в��ң������µ���ľ
            building.GetComponent<Storage>().Add(workForProperty, 1);
            //TODO �ж�ԭľ�Ĵ����Ƿ��Ѿ�����
            TransState(2);
        }
    }
    //==================================================================================

    //���Ҳ���ȡ��������ԭ��
    private void GoFindMaterialsForProduceJobState()
    {
        if (subState == 0)//������Դ����
        {
            //������Դ
            if (FindBuildingOfProduceProperty(propertyForCounsum))
            {
                subState = 1;
            }else
            {
                TransState(0);
            }
        }else if (subState == 1)//ǰ������
        {
            Vector3 targetPosition = targetBuilding? targetBuilding.doorAnchor.position : targetBuildingObj.transform.position;
            if (MoveToPositionUpdate(targetPosition))
            {
                subState = 2;
            }
        }else if (subState == 2)
        {
            //ģ�ⲥ����ȡ����
            Storage storage=targetBuildingObj.GetComponent<Storage>();
            if(GetPropertyFromStorage(propertyForCounsum, propertyMaxCount, storage))//�ӽ����л�ȡ����Դ
            {
                subState = 3;
            }else
            {
                subState = 0;
            }
        }else if (subState == 3)//Я����Դ�ص���������
        {
            // ������������ڣ���ôת����״̬0��
            if (!building)
            {
                SetWorkForId(null); //TODO ���ٽ�����ʱ������Ƚ�ͽ�����Ӧ��������Ա,���潫�˷���ȡ�����ý��������
                TransState(0);
                return;
            }
            Vector3 targetPosition = building ? building.doorAnchor.position : workforBuidingObj.transform.position;
            if (MoveToPositionUpdate(targetPosition))
            {
                SendPropertyToStorage(building.GetComponent<Storage>());
                if (isHoldProperty) ThrowProperty();
                //�����ϵ���Դ���õ�����
                TransState(2);
            }
        }
    }

    //�ҵ�����ĳ����Դ�Ľ���
    private bool FindBuildingOfProduceProperty(PropertyType propertyType)
    {
        Employ[] arr = GameObject.FindObjectsOfType<Employ>();
        foreach(Employ e in arr)
        {
            if (!e.propertyTypes.Contains(propertyType)) continue;
            if (e.GetComponent<Storage>().GetPropertyValue(propertyType) > 0)
            {
                targetBuildingObj = e.gameObject;
                targetBuilding = e.GetComponent<Building>();
                return true;
            }
        }
        targetBuildingObj = null;
        building = null;
        return false;
    }



    //==================================================================================



    //��ľ���Ĺ���״̬
    private void LumberjackJobState()
    {
        if (subState == 0)//�ҵ�һ������������ľ�ƶ�
        {
            if(treeObj==null)FindTreeForCut();
            if (treeObj&&IsEmployed)
            {
                MoveToPositionUpdate(treeObj.transform.position);
                if (Vector3.Distance(transform.position, treeObj.transform.position) <= 1)
                {
                    subState = 1;
                }
            }
            else
            {
                TransState(0);//����Ҳ�����ľ���п�������ô���뵽�й�״̬
            }
        }else if (subState == 1)//����ľ���п���
        {
            if (treeObj)
            {
                //TODO ģ�⿳��
                WorldManager.Instance.gridPlacementForStructure.DestroyObjectFromTheMap(treeObj.GetComponent<GridTransform>());
                subState = 2;
            }
            else
            {
                subState=0; //���Ҫ��������ľ�����ڣ���ô�����µ���ľ���п���
            }
        }
        else if (subState == 2)//��ԭľ��Դ���͵���ľ��
        {
            // ������������ڣ���ôת����״̬0��
            if (!building)
            {
                SetWorkForId(null); //TODO ���ٽ�����ʱ������Ƚ�ͽ�����Ӧ��������Ա,���潫�˷���ȡ�����ý��������
                TransState(0);
                return;
            }
            Vector3 targetPosition = building ? building.doorAnchor.position : workforBuidingObj.transform.position;
            if (MoveToPositionUpdate(targetPosition))
            {
                subState = 0;//��ԭľ�͵���ľ��֮���ٽ��в��ң������µ���ľ
               
                //�����϶������Դ�ӵ�
            }
        }
    }

    //Ѱ��һ������������
    private void FindTreeForCut()
    {
        treeObj = null;
        GameObject[] arr = GameObject.FindGameObjectsWithTag("Tree");
        if (arr == null || arr.Length == 0) return;
        treeObj=arr[0];
        foreach (GameObject go in arr)
        {
            if (go == treeObj) continue;
            if (Vector3.Distance(transform.position, go.transform.position) < Vector3.Distance(transform.position, treeObj.transform.position))
            {
                treeObj=go;
            }
        }
    }

    //==================================================================================

    //��̿���˵Ĺ���״̬
    private void CharcoalJobState()
    {
        if (subState == 0)
        {
            //TODO ���뽨��
            //����������Դ
            if (building.GetComponent<ProduceComponent>().GetProduceStation(workForProperty, out produceStationKey))
            {
                building.GetComponent<ProduceComponent>().SetLockStation(produceStationKey, true);
                subState = 1;
            }
        }
        else if(subState == 1){
            //������������
            //����޷�������������ô�˳��������̣���ת����0״̬
            if(building.GetComponent<ProduceComponent>().Produce(produceStationKey,out bool canProduce,out int count,out PropertyType type))
            {
                //Debug.Log($"������Դ{count}��{type},��ǰ������{type}����{building.GetComponent<Storage>().GetPropertyValue(type)}");
                //building.GetComponent<Storage>().Add(type, count);
            }
            else
            {
                if (!canProduce)
                {
                    building.GetComponent<ProduceComponent>().SetLockStation(produceStationKey, false);
                    building.GetComponent<ProduceComponent>().RemoveProduceStation(produceStationKey);
                    TransState(0);
                }
            }
        }
    }
    //==================================================================================
}
