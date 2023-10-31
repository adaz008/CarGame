using System;
using UnityEngine;

public class PreCheckpointZoneTrigger : CheckpointTriggerBase
{
    public static event Action PreCheckpointTrigger;

    protected override void Start()
    {
        base.Start();
        trackCheckPoints.EnableFirstCheckpointMissReset(this);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<CarMovement>(out CarMovement player))
        {
            PreCheckpointTrigger?.Invoke();
            //player.MissedCheckpointReset(CheckpointMiss.Missed);
            Disable();
        }
    }
}
