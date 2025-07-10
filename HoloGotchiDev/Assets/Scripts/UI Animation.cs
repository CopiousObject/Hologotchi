using System;
using System.Collections;
using System.Collections.Generic;
using LookingGlass;
using TMPro;
using UnityEngine;

public class UIAnimation : MonoBehaviour
{
    [SerializeField]
    private InterProcessCommunicator communicator;

    private float time = 0.0f;

    // Animation booleans for turning on and off specific parts of the animation
    private bool isAnimating = false;
    private bool animatePos = false;
    private bool animateScale = false;

    // Position placeholder variables
    private RectTransform rectTransform;
    private Vector3 start;
    private Vector3 end;

    //private Coroutine currentAnimation;

    // Placements for movement animation
    private Vector3 playScreen = new Vector3(73, -17, 0);
    private Vector3 ballScreen = new Vector3(2525, -17, 0);
    private Vector3 statsScreen = new Vector3(73, 2670, 0);
    private Vector3 settingsScreen = new Vector3(73, -2635, 0);
    private Vector3 notifScreen = new Vector3(1975, -2635, 0);
    private Vector3 audioScreen = new Vector3(-1920, -2635, 0);
    private Vector3 endScale = new Vector3(0, 0, 0);

    // Title screen scale Animation fields
    [SerializeField] private RectTransform titleScreen;
    private Vector3 titleScale;

    private void Awake()
    {
        // Get the RectTransform and initial scale for animation later
        rectTransform = GetComponent<RectTransform>();
        titleScale = titleScreen.localScale;

        communicator.OnMessageReceived += ReceiveMessage;
    }

    private void ReceiveMessage(string message)
    {
        if (message == "Picked up ball") NavigateToBall();
    }

    private void Update()
    {
        // Check for animation
        if (isAnimating)
        {
            time += Time.deltaTime;
            float eased = EaseOut(time);

            // do only position animation
            if (animatePos)
            {
                rectTransform.anchoredPosition = Vector3.Lerp(start, end, eased);
            }

            // Do only scaling animation
            if (animateScale)
            {
                titleScreen.localScale = Vector3.Lerp(titleScale, endScale, eased);
            }

            // end the animation
            if (time >= 1f)
            {
                isAnimating = false;
                animatePos = false;
                animateScale = false;
            }
        }
    }

    /// <summary>
    /// Animate the position of the UI
    /// </summary>
    /// <param name="destination"></param>
    private void Animate(Vector3 destination)
    {
        start = rectTransform.anchoredPosition;
        end = destination;
        time = 0.0f;
        isAnimating = true;
        animatePos = true;
    }

    // Functions to call in order to properly animate Position
    public void NavigateToPlay() => Animate(playScreen);
    public void NavigateToStats() => Animate(statsScreen);
    public void NavigateToSettings() => Animate(settingsScreen);
    public void NavigateToNotifs() => Animate(notifScreen);
    public void NavigateToAudio() => Animate(audioScreen);
    public void NavigateToBall() => Animate(ballScreen);

    // Called to affect scaling animation
    public void TitleToPlay()
    {
        time = 0.0f;
        isAnimating = true;
        animateScale = true;
    }

    // Makes the animation smoother
    private float EaseOut(float x)
    {
        return 1f - Mathf.Pow(1f - x, 4f);
    }
}
