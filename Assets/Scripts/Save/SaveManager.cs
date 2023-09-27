using Assets.Scripts.Singletons;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public void SaveUserSettings()
    {
        SaveSystem.SaveData(new UserSettingsData(UserSettings.Instance), "userSettings.json");
    }
}
