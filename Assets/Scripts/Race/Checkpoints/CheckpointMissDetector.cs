using UnityEngine;

public class CheckpointMissDetector : CheckpointTriggerBase
{
    protected override void Start()
    {
        base.Start();
        trackCheckPoints.EnableFirstCheckpointMiss(this);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<CarMovement>(out CarMovement player))
        {
            GameObject trackParent = GameObject.FindGameObjectWithTag("TrackParent");
            CountDownController countDownController = trackParent.GetComponentInChildren<CountDownController>();

            countDownController.StartMissedCounter();
        }
    }
}
