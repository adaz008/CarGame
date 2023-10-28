using Assets.Scripts.Menu.MenuSettings;
using TMPro;
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

    public void OnDeactivate()
    {
        TextMeshProUGUI[] texts = items[selectedItemIdx].GetComponentsInChildren<TextMeshProUGUI>();
        texts[2].gameObject.SetActive(false);
    }
}
