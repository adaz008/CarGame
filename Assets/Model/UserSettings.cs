using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Model
{
    public class UserSettings : MonoBehaviour
    {
        public static UserSettings Instance { get; private set;}

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
                Destroy(gameObject);

            Transmission = "Auto";
            Camera = "Close";
            ChangeCameraReverse = false;
            CarVolume =  0.1f;
            SoundEffectVolume =0.5f;
            MenuMusicVolume =0.5f;
            GameMusicVolume =0.5f;
            Gauges = true;
            Minimap = true;

            Accelerate_Keyboard = KeyCode.UpArrow;
            Brake_Reverse_Keyboard = KeyCode.DownArrow;
            SteerRight_Keyboard = KeyCode.RightArrow;
            SteerLeft_Keyboard = KeyCode.LeftArrow;
            Nitro_Keyboard = KeyCode.RightShift;
            Handbrake_Keyboard = KeyCode.Space;
            ShiftUp_Keyboard = KeyCode.LeftShift;
            ShiftDown_Keyboard = KeyCode.LeftControl;
            Reset_Keyboard = KeyCode.R;
            LookBack_Keyboard = KeyCode.L;
            ChangeCamera_Keyboard = KeyCode.C;
        }

        //Sound
       [SerializeField] public float SoundEffectVolume { get; set; }
       [SerializeField] public float CarVolume { get; set; }
       [SerializeField] public float MenuMusicVolume { get; set; }
       [SerializeField] public float GameMusicVolume { get; set; }

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
