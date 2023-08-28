using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicControl : MonoBehaviour
{
    public static MusicControl instance;
    private AudioSource _audioSource;
    GameState state;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
        DontDestroyOnLoad(this.gameObject);
        _audioSource = GetComponent<AudioSource>();
    }
    private void Start()
    {

    }

    public void PlayMusic(AudioClip musicClip)
    {
        StopMusic();
        //if (_audioSource.isPlaying) return; 
        _audioSource.PlayOneShot(musicClip);
    }

    public void StopMusic()
    {
        _audioSource.Stop();
    }
}
