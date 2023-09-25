using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Assets.Scripts.Singletons;
using Model;

public static class SaveSystem
{
    public static void SaveData<T>(T data, string fileName)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + fileName;

        using (FileStream stream = new FileStream(path, FileMode.Create))
        {
            formatter.Serialize(stream, data);
        }
    }


    public static T LoadData<T>(string fileName)
    {
        string path = Application.persistentDataPath + "/" + fileName;
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                T data = (T)formatter.Deserialize(stream);
                return data;
            }
        }
        else
        {
            Debug.Log("Save file not found in " + path);
            return default(T);
        }
    }

}
