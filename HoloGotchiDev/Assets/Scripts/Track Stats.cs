using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrackStats : MonoBehaviour
{
    // Access to each of the sliders for manipulation
    [SerializeField]
    private GameObject thirstSlider;
    [SerializeField]
    private GameObject hungerSlider;
    [SerializeField]
    private GameObject playSlider;
    [SerializeField]
    private GameObject socialSlider;
    [SerializeField]
    private GameObject cleanSlider;

    // Max value for each slider
    private int maxValue = 1;

    // List of all sliders and corressponding parts for ease of looping and manipulating
    private List<Slider> sliders = new List<Slider>();
    private List<Image> fillImages = new List<Image>();
    private List<Image> backgroundImages = new List<Image>();

    // Get access to the Holopal for values of stats
    [SerializeField] private HoloPal holopal;

    // Start is called before the first frame update
    void Start()
    {
        // Add each slider component to the list
        sliders.Add(thirstSlider.GetComponent<Slider>());
        sliders.Add(hungerSlider.GetComponent<Slider>());
        sliders.Add(playSlider.GetComponent<Slider>());
        sliders.Add(socialSlider.GetComponent<Slider>());
        sliders.Add(cleanSlider.GetComponent<Slider>());

        // Add the fill and background Images for each slider into their respective list
        foreach(Slider slider in sliders)
        {
            fillImages.Add(slider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>());
            backgroundImages.Add(slider.gameObject.transform.Find("Background").GetComponent<Image>());
        }

        // Test Values
        sliders[0].value = .35f;
        sliders[1].value = .12f;
        sliders[2].value = .56f;
        sliders[3].value = .80f;
        sliders[4].value = .75f;

        // Used to get the starting values based upon some type of file maybe for 
        // keeping continuity between loading the experience
    }

    // Update is called once per frame
    void Update()
    {
        // TEMP CODE: Just my thought process on how it would theoretically work
        // 
        // Alter each of the sliders to equate what it is compared to the other state scripts
        //sliders[0].value -= 0f;
        //sliders[1].value -= 0f;
        // sliders[2].value -= 0f;
        // sliders[3].value -= 0f;
        //sliders[4].value -= 0f;

        // Things to figure out
        // - where to get the values for each of the stats
        // - How to get them into this script for use
        // - Calculate the change between them (Should be easy as it is just taking the current and subtracting the read in value)
        // - how to do this for each stat at once within the update method.

        // - Color change as values get lower
        // ColorChange();
    }

    private void ColorChange()
    {
        for (int i = 0; i < sliders.Count; i++)
        {
            // switch to yellow
            if (sliders[i].value <= .66f)
            {
                fillImages[i].color = new Color(170, 170, 104, 255);
                backgroundImages[i].color = new Color(113, 113, 64, 255);
            }
            // switch to red
            else if (sliders[i].value <= .45f)
            {
                fillImages[i].color = new Color(170, 127, 104, 255);
                backgroundImages[i].color = new Color(113, 81, 64, 255);
            }
            // stay/return to green
            else
            {
                fillImages[i].color = new Color(127, 170, 104, 255);
                backgroundImages[i].color = new Color(81, 113, 64, 255);
            }
        }
    }
}
