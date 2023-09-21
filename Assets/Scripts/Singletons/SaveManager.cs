using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Singletons;
using Model;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public void SaveUserSettings()
    {
        SaveSystem.SaveData(UserSettings.Instance, "userSettings.json");
    }
}
