using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Menu.MenuSettings;
using UnityEngine;

public class RaceMenuUIManager : UIManger
{
    private RaceMenu menu;

    private void Start()
    {
        menu = GetComponent<RaceMenu>();
    }

    public void StartRace(GameObject RaceTrack)
    {
        base.InvokeUISound();

        this.gameObject.SetActive(false);

        menu.StartRace(RaceTrack);
    }
}
