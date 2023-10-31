using UnityEngine;


public class CarMovement : MonoBehaviour
{
    private Rigidbody playerRB;

    private float gasInput;
    private float steeringInput;
    private float brakeInput;

    [SerializeField] private AnimationCurve hpToRPMCurve;

    private float currentTorque;
    private int isEngineRunning;

    private bool handBrake = false;
    private bool lookBack = false;
    private bool isRaceStarting = false;

    private bool isChanged = false;

    private WheelUpdater wheelUpdater;
    private CarUIManager carUIManager;
    private Motor motor;
    private Transmission transmission;
    private InputManager inputManager;
    private RaceManager raceManager;
    private void Start()
    {
        CarMovement carMovement = GetComponent<CarMovement>();
        playerRB = gameObject.GetComponent<Rigidbody>();
        wheelUpdater = gameObject.GetComponent<WheelUpdater>();
        carUIManager = gameObject.GetComponent<CarUIManager>();
        motor = gameObject.GetComponent<Motor>();
        transmission = gameObject.GetComponent<Transmission>();

        inputManager = new InputManager(motor, transmission, carMovement);
        raceManager = new RaceManager(motor, transmission, playerRB);
    }

    private void FixedUpdate()
    {
        int KPH = (int)(playerRB.velocity.magnitude * 3.6f);

        carUIManager.UpdateUI(KPH);

        wheelUpdater.UpdateRadius(KPH);

        if (!isRaceStarting)
            GetInput();
        ApplyBreaking();
        HandleMotor();
        wheelUpdater.handleSteering(steeringInput);
        wheelUpdater.UpdateWheels();

        carUIManager.CheckRedLine();
    }
    private void GetInput()
    {
        inputManager.getInput(ref gasInput, ref steeringInput, ref playerRB, ref handBrake, ref lookBack);

        if (Mathf.Abs(gasInput) > 0 && isEngineRunning == 0)
        {
            isEngineRunning = 1;
            StartCoroutine(GetComponent<EngineAudio>().StartEngine());
            transmission.SetGearState(GearState.Running);
        }
        float movingDirection = Vector3.Dot(transform.forward, playerRB.velocity);

        if (movingDirection < -0.5f && gasInput > 0)
            brakeInput = Mathf.Abs(gasInput);
        else if (movingDirection > 0.5f && gasInput < 0)
            brakeInput = Mathf.Abs(gasInput);
        else
            brakeInput = 0;
    }

    private void HandleMotor()
    {
        currentTorque = motor.CalculateTorque(playerRB, gasInput);

        if (brakeInput > 0)
            currentTorque = 0;
        wheelUpdater.handleGas(currentTorque, gasInput);
    }

    private void ApplyBreaking()
    {
        wheelUpdater.handleBrake(handBrake, brakeInput);
        carUIManager.BrakeLampChange(handBrake, brakeInput);
    }

    public void StartRacePos(Vector3 position)
    {
        raceManager.StartRacePos(position, playerRB, isEngineRunning);
        SetRaceStarting(true);
    }

    public void SetRaceStarting(bool value)
    {
        isRaceStarting = value;
    }

    public float GasInput => gasInput;
    public int IsEngineRunning => isEngineRunning;

    public bool IsChanged => isChanged;

    public bool LookBack => lookBack;
    public void SetIsEngineRunning(int value) { isEngineRunning = value; }
    public void SetIsChanged(bool value) { isChanged = value; }
}
