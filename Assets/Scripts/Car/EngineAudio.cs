using System.Collections;
using UnityEngine;

public class EngineAudio : MonoBehaviour
{
    [SerializeField] private AudioSources Audios;

    private float maxVolume;

    private CarMovement carMovement;
    private Motor motor;
    private float RPM;
    private float redline;
    private float gasInput;

    private bool isEngineRunning = false;

    // Start is called before the first frame update
    void Start()
    {
        carMovement = GetComponent<CarMovement>();
        motor = GetComponent<Motor>();
        maxVolume = UserSettings.Instance.CarVolume;
        redline = (float)motor.RedLineStart;
        MuteAll();
    }

    // Update is called once per frame
    void Update()
    {
        if (carMovement)
        {
            RPM = (float)motor.GetRPM();
            gasInput = (float)carMovement.GasInput;
        }

        maxVolume = UserSettings.Instance.CarVolume;

        if (PauseMenu.GameIsPaused)
            MuteAll();
        else
        {
            if (isEngineRunning)
            {
                MuteAll();
                if (RPM <= 1000)
                {
                    //Ha 500 alatti a fordulatszám akkor legyen 600, így realisztikusabb a hang
                    RPM = RPM < 500 ? 600 : RPM;
                    Audios.Idle.volume = Mathf.Lerp(0, 1, RPM / 1000f) * maxVolume;
                }
                else if (1000 < RPM && RPM <= 3000 && Mathf.Abs(gasInput) > 0.1f)
                {
                    Audios.LowOn.volume = Mathf.Lerp(0.3f, 0.7f, RPM / 3000f) * maxVolume;
                    Audios.LowOn.pitch = Mathf.Lerp(0.7f, 1.7f, RPM / 3000f);

                }
                else if (1000 < RPM && RPM <= 3000 && Mathf.Abs(gasInput) < 0.1f)
                {
                    Audios.LowOff.volume = Mathf.Lerp(0.3f, 0.7f, RPM / 3000f) * maxVolume;
                    Audios.LowOff.pitch = Mathf.Lerp(0.7f, 1.7f, RPM / 3000f);
                }
                else if (3000 < RPM && RPM <= 5500 && Mathf.Abs(gasInput) > 0.1f)
                {
                    Audios.MedOn.volume = Mathf.Lerp(0.3f, 0.7f, RPM / 5500f) * maxVolume;
                    Audios.MedOn.pitch = Mathf.Lerp(0.65f, 1.5f, RPM / 5500f);
                }
                else if (3000 < RPM && RPM <= 5500 && Mathf.Abs(gasInput) < 0.1f)
                {
                    Audios.MedOff.volume = Mathf.Lerp(0.3f, 0.7f, RPM / 5500f) * maxVolume;
                    Audios.MedOff.pitch = Mathf.Lerp(0.65f, 1.5f, RPM / 5500f);
                }
                else if (5500 < RPM && RPM <= redline && Mathf.Abs(gasInput) > 0.1f)
                {
                    Audios.HighOn.volume = Mathf.Lerp(0.3f, 0.7f, RPM / 7400) * maxVolume;
                    Audios.HighOn.pitch = Mathf.Lerp(0.7f, 1.5f, RPM / 7400);
                }
                else if (5500 < RPM && RPM <= redline && Mathf.Abs(gasInput) < 0.1f)
                {
                    Audios.HighOff.volume = Mathf.Lerp(0.3f, 0.7f, RPM / 7400) * maxVolume;
                    Audios.HighOff.pitch = Mathf.Lerp(0.7f, 1.5f, RPM / 7400);
                }
                else if (redline < RPM)
                    Audios.maxRPM.volume = 1 * maxVolume;
            }
        }
    }

    public void MuteAll()
    {
        Audios.LowOff.volume = 0;
        Audios.LowOn.volume = 0;
        Audios.MedOff.volume = 0;
        Audios.MedOn.volume = 0;
        Audios.HighOff.volume = 0;
        Audios.HighOn.volume = 0;
        Audios.maxRPM.volume = 0;
        Audios.Idle.volume = 0;
    }

    public IEnumerator StartEngine()
    {
        Audios.Startup.volume = 1 * maxVolume;
        Audios.Startup.Play();
        yield return new WaitForSeconds(0.6f);
        Audios.Idle.volume = 1;
        isEngineRunning = true;
        yield return new WaitForSeconds(0.4f);
        carMovement.SetIsEngineRunning(2);
    }
}

[System.Serializable]
public class AudioSources
{
    public AudioSource HighOn;
    public AudioSource HighOff;
    public AudioSource MedOn;
    public AudioSource MedOff;
    public AudioSource LowOn;
    public AudioSource LowOff;
    public AudioSource Idle;
    public AudioSource Startup;
    public AudioSource maxRPM;
}

