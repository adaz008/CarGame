using TMPro;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioSource UIClick;
    [SerializeField] private AudioSource UIHover;
    [SerializeField] private AudioSource UIClose;

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

    public void ClickSound()
    {
        PlaySound(UIHover);
    }
    public void HoverSound()
    {
        PlaySound(UIHover);
    }

    public void CloseSound()
    {
        PlaySound(UIClose);    
    }

    private void PlaySound(AudioSource audio)
    {
        audio.volume = UserSettings.Instance.SoundEffectVolume;
        audio.Play();
    }
}
