using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class AudioMenu : MonoBehaviour
{
    //VolumeSettings
    [Header("Audio sliders")]
    public Sliders sliders;

    private void Start()
    {
        InitializeSliders();
    }

    private void InitializeSliders()
    {
        sliders.CarSlider.value = UserSettings.Instance.CarVolume;
        sliders.UIEffectSlider.value = UserSettings.Instance.SoundEffectVolume;
        sliders.MenuMusicSlider.value = UserSettings.Instance.MenuMusicVolume;
        sliders.GameMusicSlider.value = UserSettings.Instance.GameMusicVolume;

        sliders.CarSlider.onValueChanged.AddListener((value) => { HandleSliderValueChange(value, sliders.CarSlider); });
        sliders.CarSlider.onValueChanged.Invoke(sliders.CarSlider.value);

        sliders.UIEffectSlider.onValueChanged.AddListener((value) => { HandleSliderValueChange(value, sliders.UIEffectSlider); });
        sliders.UIEffectSlider.onValueChanged.Invoke(sliders.UIEffectSlider.value);

        sliders.MenuMusicSlider.onValueChanged.AddListener((value) => { HandleSliderValueChange(value, sliders.MenuMusicSlider); });
        sliders.MenuMusicSlider.onValueChanged.Invoke(sliders.MenuMusicSlider.value);

        sliders.GameMusicSlider.onValueChanged.AddListener((value) => { HandleSliderValueChange(value, sliders.GameMusicSlider); });
        sliders.GameMusicSlider.onValueChanged.Invoke(sliders.GameMusicSlider.value);
    }

    public void SaveAudioSettings()
    {
        UserSettings.Instance.CarVolume = sliders.CarSlider.value;
        UserSettings.Instance.SoundEffectVolume = sliders.UIEffectSlider.value;
        UserSettings.Instance.MenuMusicVolume = sliders.MenuMusicSlider.value;
        UserSettings.Instance.GameMusicVolume = sliders.GameMusicSlider.value;
    }

    private void HandleSliderValueChange(float value, Slider slider)
    {
        slider.GetComponentInChildren<TextMeshProUGUI>().text = ((int)(value * 100)).ToString();
        SaveAudioSettings();
    }

    public void IncreaseVolume(Slider slider)
    {
        if (slider.value != 1)
            slider.value += 0.01f;
    }
    public void DecreaseVolume(Slider slider)
    {
        if (slider.value != 0)
            slider.value -= 0.01f;
    }
}

[System.Serializable]
public class Sliders
{
    public Slider CarSlider;
    public Slider UIEffectSlider;
    public Slider MenuMusicSlider;
    public Slider GameMusicSlider;
}
