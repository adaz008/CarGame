using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissedCheckpointResetTrigger : MonoBehaviour
{
    [SerializeField] private TrackCheckPoints trackCheckPoints;
    BoxCollider triggerCollider;

    private void Awake()
    {
        triggerCollider = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        trackCheckPoints.EnableFirstCheckpointMissReset(this);
    }
    public void SetTrackCheckpoints(TrackCheckPoints trackCheckPoints)
    {
        this.trackCheckPoints = trackCheckPoints;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<CarMovement>(out CarMovement player))
        {
            player.MissedCheckpointReset(CheckpointMiss.Missed);
            Disable();
        }
    }

    public void Enable()
    {
        triggerCollider.enabled = true;
    }

    public void Disable()
    {
        triggerCollider.enabled = false;
    }
}
