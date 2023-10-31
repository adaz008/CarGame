using System;
using UnityEngine;

public class CheckpointMissDetector : CheckpointTriggerBase
{
    public static event Action MissedCheckpointTrigger;

    protected override void Start()
    {
        base.Start();
        trackCheckPoints.EnableFirstCheckpointMiss(this);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<CarMovement>(out CarMovement player))
        {
            MissedCheckpointTrigger?.Invoke();
        }
    }
}
