using UnityEngine;

public class SettingsUIManager : MonoBehaviour
{
    [Header("Canvases")]
    [SerializeField] private GameObject settingsMenuCanvas;
    [SerializeField] private GameObject audioSettingsCanvas;
    [SerializeField] private GameObject playerSettingsCanvas;
    [SerializeField] private GameObject controlsSettingsCanvas;

    private MenuNavigatorBase settingsMenuNavigator;
    private AudioMenuNavigator audioSettingsNavigator;
    private PlayerMenuNavigator playerSettingsNavigator;
    private ControlsMenuNavigator controlsSettingsNavigator;

    private void Start()
    {
        // Get references to the individual MenuNavigator scripts
        settingsMenuNavigator = settingsMenuCanvas.GetComponent<MenuNavigatorBase>();
        audioSettingsNavigator = audioSettingsCanvas.GetComponent<AudioMenuNavigator>();
        playerSettingsNavigator = playerSettingsCanvas.GetComponent<PlayerMenuNavigator>();
        controlsSettingsNavigator = controlsSettingsCanvas.GetComponent<ControlsMenuNavigator>();

        // Deactivate all but the SettingsMenu
        audioSettingsCanvas.SetActive(false);
        playerSettingsCanvas.SetActive(false);
        controlsSettingsCanvas.SetActive(false);
    }

    public void OpenAudioSettings()
    {
        settingsMenuCanvas.SetActive(true);
        audioSettingsCanvas.SetActive(true);
        playerSettingsCanvas.SetActive(false);
        controlsSettingsCanvas.SetActive(false);

        settingsMenuNavigator.enabled = false;
    }

    public void OpenPlayerSettings()
    {
        settingsMenuCanvas.SetActive(true);
        audioSettingsCanvas.SetActive(false);
        playerSettingsCanvas.SetActive(true);
        controlsSettingsCanvas.SetActive(false);

        settingsMenuNavigator.enabled = false;
    }

    public void OpenControlsSettings()
    {
        settingsMenuCanvas.SetActive(true);
        audioSettingsCanvas.SetActive(false);
        playerSettingsCanvas.SetActive(false);
        controlsSettingsCanvas.SetActive(true);

        settingsMenuNavigator.enabled = false;
    }

    public void OpenSettingsMenu()
    {
        settingsMenuCanvas.SetActive(true);
        audioSettingsCanvas.SetActive(false);
        playerSettingsCanvas.SetActive(false);
        controlsSettingsCanvas.SetActive(false);

        settingsMenuNavigator.enabled = true;
    }
}
