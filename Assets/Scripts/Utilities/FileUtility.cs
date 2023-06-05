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

        //��ȡ�����ļ�
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

        //ɾ������
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


        //ȷ���ļ���·�����ڣ�·��path��Ӧ�ñ���·������·��
        public static string GuaranteeDirectory(string path)
        {
            string directory = Path.Combine(Application.persistentDataPath, path);
            if (!Directory.Exists(directory))//���·��������
            {
                Directory.CreateDirectory(directory);//����һ��·�����ļ���
            }
            return directory;
        }
    }
}

