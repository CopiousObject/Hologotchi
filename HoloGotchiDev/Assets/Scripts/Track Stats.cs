using LookingGlass;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

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

    // List of all sliders and corressponding parts for ease of looping and manipulating
    private List<Slider> sliders = new List<Slider>();
    private List<Image> fillImages = new List<Image>();
    private List<Image> backgroundImages = new List<Image>();

    // Reciever for IPC Messages
    [SerializeField] private InterProcessCommunicator receiver;
    private string message;
    private bool updateThirst = false;
    private bool updateHunger = false;
    private bool updatePlay = false;
    private bool updateSocial = false;
    private bool updateDirt = false;

    // Start is called before the first frame update
    void Start()
    {
        // Subscribe to the recieving end
        Debug.Log("Subscribing to IPC message receiver");
        if (receiver != null) receiver.OnMessageReceived += ReceiveMessage;

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
        //sliders[0].value = .35f;
        //sliders[1].value = .12f;
        //sliders[2].value = .56f;
        //sliders[3].value = .80f;
        //sliders[4].value = .75f;

        // Used to get the starting values based upon some type of file maybe for 
        // keeping continuity between loading the experience
    }

    // Update is called once per frame
    void Update()
    {
        // TEMP CODE: Just my thought process on how it would theoretically work

        // Things to figure out
        // - where to get the values for each of the stats
        // - How to get them into this script for use
        // - Calculate the change between them (Should be easy as it is just taking the current and subtracting the read in value)
        // - how to do this for each stat at once within the update method.
        
        if (updateThirst)
        {
            string[] splitMessage = message.Split(',');
            float value;
            bool success = float.TryParse(splitMessage[1], out value);
            value /= 10;
            sliders[0].value = 1 - value;
            updateThirst = false;
        }

        // Update Hunger Statbar
        if (updateHunger)
        {
            string[] splitMessage = message.Split(',');
            float value;
            bool success = float.TryParse(splitMessage[1], out value);
            value /= 10;
            sliders[1].value = 1 - value;
            updateHunger = false;
        }

        // Use for later updating play and social
        if (updatePlay)
        {
            updatePlay = false;
        }
        if (updateSocial)
        {
            updateSocial = false;
        }

        // Update Dirtiness Statbar
        if (updateDirt)
        {
            string[] splitMessage = message.Split(',');
            float value;
            bool success = float.TryParse(splitMessage[1], out value);
            value /= 100;
            sliders[4].value = 1 - value;
            updateDirt = false;
        }

        // - Color change as values get lower
        // ColorChange();
    }

    /// <summary>
    /// Reads the messsage that is provided to the IPC Receiver and calls the stat updater
    /// </summary>
    /// <param name="message"></param>
    public void ReceiveMessage(string message)
    {
        Debug.Log("Received IPC message: " + message);
        if (message.Contains("Thirst"))
        {
            this.message = message;
            updateThirst = true;
        }
        if (message.Contains("Hunger"))
        {
            this.message = message;
            updateHunger = true;
        }
        if (message.Contains("Play"))
        {
            this.message = message;
            updatePlay = true;
        }
        if (message.Contains("Social"))
        {
            this.message = message;
            updateSocial = true;
        }
        if (message.Contains("Dirtiness"))
        {
            this.message = message;
            updateDirt = true;
        }
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
