using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISoundManager : MonoBehaviour
{
    [SerializeField] private UIAudioSources sources;

    private void OnEnable()
    {
        PauseMenu.OnBackPressed += PlayCloseSound;
        MenuNavigatorBase.Hover += PlayHoverSound;
        UIManger.OnOpenUI += PlayClickSound;
        ControlsMenuNavigator.OnControlsEditing += PlayClickSound;
    }

    private void OnDisable()
    {
        PauseMenu.OnBackPressed -= PlayCloseSound;
        MenuNavigatorBase.Hover -= PlayHoverSound;
        UIManger.OnOpenUI -= PlayClickSound;
        ControlsMenuNavigator.OnControlsEditing -= PlayClickSound;
    }

    private void PlayCloseSound()
    {
        sources.UIClose.volume = UserSettings.Instance.SoundEffectVolume;
        sources.UIClose.Play();
    }
    private void PlayHoverSound()
    {
        sources.UIHover.volume = UserSettings.Instance.SoundEffectVolume;
        sources.UIHover.Play();
    }

    private void PlayClickSound()
    {
        sources.UIClick.volume = UserSettings.Instance.SoundEffectVolume;
        sources.UIClick.Play();
    }
}

[System.Serializable]
public class UIAudioSources
{
    public AudioSource UIHover;
    public AudioSource UIClick;
    public AudioSource UIClose;
}
