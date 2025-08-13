using LookingGlass;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioIPCSender : MonoBehaviour
{
    [SerializeField] private ValholoIPC sender;
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
    public void NotificationChange(float value)
    {
        sender.SendValueSetting("Notification", value);
    }

    /// <summary>
    /// Used to send information on effect volume changes
    /// </summary>
    public void EffectChange(float value)
    {
        sender.SendValueSetting("Effects", value);
    }

    /// <summary>
    /// Used to send information on music volume changes
    /// </summary>
    public void MusicChange(float value)
    {
        sender.SendValueSetting("Music", value);
    }
}
