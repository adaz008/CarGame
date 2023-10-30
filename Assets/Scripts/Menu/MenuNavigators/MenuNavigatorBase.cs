using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;
using Color = UnityEngine.Color;

public class MenuNavigatorBase : MonoBehaviour
{
    [SerializeField] protected GameObject[] items;
    protected int selectedItemIdx = 0;

    [Header("Colors")]
    [SerializeField] protected Color normalColor;
    [SerializeField] protected Color hoverColor;
    [SerializeField] protected Color activeInputFieldColor;
    [SerializeField] protected Color normalBGColor;
    [SerializeField] protected Color hoverBGColor;

    public static event Action Hover;


    void OnDisable()
    {
        foreach (GameObject item in items)
        {
            item.GetComponent<Button>()?.OnDeselect(new BaseEventData(EventSystem.current));
            item.GetComponent<Slider>()?.OnDeselect(new BaseEventData(EventSystem.current));
            item.GetComponent<Toggle>()?.OnDeselect(new BaseEventData(EventSystem.current));
            item.GetComponent<Selectable>()?.OnDeselect(new BaseEventData(EventSystem.current));
        }
    }
    void OnEnable()
    {
        selectedItemIdx = 0;
        Select(items[0]);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                selectedItemIdx = selectedItemIdx == 0 ? items.Length - 1 : selectedItemIdx - 1;
                Select(items[selectedItemIdx]);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                selectedItemIdx = selectedItemIdx == items.Length - 1 ? 0 : selectedItemIdx + 1;
                Select(items[selectedItemIdx]);
            }
    }

    public void Select(GameObject selectedItem)
    {
        int counter = 0;
        if (this.isActiveAndEnabled)
        {
            Hover?.Invoke();
            foreach (GameObject item in items)
            {
                if (item != selectedItem)
                {
                    if (item.GetComponent<Button>())
                    {
                        item.GetComponent<Button>()?.OnDeselect(new BaseEventData(EventSystem.current));
                        item.GetComponentInChildren<TextMeshProUGUI>().color = normalColor;
                    }
                    item.GetComponent<Slider>()?.OnDeselect(new BaseEventData(EventSystem.current));
                    item.GetComponent<Toggle>()?.OnDeselect(new BaseEventData(EventSystem.current));
                    if (item.GetComponent<Selectable>())
                    {
                        item.GetComponent<Selectable>()?.OnDeselect(new BaseEventData(EventSystem.current));
                        TextMeshProUGUI[] texts = item.GetComponentsInChildren<TextMeshProUGUI>();
                        foreach (TextMeshProUGUI text in texts)
                            text.color = normalColor;
                        if (normalBGColor.a != 0)
                            item.GetComponentInChildren<Image>().color = normalBGColor;
                    }
                }
                else
                {
                    if (item.GetComponent<Button>())
                    {
                        item.GetComponent<Button>()?.Select();
                        item.GetComponentInChildren<TextMeshProUGUI>().color = hoverColor;
                    }
                    item.GetComponent<Slider>()?.Select();
                    item.GetComponent<Toggle>()?.Select();
                    if (item.GetComponent<Selectable>())
                    {
                        item.GetComponent<Selectable>()?.Select();
                        TextMeshProUGUI[] texts = item.GetComponentsInChildren<TextMeshProUGUI>();
                        foreach (TextMeshProUGUI text in texts)
                            text.color = hoverColor;
                        if (hoverBGColor.a != 0)
                            item.GetComponentInChildren<Image>().color = hoverBGColor;
                    }

                    selectedItemIdx = counter;
                }
                counter++;
            }
        }
    }

    protected virtual void OnDeactivate()
    {
        TextMeshProUGUI[] texts = items[selectedItemIdx].GetComponentsInChildren<TextMeshProUGUI>();
        texts[2].gameObject.SetActive(false);
    }
}
