using System.ComponentModel;
using Unity.VisualScripting;
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

    private string currentCamera;

    private void Start()
    {
        playerRB = player.GetComponent<Rigidbody>();
        carMovement = player.GetComponent<CarMovement>();
        currentCamera = UserSettings.Instance.Camera;
    }

    private void LateUpdate()
    {
        Vector3 playerForward = (-playerRB.velocity - player.transform.forward).normalized;

        bool isCameraChanged = UserSettings.Instance.Camera != currentCamera;

        if (UserSettings.Instance.Camera == "Hood")
            SetCameraPosition(hood, isCameraChanged);
        else if (UserSettings.Instance.Camera == "Bumper")
            SetCameraPosition(bumper, isCameraChanged);
        else if (UserSettings.Instance.Camera == "Inside")
            SetCameraPosition(inside, isCameraChanged);

        currentCamera = UserSettings.Instance.Camera;

        float distanceOffSet = UserSettings.Instance.Camera == "Close" ? Distance_Offset_Close : Distance_Offset_Far;

        if (UserSettings.Instance.Camera == "Close" || UserSettings.Instance.Camera == "Far")
        {
            if (!carMovement.LookBack)
            {
                SetCameraPositionFarClose(playerForward, distanceOffSet);
            }else
            {
                //Vector3 forward = (playerForward.x > 0 && playerForward.z > 0) ? -playerForward : playerForward;
                //Debug.Log("PlayerForward: " + playerForward);
                //Debug.Log("Forward: " + forward);
                SetCameraPositionFarClose(-playerForward, distanceOffSet);
            }
        }

        HandleCameraFieldOfView();
    }

    private void SetCameraPosition(Vector3 position, bool isChanged)
    {
        //Camera rotation helyreallitas
        if (isChanged)
        {
            Vector3 targetPosition = player.position - player.forward * Distance_Offset_Close;
            transform.position = targetPosition;
            transform.LookAt(player);
        }

        transform.position = player.TransformPoint(position);
    }

    private void SetCameraPositionFarClose(Vector3 playerForward, float distance)
    {
        Vector3 targetPosition = player.position + Height_Offset * Vector3.up - player.forward * distance;
        Vector3 backwardTargetPosition = player.position + Height_Offset * Vector3.up + player.forward * distance;

        if (carMovement.LookBack)
        {
            if (Vector3.Distance(transform.position, backwardTargetPosition) > 1)
                transform.position = backwardTargetPosition;
            else
                transform.position = Vector3.Lerp(transform.position, backwardTargetPosition, speed * Time.deltaTime);
        }
        else if (!UserSettings.Instance.ChangeCameraReverse)
        {
            if (Vector3.Distance(transform.position, targetPosition) > 1)
                transform.position = targetPosition;
            else
                transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);
        }
        else if (UserSettings.Instance.ChangeCameraReverse)
        {
            transform.position = Vector3.Lerp(transform.position,
                player.position + Height_Offset * Vector3.up + playerForward * distance,
                speed * Time.deltaTime);
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
