using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUIManager : UIManger
{
    [SerializeField] private MenuNavigatorBase settingsNavigator;
    [SerializeField] private GameObject AudioScreen;
    [SerializeField] private GameObject PlayerScreen;
    [SerializeField] private GameObject ControlsScreen;

    [SerializeField] private Selectable AudioSettings;
    [SerializeField] private Selectable PlayerSettings;
    [SerializeField] private Selectable  ControlsSettings;

    private Image audioArrow = null;
    private Image playerArrow = null;
    private Image controlsArrow = null;

    private void Awake()
    {
        audioArrow = GetImage("AudioArrow", AudioSettings.gameObject);
        playerArrow = GetImage("PlayerArrow", PlayerSettings.gameObject);
        controlsArrow = GetImage("ControlsArrow", ControlsSettings.gameObject);

    }

    private Image GetImage(string childName, GameObject settings)
    {
        Transform transform= settings.GetComponent<Transform>();

        Transform child = transform.GetChild(0);

        if (child != null)
        {
            return child.Find(childName).GetComponent<Image>();
        }

        return null;
    }


    private void Update()
    {
        if (!AudioScreen.activeInHierarchy && 
            !PlayerScreen.activeInHierarchy &&
            !ControlsScreen.activeInHierarchy)
        {
            settingsNavigator.enabled = true;
        }

    }

    public override void OpenUI(GameObject newUI)
    {
        InvokeUISound();
        disableAllScreen();

        newUI.SetActive(true);
        settingsNavigator.enabled = false;

        if (newUI.name.Contains("Audio"))
            enableArrow(audioArrow);
        else if (newUI.name.Contains("Player"))
            enableArrow(playerArrow);
        else if (newUI.name.Contains("Controls"))
            enableArrow(controlsArrow);

    }

    private void disableAllScreen()
    {
        AudioScreen.SetActive(false);   
        PlayerScreen.SetActive(false);
        ControlsScreen.SetActive(false);
    }

    private void enableArrow(Image arrow)
    {
        audioArrow.gameObject.SetActive(audioArrow == arrow);
        playerArrow.gameObject.SetActive(playerArrow == arrow);
        controlsArrow.gameObject.SetActive(controlsArrow == arrow);

    }
}
