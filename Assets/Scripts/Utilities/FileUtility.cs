using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Utilities
{
    public static class FileUtility
    {
        public static bool WriteToFile(string name, string content, string directPath)
        {
            var fullPath = Path.Combine(directPath, name);
            try
            {
                File.WriteAllText(fullPath, content);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError("Error saving to a file " + e.Message);
            }
            return false;
        }

        public static bool ReadFromFile(string name, out string content, string directPath)
        {
            var fullPath = Path.Combine(directPath, name);
            try
            {
                content = File.ReadAllText(fullPath);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError("Error when loading the file " + e.Message);
                content = "";
            }
            return false;
        }

        //读取所有文件
        public static bool ReadFiles(out List<DataItem> contents, string directPath)
        {
            contents = new List<DataItem>();
            DirectoryInfo root = new DirectoryInfo(directPath);
            FileInfo[] files = root.GetFiles();
            try
            {
                for (int i = 0; i < files.Length; i++)
                {
                    string content = File.ReadAllText(files[i].FullName);
                    contents.Add(new DataItem() { name = files[i].Name, content = content });
                }
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError("Error when loading the file " + e.Message);
            }

            return false;
        }

        //删除数据
        public static bool DeleteFile(string name, string directPath)
        {
            var fullPath = Path.Combine(directPath, name);
            try
            {
                File.Delete(fullPath);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError("Error when deleting the file " + e.Message);
            }
            return false;
        }


        //确保文件夹路径存在，路径path是应用保存路径的子路径
        public static string GuaranteeDirectory(string path)
        {
            string directory = Path.Combine(Application.persistentDataPath, path);
            if (!Directory.Exists(directory))//如果路径不存在
            {
                Directory.CreateDirectory(directory);//创建一个路径的文件夹
            }
            return directory;
        }
    }
}

