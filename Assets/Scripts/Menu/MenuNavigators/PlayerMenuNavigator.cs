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
}
