using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointSingleTrigger : MonoBehaviour
{
    [SerializeField ]private TrackCheckPoints trackCheckPoints;
    BoxCollider triggerCollider;
    [SerializeField] private GameObject[] enable;
    [SerializeField] private GameObject[] disable;

    private void Awake()
    {
        triggerCollider = GetComponent<BoxCollider>();  
    }

    private void Start(){
        trackCheckPoints.EnableFirstCheckpoint(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<CarMovement>(out CarMovement player))
        {
            Debug.Log(this.name);

            trackCheckPoints.PlayerThroughCheckpoint(this);
            foreach(GameObject obj in enable)
            {
                if (obj != null)
                {
                    obj.SetActive(true);
                    Debug.Log(obj.name);
                }
            }

            foreach(GameObject obj in disable)
            {
                if (obj != null)
                {
                    obj.SetActive(false);
                    Debug.Log(obj.name);
                }
            }
        }
    }

    public void SetTrackCheckpoints(TrackCheckPoints trackCheckPoints)
    {
        this.trackCheckPoints = trackCheckPoints;
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
