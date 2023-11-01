using Assets.Scripts.CarParts;
using UnityEngine;


public class CarMovement : MonoBehaviour
{
    private Rigidbody playerRB;

    private float gasInput;
    private float steeringInput;
    private float brakeInput;

    [SerializeField] private AnimationCurve hpToRPMCurve;

    private float currentTorque;
    //private int isEngineRunning;

    private bool handBrake = false;
    private bool lookBack = false;
    private bool isRaceStarting = false;

    public float GasInput => gasInput;
    public bool LookBack => lookBack;

    private WheelUpdater wheelUpdater;
    private CarUIManager carUIManager;
    private CarInsideUIManager carInsideUIManager;
    private Motor motor;
    private InputManager inputManager;
    private RaceManager raceManager;
    private void Start()
    {
        CarMovement carMovement = GetComponent<CarMovement>();
        playerRB = gameObject.GetComponent<Rigidbody>();
        wheelUpdater = gameObject.GetComponent<WheelUpdater>();
        carUIManager = gameObject.GetComponent<CarUIManager>();
        carInsideUIManager = gameObject.GetComponent<CarInsideUIManager>();
        motor = gameObject.GetComponent<Motor>();

        inputManager = new InputManager(motor, carMovement);
        raceManager = new RaceManager(motor, playerRB);
    }

    private void FixedUpdate()
    {
        int KPH = (int)(playerRB.velocity.magnitude * 3.6f);

        carUIManager.UpdateUI(KPH);
        carInsideUIManager.UpdateUI(KPH);

        wheelUpdater.UpdateRadius(KPH);

        if (!isRaceStarting)
            GetInput();
        ApplyBraking();
        HandleMotor();
        wheelUpdater.handleSteering(steeringInput);
        wheelUpdater.UpdateWheels();

        carUIManager.CheckRedLine();
        carInsideUIManager.CheckRedLine();
    }
    private void GetInput()
    {
        inputManager.getInput(ref gasInput, ref steeringInput, ref playerRB, ref handBrake, ref lookBack);

        if (Mathf.Abs(gasInput) > 0 && motor.isEngineRunning == 0)
        {
            motor.isEngineRunning = 1;
            StartCoroutine(GetComponent<EngineAudio>().StartEngine());
            motor.gearState = GearState.Running;
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

    private void ApplyBraking()
    {
        wheelUpdater.handleBrake(handBrake, brakeInput, motor.BrakePower);
        carUIManager.BrakeLampChange(handBrake, brakeInput);
    }

    public void StartRacePos(Vector3 position)
    {
        raceManager.StartRacePos(position, motor.isEngineRunning);
        isRaceStarting = true;
    }

    public void SetRaceStarting(bool value)
    {
        isRaceStarting = value;
    }

}
