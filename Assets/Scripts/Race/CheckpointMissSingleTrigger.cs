using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointMissSingleTrigger : MonoBehaviour
{
    [SerializeField] private TrackCheckPoints trackCheckPoints;
    BoxCollider triggerCollider;

    private void Awake()
    {
        triggerCollider = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        trackCheckPoints.EnableFirstCheckpointMiss(this);
    }
    public void SetTrackCheckpoints(TrackCheckPoints trackCheckPoints)
    {
        this.trackCheckPoints = trackCheckPoints;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<CarMovement>(out CarMovement player))
        {
            GameObject trackParent = GameObject.FindGameObjectWithTag("TrackParent");
            CountDownController countDownController = trackParent.GetComponentInChildren<CountDownController>();

            countDownController.StartMissedCounter();
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
