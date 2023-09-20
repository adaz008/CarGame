using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System;
using Random = UnityEngine.Random;
using UnityEngine.UI;
using Model;
using System.Runtime.ConstrainedExecution;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public enum GearState
{
    Neutral,
    Running,
    CheckingChange,
    Changing,
    Reverse
};

public enum CheckpointMiss
{
    Missed,
    Reset
};

public class CarMovement : MonoBehaviour
{
    private Rigidbody playerRB;

    public float gasInput;
    private float steeringInput;
    private float brakeInput;

    private float speed;
    private float slipAngle;

    [Header("Motor settings")]
    [SerializeField] private float motorPower;
    [SerializeField] private float brakePower;
    [SerializeField] private AnimationCurve hpToRPMCurve;
    public float RPM;
    public float redLineStart;
    [SerializeField] private float redLineEnd;
    [SerializeField] private float idleRPM;

    [Header("Wheels")]
    public WheelColliders colliders;
    public WheelTransforms transforms;

    [Header("UI")]
    [SerializeField] private TMP_Text speedText;
    [SerializeField] private TMP_Text gearText;
    [SerializeField] private Transform rpmNeedle;
    [SerializeField] private float minRPMNeedleRotation;
    [SerializeField] private float maxRPMNeedleRotation;
    [SerializeField] private Image needleCircle;


    [Header("Transmission")]
    [SerializeField] private float[] gearRatios;
    [SerializeField] private float differentialRatio;
    [SerializeField] private float increaseGearRPM;
    [SerializeField] private float decreaseGearRPM;
    [SerializeField] private float changeGearTime;
    [SerializeField] private float tireDiameter;
    private float currentTorque;
    private float clutch;
    private float wheelRPM;
    private GearState gearState;
    private int currentGear;
    public int isEngineRunning;


    [Header("Brake lamp")]
    [SerializeField] private Material brakeMaterial;
    [SerializeField] private Color brakingColor;
    [SerializeField] private float brakeColorIntensity;

    private bool handBrake = false;
    public static bool lookBack = false;
    private bool isRaceStarting = false;

    private bool isChanged = false;

    private float radius = 5, downForceValue = 0f;

    //Amikor kihagyja a checkpointot és vissza kell tenni
    Vector3 resetPos = Vector3.zero;
    Quaternion resetRot = Quaternion.identity;
    Vector3 resetVelocity = Vector3.zero;

    GearState resetGearState = GearState.Neutral;
    int resetGearIndex = 0;
    float resetRPM = 0f;

    private void Start()
    {
        playerRB = gameObject.GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        rpmNeedle.rotation = Quaternion.Euler(0f, 0f, Mathf.Lerp(minRPMNeedleRotation, maxRPMNeedleRotation, RPM / redLineEnd));

        int KPH = (int)(playerRB.velocity.magnitude * 3.6f);

        speedText.text = KPH.ToString();

        if (gearState == GearState.Neutral)
            gearText.text = "N";
        else if (gearState == GearState.Reverse)
            gearText.text = "R";
        else
            gearText.text = (currentGear + 1).ToString();

        speed = playerRB.velocity.magnitude;

        radius = 5 + KPH / 20;

        if (!isRaceStarting)
            GetInput();
        //AddDownForce();
        ApplyBreaking();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
        CheckReadline();
    }

    private void CheckReadline()
    {
        if (RPM > redLineStart)
        {
            needleCircle.color = Color.red;
            gearText.color = Color.red;
        }
        else
        {
            needleCircle.color = Color.white;
            gearText.color = Color.white;
        }
    }

    private void AddDownForce()
    {
        playerRB.AddForce(-transform.up * downForceValue * playerRB.velocity.magnitude);
    }

    private void GetInput()
    {
        if (Input.GetKeyDown(UserSettings.Instance.Reset_Keyboard))
            playerRB.rotation = new Quaternion(0f, 0f, 0f, 1f);

        if (Input.GetKey(UserSettings.Instance.ShiftUp_Keyboard)
            && gearState == GearState.Running
            && UserSettings.Instance.Transmission == "Manual"
            && !isChanged)
        {
            if (currentGear != gearRatios.Length - 1)
            {
                isChanged = !isChanged;
                StartCoroutine(ChangeGear(1));
            }
        }

        if (Input.GetKey(UserSettings.Instance.ShiftDown_Keyboard)
            && gearState == GearState.Running
            && UserSettings.Instance.Transmission == "Manual"
            && !isChanged)
        {
            if (currentGear > 0)
            {
                isChanged = !isChanged;
                StartCoroutine(ChangeGear(-1));
            }
        }

        gasInput = 0;
        if (Input.GetKey(UserSettings.Instance.Accelerate_Keyboard))
            gasInput = 1;
        else if (Input.GetKey(UserSettings.Instance.Brake_Reverse_Keyboard))
            gasInput = -1;

        steeringInput = 0;
        if (Input.GetKey(UserSettings.Instance.SteerRight_Keyboard))
            steeringInput = 1;
        else if (Input.GetKey(UserSettings.Instance.SteerLeft_Keyboard))
            steeringInput = -1;

        handBrake = Input.GetKey(UserSettings.Instance.Handbrake_Keyboard) ? true : false;

        lookBack = Input.GetKey(UserSettings.Instance.LookBack_Keyboard) ? true : false;

        NitroEffect.boosting = Input.GetKey(UserSettings.Instance.Nitro_Keyboard) ? true : false;

        slipAngle = Vector3.Angle(transform.forward, playerRB.velocity - transform.forward);

        if (Mathf.Abs(gasInput) > 0 && isEngineRunning == 0)
        {
            isEngineRunning = 1;
            StartCoroutine(GetComponent<EngineAudio>().StartEngine());
            gearState = GearState.Running;
        }

        if ((gasInput == 0 &&
            !float.IsNaN(playerRB.velocity.x) &&
            !float.IsNaN(playerRB.velocity.y) &&
            !float.IsNaN(playerRB.velocity.z) &&
            playerRB.velocity.magnitude != 0f)
            ||
            (UserSettings.Instance.Transmission == "Manual") &&
            RPM > redLineEnd)
        {
            float speed = playerRB.velocity.magnitude * 3.6f;
            float newSpeed = playerRB.velocity.magnitude * 3.6f - 0.295f;

            if (speed > 120f)
                newSpeed = playerRB.velocity.magnitude * 3.6f - 0.32f;
            else if (speed > 180f)
                newSpeed = playerRB.velocity.magnitude * 3.6f - 0.35f;

            float ratio = Mathf.Pow(newSpeed, 2) / Mathf.Pow(speed, 2);

            playerRB.velocity *= ratio < 1f ? ratio : 1f;
        }

        float movingDirection = Vector3.Dot(transform.forward, playerRB.velocity);

        //Ha benyomjuk a kuplungot akkor 0 egyébként az idõ függvényében váltózik 0 és 1 között
        //Ahogyan csusztatnánk
        if (gearState != GearState.Changing)
        {
            if (gearState == GearState.Neutral)
            {
                clutch = 0;
                if (gasInput > 0)
                    gearState = GearState.Running;
                else if (gasInput < 0)
                    gearState = GearState.Reverse;
            }
            else
                //clutch = Input.GetKey(KeyCode.AltGr) ? 0 : Mathf.Lerp(clutch, 1, Time.deltaTime);
                clutch = Mathf.Lerp(clutch, 1, Time.deltaTime);
        }
        else
            clutch = 0;

        //Új kocsik
        if (movingDirection < -0.5f && gasInput > 0)
            brakeInput = Mathf.Abs(gasInput);
        else if (movingDirection > 0.5f && gasInput < 0)
            brakeInput = Mathf.Abs(gasInput);
        else
            brakeInput = 0;

    }

    private void HandleMotor()
    {
        currentTorque = CalculateTorque();

        if (brakeInput > 0)
            currentTorque = 0;

        colliders.RRWheel.motorTorque = currentTorque * gasInput / 2;
        colliders.RLWheel.motorTorque = currentTorque * gasInput / 2;

        //colliders.FRWheel.motorTorque = currentTorque * gasInput;
        //colliders.FLWheel.motorTorque = currentTorque * gasInput;
    }

    float CalculateTorque()
    {
        float torque = 0;
        //Ha megáll, üresbe kerül
        if (RPM < idleRPM + 200 && gasInput == 0 && currentGear == 0)
            gearState = GearState.Neutral;

        //Ha elõre menetbõl egybõl tolatni kezd
        if (RPM < idleRPM + 200 && gasInput < 0 && currentGear == 0)
            gearState = GearState.Reverse;

        //Ha tolatásból egybõl elõre megy
        if (RPM < idleRPM + 200 && gasInput > 0 && currentGear == 0)
            gearState = GearState.Running;

        //Ha jár a motor és a kuplung ki van nyomva
        if (gearState == GearState.Running && clutch > 0 && UserSettings.Instance.Transmission == "Auto")
        {
            if (RPM > increaseGearRPM)
                StartCoroutine(ChangeGear(1));
            else if (RPM < decreaseGearRPM)
                StartCoroutine(ChangeGear(-1));
        }

        //Ha jár a motor
        if (isEngineRunning > 1)
        {
            //Ha a kuplung be van nyomva
            if (clutch == 0f)
            {
                //Random azért kell, hogy tiltásnál ugráljon a mutató picit
                //Illetve nézzük, hogy az alapjárat vagy az adott fordulat a nagyobb és afelé közeledik a mutató
                RPM = Mathf.Lerp(RPM, Mathf.Max(idleRPM, redLineEnd * gasInput) + Random.Range(-50, 50), Time.deltaTime);
            }
            else
            {
                //Motor fordulatszáma
                //playerRB.velocity.magnitude * 2.25f = mérföld/órában a sebesség
                //gearRatios[currentGear] =  adott fokozat áttétele
                //336f konstans
                //tireDiameter = kerek magassag
                RPM = (playerRB.velocity.magnitude * 2.25f * gearRatios[currentGear] * 336f * differentialRatio) / tireDiameter;
                RPM = RPM > redLineEnd ? (redLineEnd + Random.Range(-100, 100)) : RPM;
                //Nyomaték newtonméterben
                torque = (hpToRPMCurve.Evaluate(RPM / redLineEnd) * motorPower / RPM) * gearRatios[currentGear] * differentialRatio * 5252f * clutch;
                if (RPM > redLineEnd)
                    torque = 0f;
            }
        }
        return torque;

    }

    private void ApplyBreaking()
    {
        if (handBrake)
        {
            colliders.RRWheel.brakeTorque = brakePower;
            colliders.RLWheel.brakeTorque = brakePower;
        }
        else
        {
            colliders.FRWheel.brakeTorque = brakeInput * brakePower * 0.7f;
            colliders.FLWheel.brakeTorque = brakeInput * brakePower * 0.7f;

            colliders.RRWheel.brakeTorque = brakeInput * brakePower * 0.3f;
            colliders.RLWheel.brakeTorque = brakeInput * brakePower * 0.3f;

            if (brakeMaterial)
            {
                if (brakeInput > 0)
                {
                    brakeMaterial.EnableKeyword("_EMISSION");
                    brakeMaterial.SetColor("_EmissionColor", brakingColor * Mathf.Pow(2f, brakeColorIntensity));
                }
                else
                {
                    brakeMaterial.DisableKeyword("_EMISSION");
                    brakeMaterial.SetColor("_EmissionColor", Color.black);
                }
            }
        }
    }

    private void HandleSteering()
    {
        float currentAngle = colliders.FLWheel.steerAngle;

        float newAnglePos = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius + (1.5f / 2))) * steeringInput;
        float newAngleMinus = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius - (1.5f / 2))) * steeringInput;

        if (steeringInput > 0)
        {
            //rear tracks size is set to 1.5f       wheel base has been set to 2.55f
            colliders.FLWheel.steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius + (1.5f / 2))) * steeringInput;
            colliders.FRWheel.steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius - (1.5f / 2))) * steeringInput;
        }
        else if (steeringInput < 0)
        {
            colliders.FLWheel.steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius + (1.5f / 2))) * steeringInput;
            colliders.FRWheel.steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius - (1.5f / 2))) * steeringInput;

        }
        else
        {
            colliders.FLWheel.steerAngle = 0;
            colliders.FRWheel.steerAngle = 0;
        }


        //float currentAngleLeft = colliders.FLWheel.steerAngle;
        //float currentAngleRight = colliders.FRWheel.steerAngle;
        //float newAnglePos = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius + (1.5f / 2))) * steeringInput;
        //float newAngleMinus = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius - (1.5f / 2))) * steeringInput;

        //float speed = 3f;

        //if(steeringInput > 0)
        //{
        //    colliders.FLWheel.steerAngle = Mathf.Lerp(currentAngleLeft, newAnglePos, speed * Time.deltaTime);
        //    colliders.FRWheel.steerAngle = Mathf.Lerp(currentAngleRight, newAngleMinus, speed * Time.deltaTime);
        //}else if(steeringInput < 0)
        //{
        //    colliders.FLWheel.steerAngle = Mathf.Lerp(currentAngleLeft, newAngleMinus, speed * Time.deltaTime);
        //    colliders.FRWheel.steerAngle = Mathf.Lerp(currentAngleRight, newAnglePos, speed * Time.deltaTime);
        //}else
        //{
        //    colliders.FLWheel.steerAngle = Mathf.Lerp(currentAngleLeft, 0f, speed * Time.deltaTime);
        //    colliders.FRWheel.steerAngle = Mathf.Lerp(currentAngleRight, 0f, speed * Time.deltaTime);
        //}
    }


    private void UpdateWheels()
    {
        UpdateSingleWheel(colliders.FLWheel, transforms.FLWheel);
        UpdateSingleWheel(colliders.FRWheel, transforms.FRWheel);
        UpdateSingleWheel(colliders.RLWheel, transforms.RLWheel);
        UpdateSingleWheel(colliders.RRWheel, transforms.RRWheel);

    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }

    public void StartRacePos(Vector3 position)
    {
        if (isEngineRunning != 0)
        {
            playerRB.velocity = Vector3.zero;
            //playerRB.transform.position = new Vector3(-1035, 0.5f, -29);
            //playerRB.transform.position = position;
            Collider carCollider = playerRB.GetComponentInChildren<Collider>();
            if (carCollider)
            {
                Vector3 adjustedPosition = position - carCollider.bounds.extents.x * playerRB.transform.forward;
                playerRB.MovePosition(adjustedPosition);
            }

            playerRB.transform.rotation = Quaternion.Euler(0, 270, 0);

            gearState = GearState.Running;
            currentGear = 1;
            RPM = idleRPM;
        }

        SetRaceStarting(true);
    }

    public void MissedCheckpointReset(CheckpointMiss state)
    {
        switch (state)
        {
            case CheckpointMiss.Missed:
                resetPos = playerRB.position;
                resetRot = playerRB.rotation;
                resetVelocity = playerRB.velocity;
                resetGearState = gearState;
                resetGearIndex = currentGear;
                resetRPM = RPM;
                break;
            case CheckpointMiss.Reset:
                playerRB.position = resetPos;
                playerRB.rotation = resetRot;
                playerRB.velocity = resetVelocity;
                currentGear = resetGearIndex;
                gearState = resetGearState;
                RPM = resetRPM;
                break;
        }
    }

    public void SetRaceStarting(bool value)
    {
        isRaceStarting = value;
    }

    IEnumerator ChangeGear(int gearChange)
    {
        if (UserSettings.Instance.Transmission == "Auto")
        {
            //Ellenõrizzük, hogy történik-e váltás
            gearState = GearState.CheckingChange;
            if (currentGear + gearChange >= 0)
            {
                if (gearChange > 0)
                {
                    //Növelni szeretnénk a fokozatot
                    //Várunk egy picit hogy tényleg kell-e váltani
                    if (currentGear == 1)
                        yield return new WaitForSeconds(0.4f);
                    else
                        yield return new WaitForSeconds(0.05f);
                    //Ha a fordulatszám kisebb mint a felfele váltás fordulatszáma vagy elértük a maximális sebességi fokozatot akkor nem váltunk
                    if (RPM < increaseGearRPM || currentGear >= gearRatios.Length - 1)
                    {
                        gearState = GearState.Running;
                        yield break;
                    }
                }
                else
                {
                    //Csökkenteni szeretnénk a fokozatot
                    //Várunk egy picit hogy tényleg kell-e váltani
                    yield return new WaitForSeconds(0.1f);

                    //Ha a fordulatszám nagyobb mint a lefele váltás fordulatszáma vagy nem tudunk már lefele váltani(üresben vagyunk)
                    if (RPM > decreaseGearRPM || currentGear <= 0)
                    {
                        gearState = GearState.Running;
                        yield break;
                    }
                }
                //Váltás történik
                gearState = GearState.Changing;
                yield return new WaitForSeconds(changeGearTime);
                //Váltottunk
                currentGear += gearChange;
            }
            if (gearState != GearState.Neutral)
                gearState = GearState.Running;
        }
        else
        {
            gearState = GearState.Changing;
            yield return new WaitForSeconds(changeGearTime);
            //Váltottunk
            currentGear += gearChange;
            gearState = GearState.Running;
            isChanged = !isChanged;
        }
    }
}

[System.Serializable]
public class WheelColliders
{
    public WheelCollider FRWheel;
    public WheelCollider FLWheel;
    public WheelCollider RRWheel;
    public WheelCollider RLWheel;
}

[System.Serializable]
public class WheelTransforms
{
    public Transform FRWheel;
    public Transform FLWheel;
    public Transform RRWheel;
    public Transform RLWheel;
}
