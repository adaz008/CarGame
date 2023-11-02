using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Menu.MenuSettings
{
    public class PlayerMenu : MonoBehaviour
    {
        [Header("TypeTexts")]
        [SerializeField] private TextMeshProUGUI CameraType;
        [SerializeField] private TextMeshProUGUI TransmissionType;
        [SerializeField] private TextMeshProUGUI SpeedometerOnOff;
        [SerializeField] private TextMeshProUGUI CameraSwitchOnOff;
        [SerializeField] private TextMeshProUGUI MinimapOnOff;
        [SerializeField] private TextMeshProUGUI RearViewMirrorOnOff;

        private int cameraTypeIndex = 0;
        [SerializeField] private ChangeButtons changeCameraButtons;

        private int transmissionTypeIndex = 0;
        [SerializeField] private ChangeButtons changeTransmissionButtons;

        [Header("Toggles")]
        [SerializeField] private Toggles toggles;

        private string[] CameraTypes = { "Close", "Far", "Hood", "Bumper", "Inside"};
        private string[] TransmissionTypes = { "Auto", "Manual" };


        private void Start()
        {
            InitializePlayerSettings();
        }

        private void InitializePlayerSettings()
        {
            InitializeTransmissionSettings();
            InitializeCameraSettings();
            InitializeToggleSettings();
        }
        private void InitializeTransmissionSettings()
        {
            TransmissionType.text = UserSettings.Instance.Transmission;

            for (int i = 0; i < TransmissionTypes.Length; i++)
            {
                if (TransmissionTypes[i] == UserSettings.Instance.Transmission)
                {
                    transmissionTypeIndex = i;
                    break;
                }
            }
            changeTransmissionButtons.Left.onClick.AddListener(() => { changeTransmission(); });
            changeTransmissionButtons.Right.onClick.AddListener(() => { changeTransmission(); });
        }

        private void InitializeCameraSettings()
        {
            CameraType.text = UserSettings.Instance.Camera;

            for (int i = 0; i < CameraTypes.Length; i++)
            {
                if (CameraTypes[i] == UserSettings.Instance.Camera)
                {
                    cameraTypeIndex = i;
                    break;
                }
            }

            changeCameraButtons.Left.onClick.AddListener(() => { changeCamera(-1); });
            changeCameraButtons.Right.onClick.AddListener(() => {changeCamera(1);});
        }

        private void InitializeToggleSettings()
        {
            toggles.SpeedoMeter.onValueChanged.AddListener((value) => { ChangeToggleValue(toggles.SpeedoMeter); });
            toggles.CameraSwitch.onValueChanged.AddListener((value) => { ChangeToggleValue(toggles.CameraSwitch); });
            toggles.Minimap.onValueChanged.AddListener((value) => { ChangeToggleValue(toggles.Minimap); });
            toggles.RearView.onValueChanged.AddListener((value) => { ChangeToggleValue(toggles.RearView); });

            //Trick toggle onValueChanged
            toggles.SpeedoMeter.isOn = !UserSettings.Instance.Gauges;
            toggles.CameraSwitch.isOn = !UserSettings.Instance.ChangeCameraReverse;
            toggles.Minimap.isOn = !UserSettings.Instance.Minimap;
            toggles.RearView.isOn = !UserSettings.Instance.RearViewMirror;
            //Real toggle onValueChanged
            toggles.SpeedoMeter.isOn = UserSettings.Instance.Gauges;
            toggles.CameraSwitch.isOn = UserSettings.Instance.ChangeCameraReverse;
            toggles.Minimap.isOn = UserSettings.Instance.Minimap;
            toggles.RearView.isOn = UserSettings.Instance.RearViewMirror;
        }

        public void ChangeToggleValue(Toggle toggle)
        {
            if (toggle.name == "SpeedoMeter")
                SpeedometerOnOff.text = toggle.isOn ? "On" : "Off";
            if (toggle.name == "CameraSwitch")
                CameraSwitchOnOff.text = toggle.isOn ? "On" : "Off";
            if (toggle.name == "Minimap")
                MinimapOnOff.text = toggle.isOn ? "On" : "Off";
            if (toggle.name == "RearView")
                RearViewMirrorOnOff.text = toggle.isOn ? "On" : "Off";
        }

        public void SavePlayerSettings()
        {
            //Gauges - ChangeCameraReverse - Minimap
            UserSettings.Instance.Gauges = toggles.SpeedoMeter.isOn;
            UserSettings.Instance.ChangeCameraReverse = toggles.CameraSwitch.isOn;
            UserSettings.Instance.Minimap = toggles.Minimap.isOn;
            UserSettings.Instance.RearViewMirror = toggles.RearView.isOn;

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
    }

    [System.Serializable]
    public class ChangeButtons
    {
        public Button Left;
        public Button Right;
    }

    [System.Serializable]
    public class Toggles
    {
        public Toggle SpeedoMeter;
        public Toggle CameraSwitch;
        public Toggle Minimap;
        public Toggle RearView;
    }
}
