using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Utilities;

public class SaveSystem : PersistentSingleton<SaveSystem>
{

    //运行数据保存在名为SaveData的文件夹下
    public string DirctoryOfPlaySave
    { 
        get 
        {
            string path = Path.Combine("SaveData", "PlaySaveData");
            return FileUtility.GuaranteeDirectory(path);
        }
    }

    //编辑数据保存在名为SaveData的文件夹下
    public string DirctoryOfEditSave
    {
        get
        {
            string path = Path.Combine("SaveData", "EditSaveData");
            return FileUtility.GuaranteeDirectory(path);
        }
    }

    //==================================================================================
    //保存多个数据
    public void SaveDatas(List<DataItem> datas)
    {
        foreach(var d in datas)
        {
            SaveData(d.content, d.name);
        }
    }

    //读取多个数据
    public List<DataItem> LoadDatas()
    {
        if( FileUtility.ReadFiles(out List<DataItem> list, DirctoryOfPlaySave))
        {
            Debug.Log("Successfully loaded datas");
        }
        return list;
    }

    //保存数据
    public void SaveData(string dataToSave, string name)
    {
        if ( FileUtility.WriteToFile(name, dataToSave, DirctoryOfPlaySave))
        {
            Debug.Log("Successfully saved data");
        }
    }

    //读取数据
    public string LoadData(string name)
    {
        string data = "";
        if (FileUtility.ReadFromFile(name, out data, DirctoryOfPlaySave))
        {
            Debug.Log("Successfully loaded data");
        }
        return data;
    }

    //删除数据
    public bool DeleteData(string name)
    {
        return FileUtility.DeleteFile(name, DirctoryOfPlaySave);
    }

    //==================================================================================

    //保存多个数据
    public void SaveEditDatas(List<DataItem> datas)
    {
        foreach (var d in datas)
        {
            SaveEditData(d.content, d.name);
        }
    }

    //读取多个编辑数据
    public List<DataItem> LoadEditDatas()
    {
        if (FileUtility.ReadFiles(out List<DataItem> list, DirctoryOfEditSave))
        {
            Debug.Log("Successfully loaded datas");
        }
        return list;
    }

    //保存编辑数据
    public void SaveEditData(string dataToSave, string name)
    {
        if (FileUtility.WriteToFile(name, dataToSave, DirctoryOfEditSave))
        {
            Debug.Log("Successfully saved data");
        }
    }

    //读取编辑数据
    public string LoadEditData(string name)
    {
        string data = "";
        if ( FileUtility.ReadFromFile(name, out data, DirctoryOfEditSave))
        {
            Debug.Log("Successfully loaded data");
        }
        return data;
    }

    //删除数据
    public bool DeleteEditData(string name)
    {
        return FileUtility.DeleteFile(name, DirctoryOfEditSave);
    }

    //==================================================================================
}

public class DataItem
{
    public string name;
    public string content;
}