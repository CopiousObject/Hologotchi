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
        }
    }

    // Update is called once per frame
    void Update()
    {
        while (statUpdateQueue.TryDequeue(out var update))
        {
            string stat = update.type;
            float value = update.value;

            switch (stat)
            {
                case "Thirst":
                    sliders[0].value = value;
                    break;

                case "Hunger":
                    sliders[1].value = value;
                    break;

                case "Play":
                    sliders[2].value = value;
                    break;

                case "Chat":
                    sliders[3].value = value;
                    break;

                case "Dirtiness":
                    sliders[4].value = value;
                    break;
            }
        }

        // - Color change as values get lower
        ColorChange();
    }

    /// <summary>
    /// Reads the messsage that is provided to the IPC Receiver and calls the stat updater
    /// </summary>
    /// <param name="message"></param>
    public void ReceiveMessage(string message)
    {
        //Debug.Log("Received IPC message: " + message);
        try
        {
            string[] splitMessage = message.Split(',');
            if (float.TryParse(splitMessage[1], out float value))
            {
                if (message.Contains("Thirst")) statUpdateQueue.Enqueue(("Thirst", value));
                if (message.Contains("Hunger")) statUpdateQueue.Enqueue(("Hunger", value));
                if (message.Contains("Play")) statUpdateQueue.Enqueue(("Play", value));
                if (message.Contains("Chat")) statUpdateQueue.Enqueue(("Chat", value));
                if (message.Contains("Dirtiness")) statUpdateQueue.Enqueue(("Dirtiness", value));
            }
        }
        catch (IndexOutOfRangeException ioor)
        {
            Debug.LogWarning("Not readable message: " + ioor);
        }
        catch(NullReferenceException nre)
        {
            Debug.LogWarning("Not readable message: " + nre);
        }
    }

    private void ColorChange()
    {
        for (int i = 0; i < sliders.Count; i++)
        {
            // switch to red
            if (sliders[i].value <= .33f)
            {
                fillImages[i].color = new Color(0.666f, 0.498f, 0.407f);
            }
            // switch to yellow
            else if (sliders[i].value <= .66f)
            {
                fillImages[i].color = new Color(0.666f, 0.666f, 0.407f);
            }
            // stay/return to green
            else
            {
                fillImages[i].color = new Color(0.498f, 0.666f, 0.407f);
            }
        }
    }
}
