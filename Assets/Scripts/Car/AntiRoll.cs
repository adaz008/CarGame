using UnityEngine;

public class AntiRoll : MonoBehaviour
{

    [SerializeField] private WheelCollider WheelL;
    [SerializeField] private WheelCollider WheelR;
    private float antiRoll = 5000.0f;

    private Rigidbody car;

    void Start()
    {
        car = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        WheelHit hit;
        float travelL = 1.0f;
        float travelR = 1.0f;


        bool groundedL = WheelL.GetGroundHit(out hit);
        if (groundedL)
        {
            travelL = (-WheelL.transform.InverseTransformPoint(hit.point).y - WheelL.radius) / WheelL.suspensionDistance;
        }

        bool groundedR = WheelR.GetGroundHit(out hit);
        if (groundedR)
        {
            travelR = (-WheelR.transform.InverseTransformPoint(hit.point).y - WheelR.radius) / WheelR.suspensionDistance;
        }

        float antiRollForce = (travelL - travelR) * antiRoll;

        if (groundedL)
            car.AddForceAtPosition(WheelL.transform.up * -antiRollForce, WheelL.transform.position);

        if (groundedR)
            car.AddForceAtPosition(WheelR.transform.up * antiRollForce, WheelR.transform.position);
    }
}
