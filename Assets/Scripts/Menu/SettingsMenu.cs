using Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Slider = UnityEngine.UI.Slider;
using Toggle = UnityEngine.UI.Toggle;
using Image = UnityEngine.UI.Image;

public class SettingsMenu : MonoBehaviour
{
    //VolumeSettings
    public Slider[] sliders;

    [Header("Audio")]
    [SerializeField] private AudioSource UIClick;
    [SerializeField] private AudioSource UIHover;
    [SerializeField] private AudioSource UIClose;

    [Header("TypeTexts")]
    [SerializeField] private TextMeshProUGUI CameraType;
    [SerializeField] private TextMeshProUGUI TransmissionType;
    [SerializeField] private TextMeshProUGUI SpeedometerOnOff;
    [SerializeField] private TextMeshProUGUI CameraSwitchOnOff;
    [SerializeField] private TextMeshProUGUI MinimapOnOff;

    private int cameraTypeIndex = 0;
    [SerializeField] private Button[] changeCameraButton;

    private int transmissionTypeIndex = 0;
    [SerializeField] private Button[] changeTransmissionButton;

    [Header("Toggles")]
    [SerializeField] private Toggle[] toggles;

    [Header("InputFields")]
    [SerializeField] private TextMeshProUGUI[] InputFields;

    [Header("Prefabs")]
    [SerializeField] private GameObject timerPrefab;
    [SerializeField] private GameObject countDownPrefab;
    //[SerializeField] private GameObject trackPrefab;

    private string[] CameraTypes = { "Close", "Far", "Hood", "Bumper" };
    private string[] TransmissionTypes = { "Auto", "Manual" };
    private string[] ControlsTypes = { "Keyboard", "Controller" };

    [Header("StartRace")]
    [SerializeField] private Image startRace;
    [SerializeField] private Image endRace;
    [SerializeField] private Button PauseMenuBack;
    private bool isRace = false;

    private GameObject timerGameObject;
    private GameObject trackGameObject;
    private GameObject countDownGameObject;

    public void Start()
    {
        //Controls input fields
        for (int i = 0; i < InputFields.Length; i++)
            InputFields[i].text = UserSettings.Instance.KeyCodesKeyboard[i].ToString();

        //CarVolume-SoundEffectVolume-MenuMusicVolume-GameMusicVolume
        for (int i = 0; i < sliders.Length; i++)
        {
            Slider slider = sliders[i];

            slider.onValueChanged.AddListener((value) => { HandleSliderValueChange(value, slider); });

            if (slider.name.Contains("Car"))
                slider.value = UserSettings.Instance.CarVolume;
            if (slider.name.Contains("Sound"))
                slider.value = UserSettings.Instance.SoundEffectVolume;
            if (slider.name.Contains("Menu"))
                slider.value = UserSettings.Instance.MenuMusicVolume;
            if (slider.name.Contains("Game"))
                slider.value = UserSettings.Instance.GameMusicVolume;
        }

        //PlayerSettins
        //Transmission
        TransmissionType.text = UserSettings.Instance.Transmission;
        foreach (string type in TransmissionTypes)
        {
            if (type == UserSettings.Instance.Transmission)
                break;
            transmissionTypeIndex++;
        }
        //Camera
        CameraType.text = UserSettings.Instance.Camera;
        foreach (string type in CameraTypes)
        {
            if (type == UserSettings.Instance.Camera)
                break;
            cameraTypeIndex++;
        }

        //Speedometer-CameraSwitch-Minimap
        for (int i = 0; i < toggles.Length; i++)
        {
            if (i == 0)
            {
                toggles[i].isOn = UserSettings.Instance.Gauges;
                SpeedometerOnOff.text = toggles[i].isOn ? "ON" : "OFF";
            }
            else if (i == 1)
            {
                toggles[i].isOn = UserSettings.Instance.ChangeCameraReverse;
                CameraSwitchOnOff.text = toggles[i].isOn ? "ON" : "OFF";
            }
            else if (i == 2)
            {
                toggles[i].isOn = UserSettings.Instance.Minimap;
                MinimapOnOff.text = toggles[i].isOn ? "ON" : "OFF";
            }
        }
    }

    public void ChangeToggleValue(Toggle toggle)
    {
        if (toggle.name == "SpeedoMeter")
            SpeedometerOnOff.text = toggle.isOn ? "On" : "OFF";
        if (toggle.name == "CameraSwitch")
            CameraSwitchOnOff.text = toggle.isOn ? "On" : "OFF";
        if (toggle.name == "Minimap")
            MinimapOnOff.text = toggle.isOn ? "On" : "OFF";
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

    public void SavePlayerSettings()
    {
        //Gauges - ChangeCameraReverse - Minimap
        UserSettings.Instance.Gauges = toggles[0].isOn;
        UserSettings.Instance.ChangeCameraReverse = toggles[1].isOn;
        UserSettings.Instance.Minimap = toggles[2].isOn;

        UserSettings.Instance.Camera = CameraType.text;
        UserSettings.Instance.Transmission = TransmissionType.text;
    }

    public void PlayerSettingsChange(GameObject gameObject, int direction)
    {
        if (gameObject.name == "CameraSettings")
            changeCamera(direction);
        else
            changeTransmission();
    }


    private void changeTransmission()
    {
        transmissionTypeIndex = transmissionTypeIndex == 0 ? 1 : 0;
        TransmissionType.text = TransmissionTypes[transmissionTypeIndex];
    }

    private void changeCamera(int direction)
    {
        cameraTypeIndex = (cameraTypeIndex + direction + CameraTypes.Length) % CameraTypes.Length;
        CameraType.text = CameraTypes[cameraTypeIndex];
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

    public void ClickSound()
    {
        UIClick.volume = UserSettings.Instance.SoundEffectVolume;
        UIClick.Play();
    }
    public void HoverSound()
    {
        UIHover.volume = UserSettings.Instance.SoundEffectVolume * 2f;
        UIHover.Play();
    }

    public void CloseSound()
    {
        UIClose.volume = UserSettings.Instance.SoundEffectVolume;
        UIClose.Play();
    }

    public void StartRace(GameObject trackPrefab)
    {
        isRace = true;
        startRace.gameObject.SetActive(false);
        endRace.gameObject.SetActive(true);

        GameObject UIparent = GameObject.FindGameObjectWithTag("UI");
        GameObject Trackparent = GameObject.FindGameObjectWithTag("TrackParent");

        GameObject Car = GameObject.FindGameObjectWithTag("Car");

        timerGameObject = Instantiate(timerPrefab, Trackparent.transform);
        trackGameObject = Instantiate(trackPrefab);

        GameObject gridline = trackGameObject.GetComponentInChildren<Transform>().Find("prop_gridline").gameObject;

        countDownGameObject = Instantiate(countDownPrefab, Trackparent.transform);

        Car.GetComponent<CarMovement>().StartRacePos(gridline.transform.position);

        countDownGameObject.GetComponent<CountDownController>().StartRace();
    }

    public void EndRace()
    {
        isRace = false;
        startRace.gameObject.SetActive(true);
        endRace.gameObject.SetActive(false);

        Destroy(timerGameObject);
        Destroy(trackGameObject);
        Destroy(countDownGameObject);

        PauseMenuBack.onClick.Invoke();
    }

    public void RaceChangerSelected(GameObject raceSelectorUI)
    {
        if(isRace)
            EndRace();
        else
            raceSelectorUI.SetActive(true);    
    }
}
