using Assets.Scripts.Menu.MenuSettings;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMenuNavigator : MenuNavigatorBase
{
    [SerializeField] private PlayerMenu playerMenu;
    [SerializeField] private UIColorChanger colorChanger;

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (items[selectedItemIdx].GetComponent<Toggle>())
            {
                items[selectedItemIdx].GetComponent<Toggle>().isOn = true;
            }
            else
                playerMenu.PlayerSettingsChange(items[selectedItemIdx], 1);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (items[selectedItemIdx].GetComponent<Toggle>())
            {
                items[selectedItemIdx].GetComponent<Toggle>().isOn = false;
            }
            else
                playerMenu.PlayerSettingsChange(items[selectedItemIdx], -1);
        }
    }

    private void Awake()
    {
        foreach (GameObject item in items)
        {
            Toggle toggle = item.GetComponent<Toggle>();

            if (toggle != null)
            {
                toggle.onValueChanged.AddListener((value) => { SetIsToggleArrow(toggle); });
            }
        }
    }

    public void OnDeactivate()
    {
        TextMeshProUGUI[] texts = items[selectedItemIdx].GetComponentsInChildren<TextMeshProUGUI>();
        texts[2].gameObject.SetActive(false);

        Component component = items[selectedItemIdx].GetComponent<Component>();

        Transform child = component.transform.GetChild(0);

        if (child != null)
        {
            Image leftImage = child.Find("LeftArrow").GetComponent<Image>();
            Image rightImage = child.Find("RightArrow").GetComponent<Image>();

            colorChanger.OnPointerExitImage(leftImage);
            colorChanger.OnPointerExitImage(rightImage);
        }

    }

    private void SetIsToggleArrow(Toggle toggle)
    {
        Transform toggleTransform = toggle.transform;
        Transform child = toggleTransform.GetChild(0);

        if (child != null)
        {
            Image leftImage = child.Find("LeftArrow").GetComponent<Image>();
            Image rightImage = child.Find("RightArrow").GetComponent<Image>();

            leftImage.gameObject.SetActive(toggle.isOn);
            rightImage.gameObject.SetActive(!toggle.isOn);
        }
    }

}
