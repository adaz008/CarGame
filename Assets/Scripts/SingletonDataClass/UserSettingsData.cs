using System;
using UnityEngine;

namespace Assets.Scripts.Singletons
{
    [Serializable]
    public class UserSettingsData
    {
        //Sounds
        public float SoundEffectVolume;
        public float CarVolume;
        public float MenuMusicVolume;
        public float GameMusicVolume;

        //Player settings
        public string Transmission;
        public string Camera;
        public bool Gauges;
        public bool Minimap;
        public bool ChangeCameraReverse;
        public bool RearViewMirror;

        //Keyboard settings
        public int Accelerate_Keyboard;
        public int Brake_Reverse_Keyboard;
        public int SteerRight_Keyboard;
        public int SteerLeft_Keyboard;
        public int Nitro_Keyboard;
        public int Handbrake_Keyboard;
        public int ShiftUp_Keyboard;
        public int ShiftDown_Keyboard;
        public int Reset_Keyboard;
        public int LookBack_Keyboard;
        public int ChangeCamera_Keyboard;

        public UserSettingsData(UserSettings userSettings)
        {
            SoundEffectVolume = userSettings.SoundEffectVolume;
            CarVolume = userSettings.CarVolume;
            MenuMusicVolume = userSettings.MenuMusicVolume;
            GameMusicVolume = userSettings.GameMusicVolume;

            Transmission = userSettings.Transmission;
            Camera = userSettings.Camera;
            Gauges = userSettings.Gauges;
            Minimap = userSettings.Minimap;
            ChangeCameraReverse = userSettings.ChangeCameraReverse;
            RearViewMirror = userSettings.RearViewMirror; 

            Accelerate_Keyboard = KeyCodeToInt(userSettings.Accelerate_Keyboard);
            Brake_Reverse_Keyboard = KeyCodeToInt(userSettings.Brake_Reverse_Keyboard);
            SteerRight_Keyboard = KeyCodeToInt(userSettings.SteerRight_Keyboard);
            SteerLeft_Keyboard = KeyCodeToInt(userSettings.SteerLeft_Keyboard);
            Nitro_Keyboard = KeyCodeToInt(userSettings.Nitro_Keyboard);
            Handbrake_Keyboard = KeyCodeToInt(userSettings.Handbrake_Keyboard);
            ShiftUp_Keyboard = KeyCodeToInt(userSettings.ShiftUp_Keyboard);
            ShiftDown_Keyboard = KeyCodeToInt(userSettings.ShiftDown_Keyboard);
            Reset_Keyboard = KeyCodeToInt(userSettings.Reset_Keyboard);
            LookBack_Keyboard = KeyCodeToInt(userSettings.LookBack_Keyboard);
            ChangeCamera_Keyboard = KeyCodeToInt(userSettings.ChangeCamera_Keyboard);
        }

        private int KeyCodeToInt(KeyCode key)
        {
            return (int)key;
        }
    }
}
