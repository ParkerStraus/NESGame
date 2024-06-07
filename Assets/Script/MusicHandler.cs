using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MusicHandler : MonoBehaviour
{
    public bool StartMusicNow;
    public bool ResetFilters;
    public AudioSource musicSource;
    public AudioClip musicStart;


    // Start is called before the first frame update
    void Start()
    {
        if (StartMusicNow)
        {
            StartMusic();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartMusic()
    {
        if (musicStart != null)
        {
            musicSource.PlayOneShot(musicStart);
            musicSource.PlayScheduled(AudioSettings.dspTime + musicStart.length);
        }
        else
        {
            musicSource.Play();
        }
    }

    public void QueueNewSong(AudioClip songStart, AudioClip songLoop)
    {
        if (musicSource.isPlaying)
        {
            musicSource.Stop();

        }

        musicSource.clip = songLoop;
        if (songStart != null)
        {
            musicSource.PlayOneShot(songStart);
            musicSource.PlayScheduled(AudioSettings.dspTime + songStart.length);
        }
        else
        {
            musicSource.Play();
        }
    }

    public void StopMusic()
    {
        musicSource.Stop();
        musicSource.enabled = false;
    }

   
}
