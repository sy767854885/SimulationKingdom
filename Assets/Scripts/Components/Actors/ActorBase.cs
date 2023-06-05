using Components.Grid;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorBase : MonoBehaviour
{
    private bool isHoldProperty=false;
    private PropertyType holdProperty;//携带的资源类型
    private int holdPropertyCount=0;//携带的资源数量

    private string workForId = "";       //服务对象
    private JobType jobType=JobType.None;//决定着装
    private PropertyType workForProperty;//决定工作模式

    private int state = 0;
    private int subState = 0;
    private float stateTimer = 0;

    private float randomStayTimer;
    private Vector3 randomTarget= Vector3.zero;

    
    private GameObject workforBuidingObj;//工作的建筑
    private Building building;

    private string produceStationKey;//在建筑中的生产位的key
    private PropertyType propertyForCounsum;
    private int propertyMaxCount;//能带回的资源的最大数量
    private GameObject targetBuildingObj;
    private Building targetBuilding;
    

    private GameObject treeObj;

    

    public JobType JobType { get { return jobType; } }
    public string WorkForId { get { return workForId; } }
    public PropertyType WorkForProperty { get { return workForProperty; } }


    //是否已经被雇佣
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

    //设置工作
    public void SetJob(JobType jobType)
    {
        this.jobType = jobType;
    }

    //设置工作的对象
    public void SetWorkPropertyType(PropertyType  propertyType)
    {
        this.workForProperty = propertyType;
    }

    //被解雇
    public void UnEmployed()
    {
        SetWorkForId(null);
        SetJob(JobType.None);
    }

    //从建筑中获取资源
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

    //将角色身上的资源放到建筑中
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
            workforBuidingObj = WorldManager.Instance.GetGridTransformWithId(WorkForId).gameObject; //找到工作的建筑
            building=workforBuidingObj.GetComponent<Building>();
        }else if (state == 2)//进入工作状态
        {
            subState = 0;
        }else if (state == 3)//寻找和砍伐树木状态
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
    //移动到目标位置
    private bool MoveToPositionUpdate(Vector3 targetPosition)
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * 3);

        var lookDirection = targetPosition - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDirection), Time.deltaTime * 100);

        return Vector3.Distance(transform.position, targetPosition) <= 0.1f;
    }
    //==================================================================================

    //随机运动状态，这里模拟无事可做的情况
    private void RandomMoveState()
    {
        if (subState == 0)//停顿状态
        {
            randomStayTimer += Time.deltaTime;
            if (randomStayTimer >= 0.5f)
            {
                GetRandomStayPoint();
                subState = 1;
            }
        }else if (subState == 1)//移动状态
        {
            if (MoveToPositionUpdate(randomTarget))
            {
                randomStayTimer = 0;
                subState = 0;
            }
        }
        if (IsEmployed&&stateTimer>=5)//如果已经被雇佣，那么转换到向着工作地点前进状态
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

    //走向建筑确认工作
    private void MoveForConfirmJobState()
    {
        if (subState == 0)//移动工作建筑位置
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

    //检查需要转到什么工作状态
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

    //生产工作状态
    private void ProduceJobState()
    {
        if (subState == 0)
        {
            //确认生产类型
            ProduceComponent produceComponent=building.GetComponent<ProduceComponent>();
            if (produceComponent && produceComponent.GetPropertyTypes().Contains(WorkForProperty))
            {
                //建立工作资源
                if (building.GetComponent<ProduceComponent>().GetProduceStation(workForProperty, out produceStationKey))
                {
                    building.GetComponent<ProduceComponent>().SetLockStation(produceStationKey, true);
                    subState = 1;
                }
                subState = 1;
            }
            else
            {
                //TODO 添加检查，需要转换到哪个采集状态（是砍树还是采集小麦）
                TransState(3);
            }
        }else if (subState == 1)
        {
            //进入生产
            //进行生产过程
            //如果无法进行生产，那么退出生产过程，并转换到0状态
            ProduceComponent produceComponent = building.GetComponent<ProduceComponent>();
            if (!produceComponent.Produce(produceStationKey, out bool canProduce, out int count, out PropertyType type))
            {
                if (!canProduce)
                {
                    produceComponent.SetLockStation(produceStationKey, false);
                    produceComponent.RemoveProduceStation(produceStationKey);
                    //TODO 调用离开建筑接口
                    bool enoughConsum = produceComponent.EnoughConsum(type, out List<PropertyCount> nessaryList);
                    bool typeArrivedMax = building.GetComponent<Storage>().ArrivedMaxValue(type);
                    if (typeArrivedMax)//如果资源已经满了，那么退出生产
                    {
                        TransState(0);
                    }else if (!enoughConsum)//如果是没有生产原料，那么转到寻找生产原料
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
    //采集木料工作状态
    private void ToCutTreeJobState()
    {
        if (subState == 0)//找到一棵树，并向树木移动
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
                TransState(0);//如果找不到树木进行砍伐，那么进入到闲逛状态
            }
        }
        else if (subState == 1)//对树木进行砍伐
        {
            if (treeObj)
            {
                //TODO 模拟砍伐
                WorldManager.Instance.gridPlacementForStructure.DestroyObjectFromTheMap(treeObj.GetComponent<GridTransform>());
                TransState(4);
            }
            else
            {
                subState = 0; //如果要砍伐的树木不存在，那么查找新的树木进行砍伐
            }
        }
    }
    //==================================================================================

    //运送采集的资源回建筑的状态
    private void TransCollectedProductJobState()
    {
        // 如果建筑不存在，那么转换到状态0，
        if (!building)
        {
            SetWorkForId(null); //TODO 销毁建筑的时候会首先解雇建筑对应的所有人员,后面将此方法取消，用解雇来代替
            TransState(0);
            return;
        }
        Vector3 targetPosition = building ? building.doorAnchor.position : workforBuidingObj.transform.position;
        if (MoveToPositionUpdate(targetPosition))
        {
            subState = 0;//将原木送到伐木场之后再进行查找，砍伐新的树木
            building.GetComponent<Storage>().Add(workForProperty, 1);
            //TODO 判断原木的储藏是否已经满了
            TransState(2);
        }
    }
    //==================================================================================

    //查找并获取所需生产原料
    private void GoFindMaterialsForProduceJobState()
    {
        if (subState == 0)//查找资源建筑
        {
            //查找资源
            if (FindBuildingOfProduceProperty(propertyForCounsum))
            {
                subState = 1;
            }else
            {
                TransState(0);
            }
        }else if (subState == 1)//前往建筑
        {
            Vector3 targetPosition = targetBuilding? targetBuilding.doorAnchor.position : targetBuildingObj.transform.position;
            if (MoveToPositionUpdate(targetPosition))
            {
                subState = 2;
            }
        }else if (subState == 2)
        {
            //模拟播放拿取动画
            Storage storage=targetBuildingObj.GetComponent<Storage>();
            if(GetPropertyFromStorage(propertyForCounsum, propertyMaxCount, storage))//从建筑中获取到资源
            {
                subState = 3;
            }else
            {
                subState = 0;
            }
        }else if (subState == 3)//携带资源回到生产基地
        {
            // 如果建筑不存在，那么转换到状态0，
            if (!building)
            {
                SetWorkForId(null); //TODO 销毁建筑的时候会首先解雇建筑对应的所有人员,后面将此方法取消，用解雇来代替
                TransState(0);
                return;
            }
            Vector3 targetPosition = building ? building.doorAnchor.position : workforBuidingObj.transform.position;
            if (MoveToPositionUpdate(targetPosition))
            {
                SendPropertyToStorage(building.GetComponent<Storage>());
                if (isHoldProperty) ThrowProperty();
                //将身上的资源放置到建筑
                TransState(2);
            }
        }
    }

    //找到生产某种资源的建筑
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



    //伐木工的工作状态
    private void LumberjackJobState()
    {
        if (subState == 0)//找到一棵树，并向树木移动
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
                TransState(0);//如果找不到树木进行砍伐，那么进入到闲逛状态
            }
        }else if (subState == 1)//对树木进行砍伐
        {
            if (treeObj)
            {
                //TODO 模拟砍伐
                WorldManager.Instance.gridPlacementForStructure.DestroyObjectFromTheMap(treeObj.GetComponent<GridTransform>());
                subState = 2;
            }
            else
            {
                subState=0; //如果要砍伐的树木不存在，那么查找新的树木进行砍伐
            }
        }
        else if (subState == 2)//将原木资源运送到伐木厂
        {
            // 如果建筑不存在，那么转换到状态0，
            if (!building)
            {
                SetWorkForId(null); //TODO 销毁建筑的时候会首先解雇建筑对应的所有人员,后面将此方法取消，用解雇来代替
                TransState(0);
                return;
            }
            Vector3 targetPosition = building ? building.doorAnchor.position : workforBuidingObj.transform.position;
            if (MoveToPositionUpdate(targetPosition))
            {
                subState = 0;//将原木送到伐木场之后再进行查找，砍伐新的树木
               
                //将身上多余的资源扔掉
            }
        }
    }

    //寻找一个树用来砍伐
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

    //烧炭工人的工作状态
    private void CharcoalJobState()
    {
        if (subState == 0)
        {
            //TODO 进入建筑
            //建立工作资源
            if (building.GetComponent<ProduceComponent>().GetProduceStation(workForProperty, out produceStationKey))
            {
                building.GetComponent<ProduceComponent>().SetLockStation(produceStationKey, true);
                subState = 1;
            }
        }
        else if(subState == 1){
            //进行生产过程
            //如果无法进行生产，那么退出生产过程，并转换到0状态
            if(building.GetComponent<ProduceComponent>().Produce(produceStationKey,out bool canProduce,out int count,out PropertyType type))
            {
                //Debug.Log($"生产资源{count}份{type},当前建筑中{type}总数{building.GetComponent<Storage>().GetPropertyValue(type)}");
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
