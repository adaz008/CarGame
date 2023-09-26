using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelUpdater : MonoBehaviour
{
    [Header("Wheels")]
    [SerializeField] private WheelColliders colliders;
    [SerializeField] private WheelTransforms transforms;
    private float radius = 5;
    private const float baseRadius = 5f;
    [SerializeField] Motor motor;

    public void UpdateRadius(float newValue)
    {
        radius = baseRadius + newValue / 20;
    }

    public void handleGas(float currentTorque, float gasInput)
    {
        colliders.RRWheel.motorTorque = currentTorque * gasInput / 2;
        colliders.RLWheel.motorTorque = currentTorque * gasInput / 2;
    }

    public void handleBrake(bool handBrake, float brakeInput)
    {
        float brakePower = motor.BrakePower;
        if (handBrake)
        {
            colliders.RRWheel.brakeTorque = brakePower;
            colliders.RLWheel.brakeTorque = brakePower;
        }
        else
        {
            Debug.Log("BrakePower:" + brakePower);
            Debug.Log("BrakeInput:" + brakeInput);

            colliders.FRWheel.brakeTorque = brakeInput * brakePower * 0.7f;
            colliders.FLWheel.brakeTorque = brakeInput * brakePower * 0.7f;

            colliders.RRWheel.brakeTorque = brakeInput * brakePower * 0.3f;
            colliders.RLWheel.brakeTorque = brakeInput * brakePower * 0.3f;
        }
    }

    public void handleSteering(float steeringInput)
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
    }

    public void UpdateWheels()
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

