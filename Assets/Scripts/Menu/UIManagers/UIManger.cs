using System;
using Assets.Scripts.Menu.MenuSettings;
using UnityEngine;

public class UIManger : MonoBehaviour
{
    public static event Action OnOpenUI;
    [SerializeField] protected RaceMenu menu;
    public virtual void OpenUI(GameObject newUI)
    {
        InvokeUISound();
        this.gameObject.SetActive(false);
        newUI.SetActive(true);
    }

    protected void InvokeUISound()
    {
        OnOpenUI?.Invoke();
    }

    public void OpenRaceUI(GameObject racemenuUI)
    {
        InvokeUISound();
        this.gameObject.SetActive(false);
        menu.RaceChangerSelected(racemenuUI);
    }
}
