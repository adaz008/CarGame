using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMenuNavigator : MenuNavigatorBase
{
    [SerializeField] private SettingsMenu settingsMenu;

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
                settingsMenu.PlayerSettingsChange(items[selectedItemIdx], 1);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (items[selectedItemIdx].GetComponent<Toggle>())
            {
                items[selectedItemIdx].GetComponent<Toggle>().isOn = false;
            }
            else
                settingsMenu.PlayerSettingsChange(items[selectedItemIdx], -1);
        }
    }
}
