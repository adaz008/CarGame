using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Menu.MenuSettings;
using UnityEngine;
using UnityEngine.UI;

public class AudioMenuNavigator : MenuNavigatorBase
{
    [SerializeField] private AudioMenu audioMenu;

    protected override void Update()
    {
        base.Update();

        if (items[selectedItemIdx].GetComponentInChildren<Slider>())
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
                audioMenu.IncreaseVolume(items[selectedItemIdx].GetComponentInChildren<Slider>());
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
                audioMenu.DecreaseVolume(items[selectedItemIdx].GetComponentInChildren<Slider>());
        }
    }
}
