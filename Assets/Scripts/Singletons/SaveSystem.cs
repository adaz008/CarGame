using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Assets.Scripts.Singletons;
using Model;

public static class SaveSystem
{
    public static void SaveUserSettings()
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/userSettings.json";
        FileStream stream = new FileStream(path, FileMode.Create);

        UserSettingsData data = new UserSettingsData(UserSettings.Instance);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static UserSettingsData LoadUserSettings()
    {
        string path = Application.persistentDataPath + "/userSettings.json";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            UserSettingsData data = formatter.Deserialize(stream) as UserSettingsData;

            stream.Close();

            return data;
        }
        else
        {
            Debug.Log("Save file not found in" + path);
            return null;
        }
    }
}
