using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMusicAudio : MonoBehaviour
{
    [SerializeField] public AudioSource source;
    [SerializeField] public AudioClip[] Music; 
    private int current = 0;

    // Start is called before the first frame update
    void Start()
    {
        source.clip = Music[current];
        source.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (!source.isPlaying)
        {
            current = (current + 1) % Music.Length;
            source.clip = Music[current];
            source.Play();
        }


        source.volume = PauseMenu.GameIsPaused ? UserSettings.Instance.MenuMusicVolume : UserSettings.Instance.GameMusicVolume;
    }
}
