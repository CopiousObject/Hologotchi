using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    [SerializeField] private AudioClip musicTrack;
    [SerializeField] private AudioSource audioControl;
    private float timeBeforeNextPlay;
    // Start is called before the first frame update
    void Start()
    {
        timeBeforeNextPlay = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(timeBeforeNextPlay <= 0)
        {
            audioControl.clip = musicTrack;
            audioControl.Play();
            timeBeforeNextPlay = 20 * 60;
        }

        timeBeforeNextPlay -= Time.deltaTime;
    }
}
