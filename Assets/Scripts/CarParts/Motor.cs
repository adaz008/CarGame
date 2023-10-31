using UnityEngine;

public class Motor : MonoBehaviour
{
    [Header("Motor settings")]
    [SerializeField] private float motorPower;
    [SerializeField] private float brakePower;
    [SerializeField] private AnimationCurve hpToRPMCurve;
    private float RPM;
    [SerializeField] private float redLineStart;
    [SerializeField] private float redLineEnd;
    [SerializeField] private float increaseGearRPM;
    [SerializeField] private float decreaseGearRPM;
    private readonly float idleRPM = 800f;
    private readonly float tireDiameter = 25f;

    private Transmission transmission;
    private CarMovement carMovement;

    public float BrakePower => brakePower;

    public float RedLineStart => redLineStart;
    public float RedLineEnd => redLineEnd;
    public float IncreaseGearRPM => increaseGearRPM;
    public float DecreaseGearRPM => decreaseGearRPM;
    public float GetRPM() { return RPM; }
    public void SetRPM(float value) { RPM = value; }

    private void Start()
    {
        carMovement = gameObject.GetComponent<CarMovement>();
        transmission = gameObject.GetComponent<Transmission>();
    }

    public float CalculateTorque(Rigidbody playerRB, float gasInput)
    {
        float torque = 0;
        int isEngineRunning = carMovement.IsEngineRunning;
        float clutch = transmission.Clutch;
        float[] gearRatios = transmission.GearRatios;
        float differentialRatio = transmission.DifferentialRatio;
        int currentGear = transmission.CurrentGear;

        CheckForGearChange(gasInput);

        //Ha j�r a motor
        if (isEngineRunning > 1)
        {
            //Ha a kuplung be van nyomva
            if (clutch == 0f)
            {
                //Random az�rt kell, hogy tilt�sn�l ugr�ljon a mutat� picit
                //Illetve n�zz�k, hogy az alapj�rat vagy az adott fordulat a nagyobb �s afel� k�zeledik a mutat�
                RPM = Mathf.Lerp(RPM, Mathf.Max(idleRPM, redLineEnd * gasInput) + Random.Range(-50, 50), Time.deltaTime);
            }
            else
            {
                //Motor fordulatsz�ma
                //playerRB.velocity.magnitude * 2.25f = m�rf�ld/�r�ban a sebess�g
                //gearRatios[currentGear] =  adott fokozat �tt�tele
                //336f konstans
                //tireDiameter = kerek magassag

                RPM = (playerRB.velocity.magnitude * 2.25f * gearRatios[currentGear] * 336f * differentialRatio) / tireDiameter;
                RPM = RPM > redLineEnd ? (redLineEnd + Random.Range(-100, 100)) : RPM;
                //Nyomat�k newtonm�terben
                torque = (hpToRPMCurve.Evaluate(RPM / redLineEnd) * motorPower / RPM) * gearRatios[currentGear] * differentialRatio * 5252f * clutch;
                if (RPM > redLineEnd)
                    torque = 0f;
            }
        }
        return torque;
    }

    public void CheckForGearChange(float gasInput)
    {
        int currentGear = transmission.CurrentGear;
        GearState gearState = transmission.GearState;
        //Ha meg�ll, �resbe ker�l
        if (RPM < idleRPM + 200 && gasInput == 0 && currentGear == 0)
            gearState = GearState.Neutral;

        //Ha el�re menetb�l egyb�l tolatni kezd
        if (RPM < idleRPM + 200 && gasInput < 0 && currentGear == 0)
            gearState = GearState.Reverse;

        //Ha tolat�sb�l egyb�l el�re megy
        if (RPM < idleRPM + 200 && gasInput > 0 && currentGear == 0)
            gearState = GearState.Running;

        transmission.SetGearState(gearState);

        //Ha j�r a motor �s a kuplung ki van nyomva
        if (gearState == GearState.Running && transmission.Clutch > 0 && UserSettings.Instance.Transmission == "Auto")
        {
            if (RPM > increaseGearRPM)
                transmission.StartGearChangeCoroutine(1);
            else if (RPM < decreaseGearRPM)
                transmission.StartGearChangeCoroutine(-1);
        }
    }

    public void setRPMToIdle()
    {
        RPM = idleRPM;
    }
}
