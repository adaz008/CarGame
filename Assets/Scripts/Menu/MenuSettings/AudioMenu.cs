using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Menu.MenuSettings
{
    public class AudioMenu : MonoBehaviour
    {
        //VolumeSettings
        [Header("Audio sliders")]
        public Slider[] sliders;

        public void Start()
        {
            InitializeSliders();
        }

        private void InitializeSliders()
        {
            //CarVolume-SoundEffectVolume-MenuMusicVolume-GameMusicVolume
            foreach (Slider slider in sliders)
            {
                if (slider.name.Contains("Car"))
                    slider.value = UserSettings.Instance.CarVolume;
                if (slider.name.Contains("Sound"))
                    slider.value = UserSettings.Instance.SoundEffectVolume;
                if (slider.name.Contains("Menu"))
                    slider.value = UserSettings.Instance.MenuMusicVolume;
                if (slider.name.Contains("Game"))
                    slider.value = UserSettings.Instance.GameMusicVolume;
            }
            foreach (Slider slider in sliders)
            {
                slider.onValueChanged.AddListener((value) => { HandleSliderValueChange(value, slider); });
                slider.onValueChanged.Invoke(slider.value);
            }
        }

        public void SaveAudioSettings()
        {
            foreach (Slider slider in sliders)
            {
                if (slider.name == "CarSlider")
                    UserSettings.Instance.CarVolume = slider.value;
                if (slider.name == "SoundEffectSlider")
                    UserSettings.Instance.SoundEffectVolume = slider.value;
                if (slider.name == "MenuMusicSlider")
                    UserSettings.Instance.MenuMusicVolume = slider.value;
                if (slider.name == "GameMusicSlider")
                    UserSettings.Instance.GameMusicVolume = slider.value;
            }
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
}
