using TMPro;
using UnityEngine;

public class ControlsMenu : MonoBehaviour
{
    [Header("InputFields")]
    [SerializeField] private TextMeshProUGUI[] InputFields;

    public void Start()
    {
        InitializeInputFields();
    }

    private void InitializeInputFields()
    {
        for (int i = 0; i < InputFields.Length; i++)
            InputFields[i].text = UserSettings.Instance.KeyCodesKeyboard[i].ToString();
    }

    public void setKeyValue(int selectedIndex, KeyCode value)
    {
		UserSettings.Instance.KeyCodesKeyboard[selectedIndex] = value;
	}

}
