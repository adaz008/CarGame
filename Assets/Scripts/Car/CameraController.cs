using System.ComponentModel;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    private Rigidbody playerRB;
    private CarMovement carMovement;

    [Header("Distance")]
    [SerializeField] private float Distance_Offset_Close;
    [SerializeField] private float Distance_Offset_Far;

    [Header("Height")]
    [SerializeField] private float Height_Offset;

    [Header("Camera change speed")]
    [SerializeField] private float speed;

    [Header("OffsetVectors")]
    [SerializeField] private Vector3 hood;
    [SerializeField] private Vector3 bumper;
    [SerializeField] private Vector3 inside;

    private void Start()
    {
        playerRB = player.GetComponent<Rigidbody>();
        carMovement = player.GetComponent<CarMovement>();
    }

    private void LateUpdate()
    {
        Vector3 playerForward = (-playerRB.velocity - player.transform.forward).normalized;

        if (UserSettings.Instance.Camera == "Hood")
            SetCameraPosition(hood);
        else if (UserSettings.Instance.Camera == "Bumper")
            SetCameraPosition(bumper);
        else if (UserSettings.Instance.Camera == "Inside")
            SetCameraPosition(inside);

        float distanceOffSet = UserSettings.Instance.Camera == "Close" ? Distance_Offset_Close : Distance_Offset_Far;

        if (carMovement.LookBack && (UserSettings.Instance.Camera == "Close" || UserSettings.Instance.Camera == "Far"))
        {
            Vector3 forward = (playerForward.x > 0 && playerForward.z > 0) ? -playerForward : playerForward;
            SetCameraPositionFarClose(forward, distanceOffSet);
        }
        else if (UserSettings.Instance.Camera == "Close" || UserSettings.Instance.Camera == "Far")
        {
            SetCameraPositionFarClose(playerForward, distanceOffSet);
        }

        HandleCameraFieldOfView();
    }

    private void SetCameraPosition(Vector3 position)
    {
        transform.position = player.TransformPoint(position);
    }

    private void SetCameraPositionFarClose(Vector3 playerForward, float distance)
    {
        Vector3 targetPosition = player.position + Height_Offset * Vector3.up - player.forward * distance;

        if (UserSettings.Instance.ChangeCameraReverse)
            transform.position = Vector3.Lerp(transform.position,
                player.position + Height_Offset * Vector3.up + playerForward * distance,
                speed * Time.deltaTime);
        else
        {
            if (Vector3.Distance(transform.position, targetPosition) > 1)
                transform.position = targetPosition;
            else
                transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);
        }

        transform.LookAt(player);
    }

    private void HandleCameraFieldOfView()
    {
        if ((UserSettings.Instance.Camera == "Hood" || UserSettings.Instance.Camera == "Bumper" || UserSettings.Instance.Camera == "Inside") && PauseMenu.GameIsPaused)
        {
            transform.position = player.position + new Vector3(0f, 3f, 0f);
        }

        if (UserSettings.Instance.Camera == "Close" || UserSettings.Instance.Camera == "Far")
        {
            float fieldViewValue = (NitroEffect.boosting && NitroEffect.nitroValue > 0) ? 100f : 60f;

            GetComponent<Camera>().fieldOfView = Mathf.Lerp(GetComponent<Camera>().fieldOfView, fieldViewValue, Time.deltaTime);
        }
    }
}
