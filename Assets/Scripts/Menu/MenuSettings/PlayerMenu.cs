﻿using System;
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
        [SerializeField] private Button[] changeCameraButton;

        private int transmissionTypeIndex = 0;
        [SerializeField] private Button[] changeTransmissionButton;

        [Header("Toggles")]
        [SerializeField] private Toggle[] toggles;

        private string[] CameraTypes = { "Close", "Far", "Hood", "Bumper", "Inside"};
        private string[] TransmissionTypes = { "Auto", "Manual" };
        private string[] ControlsTypes = { "Keyboard", "Controller" };

        public void Start()
        {
            InitializePlayerSettings();
        }

        private void OnEnable()
        {
            InitializeCameraSettings();
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
        }

        private void InitializeToggleSettings()
        {
            foreach (Toggle toggle in toggles)
            {
                toggle.onValueChanged.AddListener((value) => { ChangeToggleValue(toggle); });
            }

            //Trick toggle onValueChanged
            toggles[0].isOn = !UserSettings.Instance.Gauges;
            toggles[1].isOn = !UserSettings.Instance.ChangeCameraReverse;
            toggles[2].isOn = !UserSettings.Instance.Minimap;
            toggles[3].isOn = !UserSettings.Instance.RearViewMirror;
            //Real toggle onValueChanged
            toggles[0].isOn = UserSettings.Instance.Gauges;
            toggles[1].isOn = UserSettings.Instance.ChangeCameraReverse;
            toggles[2].isOn = UserSettings.Instance.Minimap;
            toggles[3].isOn = UserSettings.Instance.RearViewMirror;
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
            UserSettings.Instance.Gauges = toggles[0].isOn;
            UserSettings.Instance.ChangeCameraReverse = toggles[1].isOn;
            UserSettings.Instance.Minimap = toggles[2].isOn;
            UserSettings.Instance.RearViewMirror = toggles[3].isOn;

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
}
