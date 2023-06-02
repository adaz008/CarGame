using System;
using System.Collections;
using System.Collections.Generic;
using Model;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    [Header("Menu UI panels")]
    [SerializeField] private GameObject pauseMenuUi;
    [SerializeField] private GameObject SettingsMenuUi;
    [SerializeField] private GameObject AudioMenuUi;
    [SerializeField] private GameObject PlayerMenuUi;
    [SerializeField] private GameObject ControlsMenuUi;
    [SerializeField] private GameObject RaceMenuUi;

    [Header("Other UI elements")]
    [SerializeField] private GameObject blurUI;
    [SerializeField] private GameObject SpeedoMeter;
    [SerializeField] private GameObject Minimap;

    [Header("Back Buttons")]
    [SerializeField] private Button SettingsBack;
    [SerializeField] private Button AudioBack;
    [SerializeField] private Button PlayerBack;
    [SerializeField] private Button ControlsBack;
    [SerializeField] private Button RaceBack;

    [Header("Selects")]
    [SerializeField] private GameObject enter;
    [SerializeField] private GameObject esc;

    [Header("SettingsMenuNavigator")]
    [SerializeField] private GameObject SettingsMenuNavigator;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
                if (pauseMenuUi.activeInHierarchy)
                    Resume();
                else
                    MoveBack();
            else
                Pause();
        }
    }

    private void MoveBack()
    {
        if (AudioMenuUi.activeInHierarchy && !MenuNavigator.isEditing)
            AudioBack.onClick.Invoke();
        else if (PlayerMenuUi.activeInHierarchy && !MenuNavigator.isEditing)
            PlayerBack.onClick.Invoke();
        else if (ControlsMenuUi.activeInHierarchy && !MenuNavigator.isEditing)
            ControlsBack.onClick.Invoke();
        else if (RaceMenuUi.activeInHierarchy && !MenuNavigator.isEditing)
            RaceBack.onClick.Invoke();
        else if (SettingsMenuUi.activeInHierarchy && !MenuNavigator.isEditing)
            SettingsBack.onClick.Invoke();
    }

    public void Resume()
    {
        pauseMenuUi.SetActive(false);
        
        enter.SetActive(false);
        esc.SetActive(false);

        blurUI.SetActive(false);
        Minimap.SetActive(UserSettings.Instance.Minimap);
        SpeedoMeter.SetActive(UserSettings.Instance.Gauges);

       Time.timeScale = 1f;

       GameIsPaused = false;
    }

    private void Pause()
    {
        pauseMenuUi.SetActive(true);

        enter.SetActive(true);
        esc.SetActive(true);

        blurUI.SetActive(true);
        Minimap.SetActive(false);
        SpeedoMeter.SetActive(false);

        Time.timeScale = 0f;

        GameIsPaused = true;
    }
}
