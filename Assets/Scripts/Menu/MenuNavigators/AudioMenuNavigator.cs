using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class AudioMenuNavigator : MenuNavigatorBase
{
    [SerializeField] private SettingsMenu settingsMenu;

    protected override void Update()
    {
        base.Update();

        if (items[selectedItemIdx].GetComponentInChildren<Slider>())
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
                settingsMenu.IncreaseVolume(items[selectedItemIdx].GetComponentInChildren<Slider>());
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
                settingsMenu.DecreaseVolume(items[selectedItemIdx].GetComponentInChildren<Slider>());
        }
    }
}
