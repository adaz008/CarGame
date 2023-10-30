using UnityEngine;
using UnityEngine.UI;

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
        setActiveCanvas(audioSettingsCanvas, false);
    }

    public void OpenPlayerSettings()
    {
        setActiveCanvas(playerSettingsCanvas, false);
    }

    public void OpenControlsSettings()
    {
        setActiveCanvas(controlsSettingsCanvas, false);
    }

    public void OpenSettingsMenu()
    {
        setActiveCanvas(settingsMenuCanvas, true);
    }

    public void setActiveCanvas(GameObject current, bool isMenunavigatorEnabled)
    {
        settingsMenuCanvas.SetActive(true);
        audioSettingsCanvas.SetActive(audioSettingsCanvas == current);
        playerSettingsCanvas.SetActive(playerSettingsCanvas == current);
        controlsSettingsCanvas.SetActive(controlsSettingsCanvas == current);

        settingsMenuNavigator.enabled = isMenunavigatorEnabled;
    }
}
