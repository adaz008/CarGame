using UnityEngine;

public class CheckpointTriggerBase : MonoBehaviour
{
    [SerializeField] protected TrackCheckPoints trackCheckPoints;
    protected BoxCollider triggerCollider;

    protected virtual void Awake()
    {
        triggerCollider = GetComponent<BoxCollider>();
    }

    protected virtual void Start() {}

    protected virtual void OnTriggerEnter(Collider other) {}

    public void SetTrackCheckpoints(TrackCheckPoints trackCheckPoints)
    {
        this.trackCheckPoints = trackCheckPoints;
    }

    public virtual void Enable()
    {
        triggerCollider.enabled = true;
    }

    public virtual void Disable()
    {
        triggerCollider.enabled = false;
    }
}
