using UnityEngine;

public class MinimapCameraController : MonoBehaviour
{

    private Transform player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Car").transform;
    }

    private void LateUpdate()
    {
        Vector3 newPosition = player.position;
        newPosition.y = transform.position.y;
        transform.position = newPosition;

        if(PauseMenu.GameIsPaused)
            transform.GetComponent<Camera>().orthographicSize = 180f;
        else
            transform.GetComponent<Camera>().orthographicSize = 120f;
    }
}
