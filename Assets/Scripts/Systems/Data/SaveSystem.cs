using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Utilities;

public class SaveSystem : PersistentSingleton<SaveSystem>
{

    //�������ݱ�������ΪSaveData���ļ�����
    public string DirctoryOfPlaySave
    { 
        get 
        {
            string path = Path.Combine("SaveData", "PlaySaveData");
            return FileUtility.GuaranteeDirectory(path);
        }
    }

    //�༭���ݱ�������ΪSaveData���ļ�����
    public string DirctoryOfEditSave
    {
        get
        {
            string path = Path.Combine("SaveData", "EditSaveData");
            return FileUtility.GuaranteeDirectory(path);
        }
    }

    //==================================================================================
    //����������
    public void SaveDatas(List<DataItem> datas)
    {
        foreach(var d in datas)
        {
            SaveData(d.content, d.name);
        }
    }

    //��ȡ�������
    public List<DataItem> LoadDatas()
    {
        if( FileUtility.ReadFiles(out List<DataItem> list, DirctoryOfPlaySave))
        {
            Debug.Log("Successfully loaded datas");
        }
        return list;
    }

    //��������
    public void SaveData(string dataToSave, string name)
    {
        if ( FileUtility.WriteToFile(name, dataToSave, DirctoryOfPlaySave))
        {
            Debug.Log("Successfully saved data");
        }
    }

    //��ȡ����
    public string LoadData(string name)
    {
        string data = "";
        if (FileUtility.ReadFromFile(name, out data, DirctoryOfPlaySave))
        {
            Debug.Log("Successfully loaded data");
        }
        return data;
    }

    //ɾ������
    public bool DeleteData(string name)
    {
        return FileUtility.DeleteFile(name, DirctoryOfPlaySave);
    }

    //==================================================================================

    //����������
    public void SaveEditDatas(List<DataItem> datas)
    {
        foreach (var d in datas)
        {
            SaveEditData(d.content, d.name);
        }
    }

    //��ȡ����༭����
    public List<DataItem> LoadEditDatas()
    {
        if (FileUtility.ReadFiles(out List<DataItem> list, DirctoryOfEditSave))
        {
            Debug.Log("Successfully loaded datas");
        }
        return list;
    }

    //����༭����
    public void SaveEditData(string dataToSave, string name)
    {
        if (FileUtility.WriteToFile(name, dataToSave, DirctoryOfEditSave))
        {
            Debug.Log("Successfully saved data");
        }
    }

    //��ȡ�༭����
    public string LoadEditData(string name)
    {
        string data = "";
        if ( FileUtility.ReadFromFile(name, out data, DirctoryOfEditSave))
        {
            Debug.Log("Successfully loaded data");
        }
        return data;
    }

    //ɾ������
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