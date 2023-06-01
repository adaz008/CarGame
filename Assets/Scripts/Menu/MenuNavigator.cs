using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;
using static UnityEditor.Progress;
using UnityEngine.InputSystem.Android;
using static UnityEngine.Rendering.DebugUI;
using System.Drawing;
using Color = UnityEngine.Color;
using Model;
using static System.Runtime.CompilerServices.RuntimeHelpers;

public class MenuNavigator : MonoBehaviour
{

    [SerializeField] private GameObject[] items;
    private int selectedItemIdx = 0;
    private UIColorChanger colorChanger;


    [SerializeField] private GameObject ControlsMenuUi;
    [SerializeField] private GameObject PlayerMenuUi;
    [SerializeField] private SettingsMenu settingsMenu;

    [Header("Colors")]
    [SerializeField] private Color normalColor;
    [SerializeField] private Color hoverColor;
    [SerializeField] private Color activeInputFieldColor;
    [SerializeField] private Color normalBGColor;
    [SerializeField] private Color hoverBGColor;
    public static bool isEditing = false;

    void OnDisable()
    {
        foreach (GameObject item in items)
        {
            item.GetComponent<Button>()?.OnDeselect(new BaseEventData(EventSystem.current));
            item.GetComponent<Slider>()?.OnDeselect(new BaseEventData(EventSystem.current));
            item.GetComponent<Toggle>()?.OnDeselect(new BaseEventData(EventSystem.current));
            item.GetComponent<Selectable>()?.OnDeselect(new BaseEventData(EventSystem.current));
        }
    }
    void OnEnable()
    {
        selectedItemIdx = 0;
        Select(items[0]);
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && !isEditing)
        {
            selectedItemIdx = selectedItemIdx == 0 ? items.Length - 1 : selectedItemIdx - 1;
            Select(items[selectedItemIdx]);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && !isEditing)
        {
            selectedItemIdx = selectedItemIdx == items.Length - 1 ? 0 : selectedItemIdx + 1;
            Select(items[selectedItemIdx]);
        }

        if (items[selectedItemIdx].GetComponentInChildren<Slider>())
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
                settingsMenu.IncreaseVolume(items[selectedItemIdx].GetComponentInChildren<Slider>());
            else if(Input.GetKeyDown(KeyCode.LeftArrow))
                settingsMenu.DecreaseVolume(items[selectedItemIdx].GetComponentInChildren<Slider>());
        }

        if (PlayerMenuUi)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (items[selectedItemIdx].GetComponent<Toggle>())
                {
                    items[selectedItemIdx].GetComponent<Toggle>().isOn = true;
                    //settingsMenu.changeToggleValue(items[selectedItemIdx].GetComponent<Toggle>());
                }
                else
                    settingsMenu.PlayerSettingsChange(items[selectedItemIdx], 1);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (items[selectedItemIdx].GetComponent<Toggle>())
                {
                    items[selectedItemIdx].GetComponent<Toggle>().isOn = false;
                    //settingsMenu.changeToggleValue(items[selectedItemIdx].GetComponent<Toggle>());
                }
                else
                    settingsMenu.PlayerSettingsChange(items[selectedItemIdx], -1);
            }
        }


        //TransmissionTypeChange, CameraTypeChange, KeyboardInputTypeChange
        //if (settingsMenu && items[selectedItemIdx].GetComponentsInChildren<Button>().Length >= 2)
        //{
        //    Button[] buttons = items[selectedItemIdx].GetComponentsInChildren<Button>();

        //    int leftIndex = buttons[0].name.Contains("Left") ? 0 : 1;
        //    int rightIndex = buttons[0].name.Contains("Right") ? 0 : 1;

        //    // 0-Left, 1-Right
        //    if (Input.GetKeyDown(KeyCode.LeftArrow))
        //    {
        //        buttons[leftIndex].onClick.Invoke();
        //        SetButtonColor(buttons[leftIndex], hoverColor);
        //    }
        //    else if (Input.GetKeyDown(KeyCode.RightArrow))
        //    {
        //        buttons[rightIndex].onClick.Invoke();
        //        SetButtonColor(buttons[rightIndex], hoverColor);
        //    }
        //    else if (Input.GetKeyUp(KeyCode.LeftArrow))
        //        SetButtonColor(buttons[leftIndex], normalColor);
        //    else if (Input.GetKeyUp(KeyCode.RightArrow))
        //        SetButtonColor(buttons[rightIndex], normalColor);
        //}

        if (ControlsMenuUi) {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                TextMeshProUGUI[] texts = items[selectedItemIdx].GetComponentsInChildren<TextMeshProUGUI>();
                isEditing = true;
                StartCoroutine(OnSelect(texts[1]));
            }   
        }

    }

    private void SetButtonColor(Button button, Color hoverColor)
    {
        ColorBlock colors = button.colors;
        colors.normalColor = hoverColor;
        button.colors = colors;
    }

    public void Select(GameObject selectedItem)
    {
        int counter = 0;
        if (this.isActiveAndEnabled)
        {
            foreach (GameObject item in items)
            {
                if (item != selectedItem)
                {
                    if (item.GetComponent<Button>())
                    {
                        item.GetComponent<Button>()?.OnDeselect(new BaseEventData(EventSystem.current));
                        item.GetComponentInChildren<TextMeshProUGUI>().color = normalColor;
                    }
                    item.GetComponent<Slider>()?.OnDeselect(new BaseEventData(EventSystem.current));
                    item.GetComponent<Toggle>()?.OnDeselect(new BaseEventData(EventSystem.current));
                    if (item.GetComponent<Selectable>())
                    {
                        item.GetComponent<Selectable>()?.OnDeselect(new BaseEventData(EventSystem.current));
                        TextMeshProUGUI[] texts = item.GetComponentsInChildren<TextMeshProUGUI>();
                        foreach (TextMeshProUGUI text in texts)
                            text.color = normalColor;
                        if (normalBGColor.a != 0)
                            item.GetComponentInChildren<Image>().color = normalBGColor;
                    }
                }
                else
                {
                    if (item.GetComponent<Button>())
                    {
                        item.GetComponent<Button>()?.Select();
                        item.GetComponentInChildren<TextMeshProUGUI>().color = hoverColor;
                    }
                    item.GetComponent<Slider>()?.Select();
                    item.GetComponent<Toggle>()?.Select();
                    if (item.GetComponent<Selectable>())
                    {
                        item.GetComponent<Selectable>()?.Select();
                        TextMeshProUGUI[] texts = item.GetComponentsInChildren<TextMeshProUGUI>();
                        foreach (TextMeshProUGUI text in texts)
                            text.color = hoverColor;
                        if (hoverBGColor.a != 0)
                            item.GetComponentInChildren<Image>().color = hoverBGColor;
                    }

                    selectedItemIdx = counter;
                }
                counter++;
            }
        }
    }

    private IEnumerator OnSelect(TextMeshProUGUI text)
    {
        //Legyen egy beolvasás minimum az enter elõtt
        bool firstTime = true;
        text.fontStyle = FontStyles.Underline;
        text.color = activeInputFieldColor;
        KeyCode currentKeyCode = KeyCode.None;
        while (isEditing)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                isEditing = false;
                text.text = UserSettings.Instance.KeyCodesKeyboard[selectedItemIdx].ToString();
            }
            else if (Input.GetKeyDown(KeyCode.Return) && !firstTime)
            {
                isEditing = false;
                UserSettings.Instance.KeyCodesKeyboard[selectedItemIdx] = currentKeyCode;
            }
            else if (Input.anyKeyDown)
            {
                foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKeyDown(keyCode))
                    {
                        //Ne az entert írja ki mentéskor vagy megnyitáskor
                        if (keyCode != KeyCode.Return)
                        {
                            currentKeyCode = keyCode;
                            text.text = keyCode.ToString();
                            firstTime = false;
                            break;
                        }
                    }
                }
            }

            yield return null;
        }

        text.fontStyle = FontStyles.Bold;
        text.color = hoverColor;

        yield return null;
    }
}
