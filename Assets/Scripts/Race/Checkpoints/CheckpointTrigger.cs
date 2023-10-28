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
            trackCheckPoints.PlayerThroughCheckpoint(this);
        }
    }
}
