using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    [Header("Menu UI panels")]
    [SerializeField] private GameObject pauseMenuUi;
    [SerializeField] private GameObject SettingsMenuUi;
    [SerializeField] private GameObject AudioMenuUi;
    [SerializeField] private GameObject PlayerMenuUi;
    [SerializeField] private GameObject ControlsMenuUi;
    [SerializeField] private GameObject RaceMenuUi;

    [Header("Other UI elements")]
    [SerializeField] private GameObject blurUI;
    [SerializeField] private GameObject SpeedoMeter;
    [SerializeField] private GameObject Minimap;
    [SerializeField] private GameObject RearViewMirror;

    [Header("Back Buttons")]
    [SerializeField] private Button SettingsBack;
    [SerializeField] private Button AudioBack;
    [SerializeField] private Button PlayerBack;
    [SerializeField] private Button ControlsBack;
    [SerializeField] private Button RaceBack;

    [Header("Selects")]
    [SerializeField] private GameObject enter;
    [SerializeField] private GameObject esc;

    [SerializeField] private ControlsMenuNavigator MenuNavigator;

    private bool isEnabled = true;

    public void SetIsEnabled(bool value) { isEnabled = value; }

    public void SetGameIsPaused(bool value) { GameIsPaused = value;}

    private void Start()
    {
        Minimap.SetActive(UserSettings.Instance.Minimap);
        SpeedoMeter.SetActive(UserSettings.Instance.Gauges);
        RearViewMirror.SetActive(UserSettings.Instance.RearViewMirror);
    }

    // Update is called once per frame
    void Update()
    {
        if (isEnabled)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (GameIsPaused)
                    if (pauseMenuUi.activeInHierarchy)
                        Resume();
                    else
                        MoveBack();
                else
                    Pause();
            }
        }

        if (!GameIsPaused)
        {
            RearViewMirror.SetActive(UserSettings.Instance.RearViewMirror && UserSettings.Instance.Camera != "Inside");
        }
    }

    private void MoveBack()
    {
        if (AudioMenuUi.activeInHierarchy && !MenuNavigator.getIsEditing())
            AudioBack.onClick.Invoke();
        else if (PlayerMenuUi.activeInHierarchy && !MenuNavigator.getIsEditing())
            PlayerBack.onClick.Invoke();
        else if (ControlsMenuUi.activeInHierarchy && !MenuNavigator.getIsEditing())
            ControlsBack.onClick.Invoke();
        else if (RaceMenuUi.activeInHierarchy && !MenuNavigator.getIsEditing())
            RaceBack.onClick.Invoke();
        else if (SettingsMenuUi.activeInHierarchy && !MenuNavigator.getIsEditing())
            SettingsBack.onClick.Invoke();
    }

    public void Resume()
    {
        pauseMenuUi.SetActive(false);

        enter.SetActive(false);
        esc.SetActive(false);

        blurUI.SetActive(false);
        Minimap.SetActive(UserSettings.Instance.Minimap);
        SpeedoMeter.SetActive(UserSettings.Instance.Gauges);
        RearViewMirror.SetActive(UserSettings.Instance.RearViewMirror);

        Time.timeScale = 1f;

        GameIsPaused = false;
    }

    private void Pause()
    {
        pauseMenuUi.SetActive(true);

        enter.SetActive(true);
        esc.SetActive(true);

        blurUI.SetActive(true);
        Minimap.SetActive(false);
        SpeedoMeter.SetActive(false);
        RearViewMirror.SetActive(false);

        Time.timeScale = 0f;

        GameIsPaused = true;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
