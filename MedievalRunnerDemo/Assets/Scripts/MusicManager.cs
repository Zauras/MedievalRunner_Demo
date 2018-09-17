using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;


    private AudioSource audioSource;
    //private bool playing;

    [SerializeField]
    private Button musicButton;

    [SerializeField]
    private Sprite musicOn, musicOff;

    void Awake()
    {
        if (instance == null) instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        CheckIfMusicIsOnOrOff();
    }


    public void PlayMusic(bool play)
    {
        if (play & !audioSource.isPlaying){
            audioSource.Play();
        }
        else if (audioSource.isPlaying){
            audioSource.Stop();
        }
    }

    void CheckIfMusicIsOnOrOff()
    {
        if (GamePreferences.GetMusicState() == 0){
            if (instance != null)
            {
                instance.PlayMusic(false);
            }
            musicButton.image.sprite = musicOff;
        }
        else
        {
            if (instance != null)
            {
                instance.PlayMusic(true);
                musicButton.image.sprite = musicOn;
            }
        }
    }

    public void TurnMusicOnOrOff()
    {
        if (GamePreferences.GetMusicState() == 0){
            GamePreferences.SetMusicState(1); // Music is ON
            if (instance != null){
                instance.PlayMusic(true); // Play Music
            }
            musicButton.image.sprite = musicOn;
        }
        else{
            GamePreferences.SetMusicState(0); // Music is Off
            if (instance != null){
                instance.PlayMusic(false); // Stop Play Music
            }
            musicButton.image.sprite = musicOff;
        }
    }

}