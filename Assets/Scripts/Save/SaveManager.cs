using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Singletons;
using Model;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public void SaveUserSettings()
    {
        SaveSystem.SaveData(new UserSettingsData(UserSettings.Instance), "userSettings.json");
    }
}
