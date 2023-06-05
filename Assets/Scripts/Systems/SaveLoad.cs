using GameSys.Build;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class SaveLoad : PersistentSingleton<SaveLoad>
{
    //保存游戏
    public string SaveGame(bool isNew,string saveName)
    {
        SaveDataSerialization saveData = WorldManager.Instance.ExportData();
        var jsonFormat = JsonUtility.ToJson(saveData);
        Debug.Log(jsonFormat);
        if (isNew || string.IsNullOrEmpty(saveName))
        {
            saveName = $"SaveData_{TimeTool.GetTimeStamp()}";
            SaveSystem.Instance.SaveData(jsonFormat, saveName);
        }
        else
        {
            SaveSystem.Instance.SaveData(jsonFormat, saveName);
        }
        return saveName;
    }


    public void LoadSaveData(string saveName)
    {
        var jsonFormatData = SaveSystem.Instance.LoadData(saveName);
        if (String.IsNullOrEmpty(jsonFormatData))
            return;
        SaveDataSerialization saveData = JsonUtility.FromJson<SaveDataSerialization>(jsonFormatData);
        LoadSceneWithSerialization(saveData);
    }

    //==================================================================================
    //保存编辑的地图
    public string SaveEdit(bool isNew,string saveName)
    {
        SaveDataSerialization saveData = WorldManager.Instance.ExportData();
        var jsonFormat = JsonUtility.ToJson(saveData);
        Debug.Log(jsonFormat);
        if (isNew || string.IsNullOrEmpty(saveName))
        {
            saveName = $"SaveEditData_{TimeTool.GetTimeStamp()}";
            SaveSystem.Instance.SaveEditData(jsonFormat, saveName);
        }
        else
        {
            SaveSystem.Instance.SaveEditData(jsonFormat, saveName);
        }
        return saveName;
    }
    
    //加载编辑数据
    public void LoadEditData(string saveName)
    {
        var jsonFormatData = SaveSystem.Instance.LoadEditData(saveName);
        if (String.IsNullOrEmpty(jsonFormatData))
            return;
        SaveDataSerialization saveData = JsonUtility.FromJson<SaveDataSerialization>(jsonFormatData);
        LoadSceneWithSerialization(saveData);
    }

    //==================================================================================

    //读取存档数据
    private void LoadSceneWithSerialization(SaveDataSerialization saveData)
    {
        WorldManager.Instance.SetWorldSize(saveData.worldSize.GetValue());
        foreach (var sData in saveData.groundsData)
        {
            EditManager.Instance.PlaceGround(sData.prefabId, sData.position.GetValue());
        }
        foreach (var sData in saveData.structuresData)
        {
            BuildManager.Instance.PlaceStructure(sData.prefabId, sData.position.GetValue(), sData.rotationValue);
        }
    }
}
