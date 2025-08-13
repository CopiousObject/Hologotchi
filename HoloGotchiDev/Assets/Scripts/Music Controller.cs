using LookingGlass;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MusicController : MonoBehaviour
{
    [SerializeField] private ValholoIPC receiver;

    [SerializeField] private AudioClip[] ambiantSounds;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource soundEffectSource;
    private float timeBeforeRandomSound;

    // Start is called before the first frame update
    void Start()
    {
        receiver.OnHandleMessage += HandleMessage;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeBeforeRandomSound <= 0)
        {
            soundEffectSource.PlayOneShot(ambiantSounds[Random.Range(0, ambiantSounds.Length)]);
            timeBeforeRandomSound = Random.Range(30, 121);
        }

        timeBeforeRandomSound -= Time.deltaTime;
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
                musicSource.volume = value;
            }
            else if (name == "Effects")
            {
                soundEffectSource.volume = value;
            }
        }
    }
}
