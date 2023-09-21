using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Slider = UnityEngine.UI.Slider;
using Toggle = UnityEngine.UI.Toggle;
using Image = UnityEngine.UI.Image;
using Model;
using Unity.VisualScripting;
using System.Diagnostics;

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
