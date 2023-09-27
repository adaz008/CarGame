using UnityEngine;

public class PreCheckpointZoneTrigger : CheckpointTriggerBase
{
    protected override void Start()
    {
        base.Start();
        trackCheckPoints.EnableFirstCheckpointMissReset(this);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<CarMovement>(out CarMovement player))
        {
            player.MissedCheckpointReset(CheckpointMiss.Missed);
            Disable();
        }
    }
}
