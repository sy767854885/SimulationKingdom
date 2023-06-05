using Components.Grid;
using GameSys.Build;
using GameSys.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Utilities;

public enum GameState
{
    None,
    Running,
    ReadyExit,
    Edit,
}

public class GameManager : PersistentSingleton<GameManager>
{
    GameState gameState=GameState.None;

    private Action sceneLoadedAction;

    public SaveSystem saveSystem;

    private string currentSaveName;

    private ActionController actionController;

    private void Update()
    {
        if (gameState == GameState.None)
        {

        }else if (gameState == GameState.Running)
        {
            if (Input.GetMouseButtonDown(1))
            {
                RemoveActionController();
                CreateDefaultActionController();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (actionController != null && actionController.GetType() != typeof(GameSys.SelectController))
                {
                    RemoveActionController();
                    CreateDefaultActionController();
                }
                else
                {
                    ReadyQuit();
                }
            }
        }
        else if (gameState == GameState.ReadyExit)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ContinuePlay();
            }
        }else if (gameState == GameState.Edit)
        {
            if (Input.GetMouseButtonDown(1))
            {
                RemoveActionController();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                RemoveActionController();
            }
        }
    }


    //使用已经存储的地图存档开始新游戏
    public void PlayGameWithMap(string mapSaveName)
    {
        PropertiesManager.Instance.Build();
        GameSys.UI.UIManager.Instance.CloseAllOpeninigPanels();
        //LoadEditData(mapSaveName);
        currentSaveName = null;
        SaveLoad.Instance.LoadEditData(mapSaveName);
        UIManager.Instance.OpenSinglePanel<PlayPanel>();
        gameState = GameState.Running;
        CreateDefaultActionController();
    }

    //使用存档开始游戏
    public void PlayGameWithSave(string saveName)
    {
        PropertiesManager.Instance.Build();
        GameSys.UI.UIManager.Instance.CloseAllOpeninigPanels();
        currentSaveName = saveName;
        //LoadStructureToMap(saveName);
        SaveLoad.Instance.LoadSaveData(saveName);
        UIManager.Instance.OpenSinglePanel<PlayPanel>();
        gameState = GameState.Running;
        CreateDefaultActionController();
    }


    //准备退出
    public void ReadyQuit()
    {
        UIManager.Instance.OpenSinglePanel<PlayExitPanel>();
        gameState=GameState.ReadyExit;
    }

    //继续游戏
    public void ContinuePlay()
    {
        UIManager.Instance.CloseSinglePanel<PlayExitPanel>();
        CreateDefaultActionController();
        gameState = GameState.Running;
    }

    //退出游戏
    public void QuitPlay()
    {
        RemoveActionController();
        UIManager.Instance.CloseAllOpeninigPanels();
        SceneManager.LoadScene("MainScene");
        WorldManager.Instance.Clear();
        gameState = GameState.None;
        sceneLoadedAction = () =>
        {
            sceneLoadedAction = null;
            UIManager.Instance.OpenSinglePanel<MainPanel>();
        };
    }

    //==================================================================================

    public void PlayMapEdit()
    {
        UIManager.Instance.CloseAllOpeninigPanels();
        UIManager.Instance.OpenSinglePanel<EditMapPanel>();
        EditManager.Instance.SetSize(EditManager.Instance.defaultSize);
        WorldManager.Instance.ShowBorder(true);
        gameState = GameState.Edit;
        currentSaveName = null;
    }

    public void LoadEdit(string saveName)
    {
        currentSaveName = saveName;
        UIManager.Instance.CloseAllOpeninigPanels();
        SaveLoad.Instance.LoadEditData(saveName);
        UIManager.Instance.OpenSinglePanel<EditMapPanel>();
        WorldManager.Instance.ShowBorder(true);
        WorldManager.Instance.SetCheckPlane();
        gameState = GameState.Edit;
    }

    public void QuitMapEdit()
    {
        RemoveActionController();
        WorldManager.Instance.Clear();
        UIManager.Instance.CloseAllOpeninigPanels();
        UIManager.Instance.OpenSinglePanel<MainPanel>();
        WorldManager.Instance.ShowBorder(false);
        WorldManager.Instance.SetCheckPlane();
        gameState = GameState.None;
    }

    public void SaveEdit(bool isNew)
    {
        currentSaveName = SaveLoad.Instance.SaveEdit(isNew, currentSaveName);
    }

    //==================================================================================

    //生成建造控制器
    public void CreateBuildController(string id)
    {
        RemoveActionController();
        actionController = BuildManager.Instance.CreateBuildController(id, transform);
    }

    //生成摧毁控制器
    public void CreateDestroyController()
    {
        RemoveActionController();
        actionController =BuildManager.Instance.CreateDestroyController();
    }

    //创建编辑控制器
    public void CreateEditController(string id, bool isGround)
    {
        RemoveActionController();
        actionController = EditManager.Instance.CreateEditController(id, transform, isGround);
    }

    //创建编辑模式下的摧毁控制器
    public void CreateEditDestroyController()
    {
        RemoveActionController();
        actionController = EditManager.Instance.CreateDestroyController();
    }

    //移除控制器
    private void RemoveActionController()
    {
        if (actionController != null)
        {
            actionController.Clear();
            Destroy(actionController.gameObject);
        }
    }
    
    //创建默认控制器
    private void CreateDefaultActionController()
    {
        GameObject o = new GameObject("selectController");
        o.transform.parent = transform;
        actionController = o.AddComponent<GameSys.SelectController>();
    }

    public void SaveGame(bool isNew)
    {
        currentSaveName = SaveLoad.Instance.SaveGame(isNew, currentSaveName);
    }


    //==================================================================================




    //==================================================================================

    private void OnLevelWasLoaded(int level)
    {
        sceneLoadedAction?.Invoke();
    }
}
