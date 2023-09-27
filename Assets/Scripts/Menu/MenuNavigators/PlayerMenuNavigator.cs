using Assets.Scripts.Menu.MenuSettings;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMenuNavigator : MenuNavigatorBase
{
    [SerializeField] private PlayerMenu playerMenu;

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
    private void SetIsToggleArrow(Toggle toggle)
    {
        Debug.Log("Set toggle Arrow for:" + toggle.name);
        Debug.Log("Value:" + toggle.isOn);

        Transform toggleTransform = toggle.transform;
        Transform child = toggleTransform.GetChild(0);

        for (int i = 0; i < child.childCount; i++)
        {
            Transform grandChildTransform = child.GetChild(i);
            Image imageComponent = grandChildTransform.GetComponent<Image>();

            if (imageComponent != null)
            {
                if (imageComponent.name.Contains("Left"))
                {
                    imageComponent.gameObject.SetActive(toggle.isOn);
                }
                else if (imageComponent.name.Contains("Right"))
                {
                    imageComponent.gameObject.SetActive(!toggle.isOn);
                }
            }
        }
    }

}
