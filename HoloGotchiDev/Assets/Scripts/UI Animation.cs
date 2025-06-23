using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIAnimation : MonoBehaviour
{
    float time = 0.0f;
    private RectTransform rectTransform;

    //private Coroutine currentAnimation;

    // Placements
    private Vector3 playScreen = new Vector3(73, -17, 0);
    private Vector3 statsScreen = new Vector3(73, 2670, 0);
    private Vector3 settingsScreen = new Vector3(73, -2635, 0);
    private Vector3 notifScreen = new Vector3(1975, -2635, 0);
    private Vector3 audioScreen = new Vector3(-1920, -2635, 0);

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    /// <summary>
    /// Navigates the UI to the Main Play Screen
    /// </summary>
    public void NavigateToPlay()
    {
        while(time <= 1.0f)
        {
            time += Time.deltaTime;
            rectTransform.anchoredPosition = Vector3.Lerp(rectTransform.anchoredPosition, playScreen, EaseOut(time));
        }
        time = 0.0f;
    }

    /// <summary>
    /// Navigates the UI to the Statistics Screen
    /// </summary>
    public void NavigateToStats()
    {
        //if (currentAnimation != null)
        //    StopCoroutine(currentAnimation);

        //currentAnimation = StartCoroutine(AnimateToPosition(statsScreen, 1.0f));

        while (time <= 1.0f)
        {
            time += Time.deltaTime;
            rectTransform.anchoredPosition = Vector3.Lerp(rectTransform.anchoredPosition, statsScreen, EaseOut(time));
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
            rectTransform.anchoredPosition = Vector3.Lerp(rectTransform.anchoredPosition, settingsScreen, EaseOut(time));
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
            rectTransform.anchoredPosition = Vector3.Lerp(rectTransform.anchoredPosition, notifScreen, EaseOut(time));
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
            rectTransform.anchoredPosition = Vector3.Lerp(rectTransform.anchoredPosition, audioScreen, EaseOut(time));
        }
        time = 0.0f;
    }

    //private IEnumerator AnimateToPosition(Vector3 target, float duration)
    //{
    //    Vector3 start = rectTransform.anchoredPosition;
    //    float time = 0f;

    //    while (time < duration)
    //    {
    //        time += Time.deltaTime;
    //        float t = Mathf.Clamp01(time / duration);
    //        float easedT = EaseOut(t);
    //        rectTransform.anchoredPosition = Vector3.LerpUnclamped(start, target, easedT);
    //        yield return null;
    //    }

    //    rectTransform.anchoredPosition = target;
    //}

    private float EaseOut(float x)
    {
        return 1f - Mathf.Pow(1f - x, 4f);
    }
}
