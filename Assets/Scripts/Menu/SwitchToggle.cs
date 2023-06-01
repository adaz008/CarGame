using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SwitchToggle : MonoBehaviour
{
    public RectTransform uiHandle;
    public Color backgroundActiveColor;
    public Color handleActiveColor;

    Image handleImage, backgroundImage;

    Color handleDefaultColor, backgroundDefaultColor;

    Toggle toggle;

    Vector2 handlePos;

    public void Awake()
    {
        toggle = GetComponent<Toggle>();
        handlePos = uiHandle.anchoredPosition;

        backgroundImage = uiHandle.parent.GetComponent<Image>();
        handleImage = uiHandle.GetComponent<Image>();

        backgroundDefaultColor = backgroundImage.color;
        handleDefaultColor = handleImage.color;

        if (toggle.isOn)
            OnSwitch(true);
    }

    public void OnSwitch(bool On)
    {
        uiHandle.anchoredPosition = On ? handlePos * -1 : handlePos;

        backgroundImage.color = On ? backgroundActiveColor : backgroundDefaultColor;
        handleImage.color = On ? handleActiveColor : handleDefaultColor;
    }
}
