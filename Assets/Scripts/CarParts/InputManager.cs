using Assets.Scripts.Menu.MenuSettings;
using UnityEngine;

public class InputManager
{
    private Transmission _transmission;
    private Motor _motor;
    private CarMovement _carmovement;

    public InputManager(Motor motor, Transmission transmission, CarMovement carmovement)
    {
        _motor = motor;
        _transmission = transmission;
        _carmovement = carmovement;
    }

    public void getInput(ref float gasInput, ref float steeringInput, ref Rigidbody playerRB, ref bool handBrake, ref bool lookBack)
    {
        GearState gearState = _transmission.GearState;
        int currentGear = _transmission.CurrentGear;
        float[] gearRatios = _transmission.GearRatios;

        if (Input.GetKeyDown(UserSettings.Instance.Reset_Keyboard))
            playerRB.rotation = new Quaternion(0f, 0f, 0f, 1f);

        if (Input.GetKey(UserSettings.Instance.ShiftUp_Keyboard)
            && gearState == GearState.Running
            && UserSettings.Instance.Transmission == "Manual"
            && !_carmovement.IsChanged)
        {
            if (currentGear != gearRatios.Length - 1)
            {
                _carmovement.SetIsChanged(!_carmovement.IsChanged);
                _transmission.StartGearChangeCoroutine(1);
            }
        }

        if (Input.GetKey(UserSettings.Instance.ShiftDown_Keyboard)
            && gearState == GearState.Running
            && UserSettings.Instance.Transmission == "Manual"
            && !_carmovement.IsChanged)
        {
            if (currentGear > 0)
            {
                _carmovement.SetIsChanged(!_carmovement.IsChanged);
                _transmission.StartGearChangeCoroutine(-1);
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


        if (Input.GetKeyDown(UserSettings.Instance.ChangeCamera_Keyboard))
        {
            UserSettings.Instance.nextCamera();
        }


        if ((gasInput == 0 &&
            !float.IsNaN(playerRB.velocity.x) &&
            !float.IsNaN(playerRB.velocity.y) &&
            !float.IsNaN(playerRB.velocity.z) &&
            playerRB.velocity.magnitude != 0f)
            ||
            (UserSettings.Instance.Transmission == "Manual") &&
            _motor.GetRPM() > _motor.RedLineEnd)
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



        //Ha benyomjuk a kuplungot akkor 0 egyébként az idõ függvényében váltózik 0 és 1 között
        //Ahogyan csusztatnánk
        if (gearState != GearState.Changing)
        {
            if (gearState == GearState.Neutral)
            {
                _transmission.SetClutch(0);
                if (gasInput > 0)
                    gearState = GearState.Running;
                else if (gasInput < 0)
                    gearState = GearState.Reverse;
            }
            else
                //clutch = Input.GetKey(KeyCode.AltGr) ? 0 : Mathf.Lerp(clutch, 1, Time.deltaTime);
                _transmission.SetClutch(Mathf.Lerp(_transmission.Clutch, 1, Time.deltaTime));
        }
        else
            _transmission.SetClutch(0);


    }
}
