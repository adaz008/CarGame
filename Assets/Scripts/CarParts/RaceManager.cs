using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CheckpointMiss
{
    Missed,
    Reset
};

public class RaceManager : MonoBehaviour
{
    //Amikor kihagyja a checkpointot és vissza kell tenni
    Vector3 resetPos = Vector3.zero;
    Quaternion resetRot = Quaternion.identity;
    Vector3 resetVelocity = Vector3.zero;

    GearState resetGearState = GearState.Neutral;
    int resetGearIndex = 0;
    float resetRPM = 0f;

    [SerializeField] Transmission transmission;
    [SerializeField] Motor motor;

    public void StartRacePos(Vector3 position, Rigidbody playerRB, int isEngineRunning)
    {
        if (isEngineRunning != 0)
        {
            playerRB.velocity = Vector3.zero;
            Collider carCollider = playerRB.GetComponentInChildren<Collider>();
            if (carCollider)
            {
                Vector3 adjustedPosition = position - carCollider.bounds.extents.x * playerRB.transform.forward;
                playerRB.MovePosition(adjustedPosition);
            }

            playerRB.transform.rotation = Quaternion.Euler(0, 270, 0);

            transmission.SetGearState(GearState.Running);
            motor.setRPMToIdle();
        }
    }

    public void MissedCheckpointReset(CheckpointMiss state, Rigidbody playerRB)
    {
        switch (state)
        {
            case CheckpointMiss.Missed:
                resetPos = playerRB.position;
                resetRot = playerRB.rotation;
                resetVelocity = playerRB.velocity;
                resetGearState = transmission.GearState;
                resetGearIndex = transmission.CurrentGear;
                resetRPM = motor.GetRPM();
                break;
            case CheckpointMiss.Reset:
                playerRB.position = resetPos;
                playerRB.rotation = resetRot;
                playerRB.velocity = resetVelocity;
                transmission.SetCurrentGear(resetGearIndex);
                transmission.SetGearState(resetGearState);
                motor.SetRPM(resetRPM);
                break;
        }
    }
}
