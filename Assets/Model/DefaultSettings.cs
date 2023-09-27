using UnityEngine;

namespace Assets.Model
{
    public class DefaultSettings : MonoBehaviour
    {
        public static DefaultSettings Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
                Destroy(gameObject);
        }

        //Sound
        public float SoundEffectVolume { get { return 0.5f; } }
        public float CarVolume { get { return 0.5f; } }
        public float MenuMusicVolume { get { return 0.5f; } }
        public float GameMusicVolume { get { return 0.5f; } }

        //Player
        public string Transmission { get; set; } //auto-manual
        public string Camera { get; set; } //far-close-bumper-hood
        public bool Gauges { get; set; }
        public bool Minimap { get; set; } //On-Off
        public bool ChangeCameraReverse { get; set; } //On-off, ötlet

        //Control-Keyboard
        public string Accelerate_Keyboard { get; set; }
        public string Brake_Reverse_Keyboard { get; set; }
        public string SteerRight_Keyboard { get; set; }
        public string SteerLeft_Keyboard { get; set; }
        public string Nitro_Keyboard { get; set; }
        public string Handbrake_Keyboard { get; set; }
        public string ShiftUp_Keyboard { get; set; }
        public string ShiftDown_Keyboard { get; set; }
        public string Reset_Keyboard { get; set; }
        public string LookBack_Keyboard { get; set; }
        public string ChangeCamera_Keyboard { get; set; }

        //Control-Controller
        public string Accelerate_Controller { get; set; }
        public string Brake_Reverse_Controller { get; set; }
        public string SteerRight_Controller { get; set; }
        public string SteerLeft_Controller { get; set; }
        public string Nitro_Controller { get; set; }
        public string Handbrake_Controller { get; set; }
        public string ShiftUp_Controller { get; set; }
        public string ShiftDown_Controller { get; set; }
        public string Reset_Controller { get; set; }
        public string LookBack_Controller { get; set; }
        public string ChangeCamera_Controller { get; set; }
    }
}
