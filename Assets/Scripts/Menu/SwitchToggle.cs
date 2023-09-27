using UnityEngine;
using UnityEngine.UI;

public class SwitchToggle : MonoBehaviour
{
    [SerializeField] Image leftArrow;
    [SerializeField] Image rightArrow;

    public void IsToggleChange(Toggle toggle)
    {
        Debug.Log("Switch");
        rightArrow.enabled = !toggle.isOn;
        leftArrow.enabled = toggle.isOn;
    }
}
