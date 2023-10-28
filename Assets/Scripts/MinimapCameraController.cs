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

        float mapCameraRotation = (360f - player.rotation.eulerAngles.y) % 360f;

        transform.rotation = Quaternion.Euler(90f, 0f, mapCameraRotation);

        if (PauseMenu.GameIsPaused)
            transform.GetComponent<Camera>().orthographicSize = 180f;
        else
            transform.GetComponent<Camera>().orthographicSize = 120f;
    }
}
