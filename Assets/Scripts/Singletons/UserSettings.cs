using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Singletons;
using Unity.VisualScripting;
using UnityEngine;

namespace Model
{
    public class UserSettings : MonoBehaviour
    {
        public static UserSettings Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
                Destroy(gameObject);

            UserSettingsData data = SaveSystem.LoadUserSettings();

            CarVolume = data?.CarVolume ?? 0.5f;
            SoundEffectVolume = data?.SoundEffectVolume ?? 0.5f;
            MenuMusicVolume = data?.MenuMusicVolume ?? 0.5f;
            GameMusicVolume = data?.GameMusicVolume ?? 0.5f;

            Transmission = data == null ? "Auto" : data.Transmission;
            Camera = data == null ? "Close" : data.Camera;
            ChangeCameraReverse = data == null ? false : data.ChangeCameraReverse;
            Gauges = data == null ? true : data.Gauges;
            Minimap = data == null ? true : data.Minimap;

            Accelerate_Keyboard = data == null ? KeyCode.UpArrow : IntToKeyCode(data.Accelerate_Keyboard);
            Brake_Reverse_Keyboard = data == null ? KeyCode.DownArrow : IntToKeyCode(data.Brake_Reverse_Keyboard);
            SteerRight_Keyboard = data == null ? KeyCode.RightArrow : IntToKeyCode(data.SteerRight_Keyboard);
            SteerLeft_Keyboard = data == null ? KeyCode.LeftArrow : IntToKeyCode(data.SteerLeft_Keyboard);
            Nitro_Keyboard = data == null ? KeyCode.RightShift : IntToKeyCode(data.Nitro_Keyboard);
            Handbrake_Keyboard = data == null ? KeyCode.Space : IntToKeyCode(data.Handbrake_Keyboard);
            ShiftUp_Keyboard = data == null ? KeyCode.LeftShift : IntToKeyCode(data.ShiftUp_Keyboard);
            ShiftDown_Keyboard = data == null ? KeyCode.LeftControl : IntToKeyCode(data.ShiftDown_Keyboard);
            Reset_Keyboard = data == null ? KeyCode.R : IntToKeyCode(data.Reset_Keyboard);
            LookBack_Keyboard = data == null ? KeyCode.L : IntToKeyCode(data.LookBack_Keyboard);
            ChangeCamera_Keyboard = data == null ? KeyCode.C : IntToKeyCode(data.ChangeCamera_Keyboard);
        }

        // Converts an integer to a KeyCode
        private KeyCode IntToKeyCode(int value)
        {
            return (KeyCode)value;
        }

        //Sound
        public float SoundEffectVolume { get; set; }
        public float CarVolume { get; set; }
        public float MenuMusicVolume { get; set; }
        public float GameMusicVolume { get; set; }

        //Player
        public string Transmission { get; set; } //auto-manual
        public string Camera { get; set; } //far-close-bumper-hood
        public bool Gauges { get; set; }
        public bool Minimap { get; set; } //On-Off
        public bool ChangeCameraReverse { get; set; } //On-off, ötlet

        //Control-Keyboard
        private KeyCode[] keyCodesKeyboard = new KeyCode[11];

        public KeyCode Accelerate_Keyboard { get => keyCodesKeyboard[0]; set => keyCodesKeyboard[0] = value; }
        public KeyCode Brake_Reverse_Keyboard { get => keyCodesKeyboard[1]; set => keyCodesKeyboard[1] = value; }
        public KeyCode SteerRight_Keyboard { get => keyCodesKeyboard[2]; set => keyCodesKeyboard[2] = value; }
        public KeyCode SteerLeft_Keyboard { get => keyCodesKeyboard[3]; set => keyCodesKeyboard[3] = value; }
        public KeyCode Nitro_Keyboard { get => keyCodesKeyboard[4]; set => keyCodesKeyboard[4] = value; }
        public KeyCode Handbrake_Keyboard { get => keyCodesKeyboard[5]; set => keyCodesKeyboard[5] = value; }
        public KeyCode ShiftUp_Keyboard { get => keyCodesKeyboard[6]; set => keyCodesKeyboard[6] = value; }
        public KeyCode ShiftDown_Keyboard { get => keyCodesKeyboard[7]; set => keyCodesKeyboard[7] = value; }
        public KeyCode Reset_Keyboard { get => keyCodesKeyboard[8]; set => keyCodesKeyboard[8] = value; }
        public KeyCode LookBack_Keyboard { get => keyCodesKeyboard[9]; set => keyCodesKeyboard[9] = value; }
        public KeyCode ChangeCamera_Keyboard { get => keyCodesKeyboard[10]; set => keyCodesKeyboard[10] = value; }

        public KeyCode[] KeyCodesKeyboard { get => keyCodesKeyboard; }
    }
}
