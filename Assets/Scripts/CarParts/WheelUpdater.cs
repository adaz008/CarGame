using UnityEngine;

public class WheelUpdater : MonoBehaviour
{
    [Header("Wheels")]
    [SerializeField] private WheelColliders colliders;
    [SerializeField] private WheelTransforms transforms;
    private float radius = 5f;
    private const float baseRadius = 5f;
    private float ForwardStifness;
    private float SidewaysStifness;


	private void Awake()
	{
        ForwardStifness = colliders.RRWheel.forwardFriction.stiffness;
        SidewaysStifness = colliders.RRWheel.sidewaysFriction.stiffness;
	}

	public void UpdateRadius(float newValue)
    {
        radius = baseRadius + newValue / 20;
    }

    public void handleGas(float currentTorque, float gasInput)
    {
        colliders.RRWheel.motorTorque = currentTorque * gasInput / 2;
        colliders.RLWheel.motorTorque = currentTorque * gasInput / 2;
    }

    public void handleBrake(bool handBrake, float brakeInput, float brakePower)
    {
        if (handBrake)
        {
            colliders.RRWheel.brakeTorque = Mathf.Infinity;
            colliders.RLWheel.brakeTorque = Mathf.Infinity;
		}
        else
        {
            colliders.FRWheel.brakeTorque = brakeInput * brakePower * 0.7f;
            colliders.FLWheel.brakeTorque = brakeInput * brakePower * 0.7f;

            colliders.RRWheel.brakeTorque = brakeInput * brakePower * 0.3f;
            colliders.RLWheel.brakeTorque = brakeInput * brakePower * 0.3f;
        }
    }

	public void handleFriction(bool handBrake)
	{
		ChangeFriction(colliders.RRWheel, handBrake);
		ChangeFriction(colliders.RLWheel, handBrake);
	}

	private void ChangeFriction(WheelCollider collider, bool handBrake)
    {
		WheelFrictionCurve forwardFriction = collider.forwardFriction;
        forwardFriction.stiffness = (handBrake) ?
                        Mathf.Lerp(forwardFriction.stiffness, 0.7f, Time.deltaTime * 2f) :
                        Mathf.Lerp(forwardFriction.stiffness, ForwardStifness, Time.deltaTime * 2f);
		collider.forwardFriction = forwardFriction;

		WheelFrictionCurve sidewaysFriction = collider.sidewaysFriction;
		sidewaysFriction.stiffness = (handBrake) ?
						Mathf.Lerp(sidewaysFriction.stiffness, 0.7f, Time.deltaTime * 2f) :
						Mathf.Lerp(sidewaysFriction.stiffness, SidewaysStifness, Time.deltaTime * 2f);
		collider.sidewaysFriction = sidewaysFriction;
	}

    public void handleSteering(float steeringInput)
    {
        float currentAngle = colliders.FLWheel.steerAngle;

        float newAnglePos = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius + (1.5f / 2))) * steeringInput;
        float newAngleMinus = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius - (1.5f / 2))) * steeringInput;

        if (steeringInput != 0)
        {
            float steeringAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius + (1.5f / 2))) * steeringInput;
            colliders.FLWheel.steerAngle = steeringAngle;
            colliders.FRWheel.steerAngle = steeringAngle;
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
