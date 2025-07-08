using LookingGlass;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;
using System.Collections.Concurrent;

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
    private ConcurrentQueue<(string type, float value)> statUpdateQueue = new ConcurrentQueue<(string, float)>();

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

        while (statUpdateQueue.TryDequeue(out var update))
        {
            string stat = update.type;
            float value = update.value;

            Debug.Log($"[MAIN] UI update for {update.type} = {update.value}");

            switch (stat)
            {
                case "Thirst":
                    sliders[0].value = value;
                    break;

                case "Hunger":
                    sliders[1].value = value;
                    break;

                case "Dirtiness":
                    sliders[4].value = 1 - (value / 100);
                    break;
            }
        }

        //if (updateThirst)
        //{
        //    float value;
        //    bool success = float.TryParse(splitMessage[1], out value);
        //    sliders[0].value = 1 - value;
        //    updateThirst = false;
        //}

        //// Update Hunger Statbar
        //if (updateHunger)
        //{
        //    float value;
        //    bool success = float.TryParse(splitMessage[1], out value);
        //    sliders[1].value = 1 - value;
        //    updateHunger = false;
        //}

        //// Use for later updating play and social
        //if (updatePlay)
        //{
        //    updatePlay = false;
        //}
        //if (updateSocial)
        //{
        //    updateSocial = false;
        //}

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
        string[] splitMessage = message.Split(',');
        if(float.TryParse(splitMessage[1], out float value))
        {
            if (message.Contains("Thirst")) statUpdateQueue.Enqueue(("Thirst", value));
            if (message.Contains("Hunger")) statUpdateQueue.Enqueue(("Hunger", value));
            if (message.Contains("Dirtiness")) statUpdateQueue.Enqueue(("Dirtiness", value));
        }
        //if (message.Contains("Thirst"))
        //{
        //    updateThirst = true;
        //}
        //if (message.Contains("Hunger"))
        //{
        //    updateHunger = true;
        //}
        //if (message.Contains("Play"))
        //{
        //    updatePlay = true;
        //}
        //if (message.Contains("Social"))
        //{
        //    updateSocial = true;
        //}
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
