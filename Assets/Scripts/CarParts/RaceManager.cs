using Assets.Scripts.CarParts;
using UnityEngine;


public class RaceManager
{
    //Amikor kihagyja a checkpointot és vissza kell tenni
    private Rigidbody _player;
    Vector3 resetPos = Vector3.zero;
    Quaternion resetRot = Quaternion.identity;
    Vector3 resetVelocity = Vector3.zero;

    GearState resetGearState = GearState.Neutral;
    int resetCurrentGear = 0;
    float resetRPM = 0f;

    //private Transmission _transmission;
    private Motor _motor;

    public RaceManager(Motor motor,Rigidbody player)
    {
        _motor = motor;
        _player = player;

        PreCheckpointZoneTrigger.PreCheckpointTrigger += MissedCheckpointMissed;
        CountDownController.MissedCheckpointReset += MissedCheckpointReset;
    }

    public void StartRacePos(Vector3 position, Rigidbody playerRB, int isEngineRunning)
    {
        if (isEngineRunning != 0)
        {
            Collider carCollider = playerRB.GetComponentInChildren<Collider>();
            if (carCollider)
            {
                Vector3 adjustedPosition = position - carCollider.bounds.extents.x * playerRB.transform.forward;
                playerRB.MovePosition(adjustedPosition);
            }

            playerRB.transform.rotation = Quaternion.Euler(0, 270, 0);

            _motor.gearState = GearState.Running;
            _motor.setRPMToIdle();
            playerRB.velocity = Vector3.zero;
        }
    }


    public void MissedCheckpointMissed()
    {
        resetPos = _player.position;
        resetRot = _player.rotation;
        resetVelocity = _player.velocity;
        resetGearState = _motor.gearState;
        resetCurrentGear = _motor.currentGear;
        resetRPM = _motor.RPM;
    }

    public void MissedCheckpointReset()
    {
        _player.position = resetPos;
        _player.rotation = resetRot;
        _player.velocity = resetVelocity;
        _motor.currentGear = resetCurrentGear;
        _motor.gearState = resetGearState;
        _motor.RPM = resetRPM;
    }
}
