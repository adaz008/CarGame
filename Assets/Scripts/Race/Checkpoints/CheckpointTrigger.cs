using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CheckpointTrigger : CheckpointTriggerBase
{
    protected override void Start(){
        base.Start();
        trackCheckPoints.EnableFirstCheckpoint(this);
    }
    protected override void OnTriggerEnter(Collider other){
        if (other.TryGetComponent<CarMovement>(out CarMovement player))
        {
            Debug.Log(this.name);

            trackCheckPoints.PlayerThroughCheckpoint(this);
        }
    }
}
