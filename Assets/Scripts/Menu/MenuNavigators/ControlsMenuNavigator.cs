using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class ControlsMenuNavigator : MenuNavigatorBase
{
    public static event Action OnControlsEditing;
    public static bool isEditing = false;

    protected override void Update()
    {
        if (!isEditing)
            base.Update();

        if (Input.GetKeyDown(KeyCode.Return))
        {
            TextMeshProUGUI[] texts = items[selectedItemIdx].GetComponentsInChildren<TextMeshProUGUI>();
            isEditing = true;
            OnControlsEditing?.Invoke();
            StartCoroutine(OnSelect(texts[1]));
        }
    }

    private IEnumerator OnSelect(TextMeshProUGUI text)
    {
        //Legyen egy beolvasás minimum az enter előtt
        bool firstTime = true;
        text.fontStyle = FontStyles.Underline;
        text.color = activeInputFieldColor;
        KeyCode currentKeyCode = KeyCode.None;
        while (isEditing)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Debug.Log("IsEditing false");
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
