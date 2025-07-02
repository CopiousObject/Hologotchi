using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrackStats : MonoBehaviour
{
    // Access to each of the sliders for manipulation
    [SerializeField]
    private Slider thirstSlider;
    [SerializeField]
    private Slider hungerSlider;
    [SerializeField]
    private Slider playSlider;
    [SerializeField]
    private Slider socialSlider;
    [SerializeField]
    private Slider cleanSlider;

    // Max value for each slider
    private int maxValue = 1;

    // Start is called before the first frame update
    void Start()
    {
        // Used to get the starting values based upon some type of file maybe for 
        // keeping continuity between loading the experience
    }

    // Update is called once per frame
    void Update()
    {
        // TEMP CODE: Just my thought process on how it would theoretically work
        // 
        // Alter each of the sliders to equate what it is compared to the other state scripts
        // thirstSlider.value -= change in thirst stat
        // hungerSlider.value -= change in hunger stat
        // playSlider.value -= change in play stat
        // socialSlider.value -= change in social stat
        // cleanSlider.value -= change in clean stat

        // Things to figure out
        // - where to get the values for each of the stats
        // - How to get them into this script for use
        // - Calculate the change between them (Should be easy as it is just taking the current and subtracting the read in value)
        // - how to do this for each stat at once within the update method.
    }
}
