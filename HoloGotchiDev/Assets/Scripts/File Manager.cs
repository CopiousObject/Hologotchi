using LookingGlass;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Enumeration;
using UnityEngine;
using UnityEngine.UI;

public class FileManager : MonoBehaviour
{
    private const string fileName = "Hologotchi Save Data";
    [SerializeField]
    private QuitApplication quitter;
    [SerializeField]
    private InterProcessCommunicator communicator;
    private string growthState;
    private string growthTime;

    [SerializeField]
    private Slider waterSlider;
    [SerializeField]
    private Slider playSlider;
    [SerializeField]
    private Slider chatSlider;
    [SerializeField]
    private Slider foodSlider;
    [SerializeField]
    private Slider kemptSlider;


    // Start is called before the first frame update
    // Used to load in any saved data
    void Start()
    {
        communicator.OnMessageReceived += ReceiveMessage;
        StreamReader input = null;

        try
        {
            // open the file for reading
            string path = "..\\..\\..\\" + fileName;
            input = new StreamReader(path);

            // read the stats
            
            // read the growthstate

            // read the growth time
        }
        catch (Exception e)
        {
            Debug.Log("An error occured: " + e);
        }
        finally
        {
            if(input != null)
            {
                input.Close();
            }
        }
    }

    /// <summary>
    /// For saving the file
    /// </summary>
    public void Save()
    {
        StreamWriter output = null;

        try
        {
            // Open the streamwriter
            string path = "..\\..\\..\\" + fileName;
            output = new StreamWriter(path);

            // write out the stats
            output.WriteLine("Stats:");
            output.WriteLine("{0},{1},{2},{3},{4}", 
                waterSlider.value, playSlider.value, chatSlider.value,
                foodSlider.value, kemptSlider.value);

            // write out the growthstate
            output.WriteLine("GrowthState:");
            output.WriteLine(growthState);

            // write out the remaining growth time
            output.WriteLine("Growth Time:");
            string[] splitGrowthTime = growthTime.Split(',');
            output.WriteLine(splitGrowthTime[1]);
        }
        catch (Exception e)
        {
            Debug.Log("An error occured: " + e);
        }
        finally
        {
            if (output != null)
            {
                output.Close();
            }
        }
    }

    /// <summary>
    /// Saves and quits the game
    /// </summary>
    public void SaveQuit()
    {
        // Save();
        quitter.Quit();
    }


    private void ReceiveMessage(string message)
    {
        if (message == "egg" || message == "baby" ||
            message == "child" || message == "adult") growthState = message;
        if (message.Contains("Time")) growthTime = message;
    }
}
