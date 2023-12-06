using System;
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
	private float downforce;
	private float DownForceValue = 10f;

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
        playerRB = gameObject.GetComponent<Rigidbody>();
        wheelUpdater = gameObject.GetComponent<WheelUpdater>();
        carUIManager = gameObject.GetComponent<CarUIManager>();
        carInsideUIManager = gameObject.GetComponent<CarInsideUIManager>();
        motor = gameObject.GetComponent<Motor>();

        inputManager = new InputManager(motor);
        raceManager = new RaceManager(motor, playerRB);
    }

    private void FixedUpdate()
    {
        int KPH = (int)(playerRB.velocity.magnitude * 3.6f);

        AddDownForce(KPH);

        UpdateUI(KPH);

        wheelUpdater.UpdateRadius(KPH);
        wheelUpdater.handleFriction(handBrake);

        if (!isRaceStarting)
            GetInput();
        EngineBrake();
        ApplyBraking();
        HandleMotor();

        wheelUpdater.handleSteering(steeringInput);
        wheelUpdater.UpdateWheels();

        CheckRedline();
    }

    private void UpdateUI(int KPH)
    {
		carUIManager.UpdateUI(KPH);
		carInsideUIManager.UpdateUI(KPH);
	}

	private void CheckRedline()
	{
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

    private void EngineBrake()
    {
		motor.EngineBrake(ref playerRB, gasInput);
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

	private void AddDownForce(int KPH)
	{
		downforce = Mathf.Abs(DownForceValue * playerRB.velocity.magnitude);
		downforce = KPH > 60 ? downforce : 0;
		playerRB.AddForce(-transform.up * downforce);

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
