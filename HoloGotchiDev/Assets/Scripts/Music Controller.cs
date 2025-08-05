using LookingGlass;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MusicController : MonoBehaviour
{
    [SerializeField] private InterProcessCommunicator receiver;

    [SerializeField] private AudioClip musicTrack;
    [SerializeField] private AudioClip[] ambiantSounds;
    [SerializeField] private AudioSource audioControl;
    private float timeBeforeNextPlay;
    private float timeBeforeRandomSound;
    private bool playingMusic;

    private float musicVolume;
    private float effectsVolume;
    // Start is called before the first frame update
    void Start()
    {
        timeBeforeNextPlay = 0;
        playingMusic = true;
        receiver.OnMessageReceived += ReceiveMessage;
    }

    // Update is called once per frame
    void Update()
    {
        if(timeBeforeNextPlay <= 0)
        {
            audioControl.clip = musicTrack;
            audioControl.volume = musicVolume;
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
                audioControl.volume = effectsVolume;
                audioControl.Play();
                timeBeforeRandomSound = Random.Range(30, 121);
            }
        }

        timeBeforeNextPlay -= Time.deltaTime;
    }

    /// <summary>
    /// Recieves the message for the inter processing
    /// </summary>
    /// <param name="message"></param>
    private void ReceiveMessage(string message)
    {
        try
        {
            string[] splitMessage = message.Split(',');
            float value;

            if (message.Contains("Music"))
            {
                float.TryParse(splitMessage[1], out value);
                musicVolume = value;
            }
            if (message.Contains("Effects"))
            {
                float.TryParse(splitMessage[1], out value);
                effectsVolume = value;
            }
        }
        catch
        {

        }
    }
}
