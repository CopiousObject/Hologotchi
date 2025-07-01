using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIAnimation : MonoBehaviour
{
    private float time = 0.0f;
    private RectTransform rectTransform;
    private bool isAnimating = false;
    private bool animatePos = false;
    private bool animateScale = false;
    private Vector3 start;
    private Vector3 end;
    //private Coroutine currentAnimation;

    // Placements
    private Vector3 playScreen = new Vector3(73, -17, 0);
    private Vector3 statsScreen = new Vector3(73, 2670, 0);
    private Vector3 settingsScreen = new Vector3(73, -2635, 0);
    private Vector3 notifScreen = new Vector3(1975, -2635, 0);
    private Vector3 audioScreen = new Vector3(-1920, -2635, 0);
    private Vector3 endScale = new Vector3(0, 0, 0);

    // Title Screen Rect Transforms
    [SerializeField] private RectTransform titleScreen;

    private Vector3 titleScale;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        titleScale = titleScreen.localScale;
    }

    private void Update()
    {
        if (isAnimating)
        {
            time += Time.deltaTime;
            float eased = EaseOut(time);

            if (animatePos)
            {
                rectTransform.anchoredPosition = Vector3.Lerp(start, end, eased);
            }

            if (animateScale)
            {
                titleScreen.localScale = Vector3.Lerp(titleScale, endScale, eased);
            }

            if (time >= 1f)
            {
                isAnimating = false;
                animatePos = false;
                animateScale = false;
            }
        }
    }


    private void Animate(Vector3 destination)
    {
        start = rectTransform.anchoredPosition;
        end = destination;
        time = 0.0f;
        isAnimating = true;
        animatePos = true;
    }

    public void NavigateToPlay() => Animate(playScreen);
    public void NavigateToStats() => Animate(statsScreen);
    public void NavigateToSettings() => Animate(settingsScreen);
    public void NavigateToNotifs() => Animate(notifScreen);
    public void NavigateToAudio() => Animate(audioScreen);

    public void TitleToPlay()
    {
        time = 0.0f;
        isAnimating = true;
        animateScale = true;
    }

    private float EaseOut(float x)
    {
        return 1f - Mathf.Pow(1f - x, 4f);
    }
}
