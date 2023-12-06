using Assets.Scripts.CarParts;
using Assets.Scripts.Menu.MenuSettings;
using UnityEngine;

public class InputManager
{
    private Motor _motor;

    public InputManager(Motor motor)
    {
        _motor = motor;
    }

    public void getInput(ref float gasInput, ref float steeringInput, ref Rigidbody playerRB, ref bool handBrake, ref bool lookBack)
    {
        GearState gearState = _motor.gearState;
        int currentGear = _motor.currentGear;
        float[] gearRatios = _motor.GearRatios;

        if (Input.GetKeyDown(UserSettings.Instance.Reset_Keyboard))
            playerRB.rotation = new Quaternion(0f, 0f, 0f, 1f);

        if (Input.GetKey(UserSettings.Instance.ShiftUp_Keyboard)
            && gearState == GearState.Running
            && UserSettings.Instance.Transmission == "Manual"
            && !_motor.isChanged)
        {
            if (currentGear != gearRatios.Length - 1)
            {
                _motor.isChanged = !_motor.isChanged;
                _motor.StartGearChangeCoroutine(1);
            }
        }

        if (Input.GetKey(UserSettings.Instance.ShiftDown_Keyboard)
            && gearState == GearState.Running
            && UserSettings.Instance.Transmission == "Manual"
            && !_motor.isChanged)
        {
            if (currentGear > 0)
            {
                _motor.isChanged = !_motor.isChanged;
                _motor.StartGearChangeCoroutine(-1);
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
    }
}
