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
    }
    
    /// <summary>
    /// Used to send information on notification volume changes
    /// </summary>
    public void NotificationChange()
    {
        sender.SendData("Notification," + notificationVolume);
    }

    /// <summary>
    /// Used to send information on effect volume changes
    /// </summary>
    public void EffectChange()
    {
        sender.SendData("Effects," + effectsVolume);
    }

    /// <summary>
    /// Used to send information on music volume changes
    /// </summary>
    public void MusicChange()
    {
        sender.SendData("Music," + musicVolume);
    }
}
