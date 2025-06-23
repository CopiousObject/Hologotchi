using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnimation : MonoBehaviour
{
    float time = 0.0f;

    // Placements
    private Vector3 playScreen = new Vector3(73, -17, 0);
    private Vector3 statsScreen = new Vector3(73, 2670, 0);
    private Vector3 settingsScreen = new Vector3(73, -2635, 0);
    private Vector3 notifScreen = new Vector3(1975, -2635, 0);
    private Vector3 audioScreen = new Vector3(-1920, -2635, 0);

    /// <summary>
    /// Navigates the UI to the Main Play Screen
    /// </summary>
    public void NavigateToPlay()
    {
        while(time <= 1.0f)
        {
            time += Time.deltaTime;
            this.GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(this.GetComponent<RectTransform>().anchoredPosition, playScreen, time);
        }
        time = 0.0f;
    }

    /// <summary>
    /// Navigates the UI to the Statistics Screen
    /// </summary>
    public void NavigateToStats()
    {
        while (time <= 1.0f)
        {
            time += Time.deltaTime;
            this.GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(this.GetComponent<RectTransform>().anchoredPosition, statsScreen, time);
        }
        time = 0.0f;
    }

    /// <summary>
    /// Navigates the UI to the Main Settings Screen
    /// </summary>
    public void NavigateToSettings()
    {
        while (time <= 1.0f)
        {
            time += Time.deltaTime;
            this.GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(this.GetComponent<RectTransform>().anchoredPosition, settingsScreen, time);
        }
        time = 0.0f;
    }

    /// <summary>
    /// Navigates the UI to the Notification Settings Screen
    /// </summary>
    public void NavigateToNotifs()
    {
        while (time <= 1.0f)
        {
            time += Time.deltaTime;
            this.GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(this.GetComponent<RectTransform>().anchoredPosition, notifScreen, time);
        }
        time = 0.0f;
    }

    /// <summary>
    /// Navigates the UI to the Audio Settings Screen
    /// </summary>
    void NavigateToAudio()
    {
        while (time <= 1.0f)
        {
            time += Time.deltaTime;
            this.GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(this.GetComponent<RectTransform>().anchoredPosition, audioScreen, time);
        }
        time = 0.0f;
    }
}
