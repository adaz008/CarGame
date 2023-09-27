using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    private Rigidbody playerRB;
    [Header("Distance")]
    [SerializeField] private float Distance_Offset_Close;
    [SerializeField] private float Distance_Offset_Far;
    [Header("Height")]
    [SerializeField] private float Height_Offset_Close;
    [SerializeField] private float Height_Offset_Far;
    [SerializeField] private float speed;
    private CarMovement carMovement;

    // Start is called before the first frame update
    void Start()
    {
        playerRB = player.GetComponent<Rigidbody>();
        carMovement = player.GetComponent<CarMovement>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 playerForward = (-playerRB.velocity - player.transform.forward).normalized;

        Vector3 hood = new Vector3(-1.469f, 0.393f, -0.008f);
        Vector3 bumper = new Vector3(-2.4f, -0.02f, -0.008f);
        Vector3 back = new Vector3(-8f, 2f, -0.008f);

        if (carMovement.LookBack)
        {
            transform.position = player.position - playerForward * 8f;
            if (UserSettings.Instance.Camera == "Close" && carMovement.GasInput >= 0)
                transform.position = player.position - playerForward * Distance_Offset_Close + Vector3.up * Height_Offset_Close;
            if (UserSettings.Instance.Camera == "Close" && carMovement.GasInput < 0)
                transform.position = player.position + playerForward * Distance_Offset_Close + Vector3.up * Height_Offset_Close;
            if (UserSettings.Instance.Camera == "Far" && carMovement.GasInput >= 0)
                transform.position = player.position - playerForward * Distance_Offset_Far + Vector3.up * Height_Offset_Far;
            if (UserSettings.Instance.Camera == "Far" && carMovement.GasInput < 0)
                transform.position = player.position + playerForward * Distance_Offset_Far + Vector3.up * Height_Offset_Far;
            transform.LookAt(player);
        }
        else
        {
            if (UserSettings.Instance.Camera == "Hood")
                transform.position = player.position + hood;
            else if (UserSettings.Instance.Camera == "Bumper")
                transform.position = player.position + bumper;
            else if (UserSettings.Instance.Camera == "Far" && !UserSettings.Instance.ChangeCameraReverse)
            {
                Vector3 targetPosition = player.position + Height_Offset_Far * Vector3.up - player.forward * Distance_Offset_Far;

                if (Vector3.Distance(transform.position, targetPosition) > 1)
                    transform.position = targetPosition;
                else
                    transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);

                transform.LookAt(player);
            }
            else if (UserSettings.Instance.Camera == "Close" && !UserSettings.Instance.ChangeCameraReverse)
            {
                Vector3 targetPosition = player.position + Height_Offset_Close * Vector3.up - player.forward * Distance_Offset_Close;

                if (Vector3.Distance(transform.position, targetPosition) > 1)
                    transform.position = targetPosition;
                else
                    transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);

                transform.LookAt(player);
            }
            else if (UserSettings.Instance.Camera == "Far" && UserSettings.Instance.ChangeCameraReverse)
            {
                transform.position = Vector3.Lerp(transform.position,
                player.position + Height_Offset_Far * Vector3.up
                + playerForward * Distance_Offset_Far,
                speed * Time.deltaTime);

                transform.LookAt(player);
            }
            else if (UserSettings.Instance.Camera == "Close" && UserSettings.Instance.ChangeCameraReverse)
            {
                transform.position = Vector3.Lerp(transform.position,
                player.position + Height_Offset_Close * Vector3.up
                + playerForward * Distance_Offset_Close,
                speed * Time.deltaTime);

                transform.LookAt(player);
            }
        }

        if (UserSettings.Instance.Camera == "Hood" && PauseMenu.GameIsPaused)
            transform.position += new Vector3(0f, 1f, 0f);

        if (NitroEffect.boosting && NitroEffect.nitroValue > 0)
            GetComponent<Camera>().fieldOfView = Mathf.Lerp(GetComponent<Camera>().fieldOfView, 100, Time.deltaTime);
        else
            GetComponent<Camera>().fieldOfView = Mathf.Lerp(GetComponent<Camera>().fieldOfView, 60, Time.deltaTime);


    }
}