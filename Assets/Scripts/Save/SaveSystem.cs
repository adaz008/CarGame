using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveDataToBinary<T>(T data, string fileName)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + fileName;

        using (FileStream stream = new FileStream(path, FileMode.Create))
        {
            formatter.Serialize(stream, data);
        }
    }


    public static T LoadDataToBinary<T>(string fileName)
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

	public static void SaveDataToJson<T>(T data, string fileName)
	{
		string json = JsonUtility.ToJson(data);
		string path = Application.persistentDataPath + "/" + fileName;
		System.IO.File.WriteAllText(path, json);
	}

	public static T LoadDataFromJson<T>(string fileName)
	{
		string path = Application.persistentDataPath + "/" + fileName;
		if (System.IO.File.Exists(path))
		{
			string json = System.IO.File.ReadAllText(path);
			return JsonUtility.FromJson<T>(json);
		}
		else
		{
			Debug.Log("Save file not found in " + path);
			return default(T);
		}
	}

}
