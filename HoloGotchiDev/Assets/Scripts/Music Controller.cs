using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    [SerializeField] private AudioClip musicTrack;
    [SerializeField] private AudioClip[] ambiantSounds;
    [SerializeField] private AudioSource audioControl;
    private float timeBeforeNextPlay;
    private float timeBeforeRandomSound;
    private bool playingMusic;
    // Start is called before the first frame update
    void Start()
    {
        timeBeforeNextPlay = 0;
        playingMusic = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(timeBeforeNextPlay <= 0)
        {
            audioControl.clip = musicTrack;
            audioControl.Play();
            timeBeforeNextPlay = 20 * 60;
            playingMusic = true;
        }
        if(timeBeforeNextPlay <= 15 * 60)
        {
            playingMusic = false;
        }

        if (!playingMusic)
        {
            if (timeBeforeRandomSound <= 0)
            {
                audioControl.clip = ambiantSounds[Random.Range(0, ambiantSounds.Length)];
                audioControl.Play();
                timeBeforeRandomSound = Random.Range(30, 121);
            }
        }

        timeBeforeNextPlay -= Time.deltaTime;
    }
}
