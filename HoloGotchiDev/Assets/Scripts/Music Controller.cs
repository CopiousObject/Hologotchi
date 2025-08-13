using LookingGlass;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MusicController : MonoBehaviour
{
    [SerializeField] private ValholoIPC receiver;

    [SerializeField] private AudioClip musicTrack;
    [SerializeField] private AudioClip[] ambiantSounds;
    [SerializeField] private AudioSource audioControl;
    private float timeBeforeNextPlay;
    private float timeBeforeRandomSound;
    private bool playingMusic;

    private float musicVolume = 1;
    private float effectsVolume = 1;
    // Start is called before the first frame update
    void Start()
    {
        timeBeforeNextPlay = 0;
        playingMusic = true;
        receiver.OnHandleMessage += HandleMessage;
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
            audioControl.volume = musicVolume;
        }

        if (!playingMusic)
        {
            if (timeBeforeRandomSound <= 0)
            {
                audioControl.clip = ambiantSounds[Random.Range(0, ambiantSounds.Length)];
                audioControl.Play();
                timeBeforeRandomSound = Random.Range(30, 121);
            }
            audioControl.volume = effectsVolume;
            timeBeforeRandomSound -= Time.deltaTime;
        }

        timeBeforeNextPlay -= Time.deltaTime;
    }

    void HandleMessage(IPCMessageId id, string message)
    {
        if (id == IPCMessageId.ValueSetting)
        {
            var args = message.Split(',');

            var name = args[0];
            var value = float.Parse(args[1]);

            if (name == "Music")
            {
                audioControl.volume = value;
                musicVolume = value;
            }
            else if (name == "Effects")
            {
                effectsVolume = value;
            }
        }
    }
}
