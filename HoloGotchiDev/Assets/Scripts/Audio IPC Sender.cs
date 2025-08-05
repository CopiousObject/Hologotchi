using LookingGlass;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioIPCSender : MonoBehaviour
{
    [SerializeField] private InterProcessCommunicator sender;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider notificationSlider;
    [SerializeField] private Slider effectsSlider;

    private float masterVolume;
    private float musicVolume;
    private float notificationVolume;
    private float effectsVolume;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        masterVolume = masterSlider.value;

        musicVolume = musicSlider.value * masterVolume;
        notificationVolume = notificationSlider.value * masterVolume;
        effectsVolume = effectsSlider.value * masterVolume;

        sender.SendData("Music," + musicVolume);
        sender.SendData("Notification," + notificationVolume);
        sender.SendData("Effects," + effectsVolume);
    }


}
