using UnityEngine;
using System.Collections;

public class MapSoundTrack : MonoBehaviour
{
    [SerializeField] AudioClip track;

    private void Start()
    {
        StartTrack();
    }

    private void StartTrack()
    {
        if (track != null)
            MusicControl.instance.PlayMusic(track);
    }
}