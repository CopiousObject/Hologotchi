using LookingGlass;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Enumeration;
using System.Security;
using UnityEngine;
using UnityEngine.UI;

public class FileManager : MonoBehaviour
{
    private const string fileName = "Hologotchi Save Data";
    [SerializeField]
    private QuitApplication quitter;
    [SerializeField]
    private InterProcessCommunicator communicator;
    private string growthStage;
    private string growthTime;

    [SerializeField]
    private Slider[] statBars;

    private float time;


    // Start is called before the first frame update
    // Used to load in any saved data
    void Start() 
    {
        Load();
    }

    /// <summary>
    /// Automatically ssave after some time
    /// </summary>
    void FixedUpdate()
    {
        time += Time.deltaTime;

        if (time >= 30.0f)
        {
            //Save();
        }
    }

    /// <summary>
    /// Load the file in at the beginning of the game
    /// </summary>
    public void Load()
    {
        communicator.OnMessageReceived += ReceiveMessage;
        StreamReader input = null;

        try
        {
            // open the file for reading
            string path = "..\\..\\..\\" + fileName;
            input = new StreamReader(path);
            string line = null;

            // read the stats
            Debug.Log(input.ReadLine());
            line = input.ReadLine();
            string[] statValues = line.Split(',');

            AssignStats(statBars, statValues);

            // read the growthstate
            Debug.Log(input.ReadLine());
            line = input.ReadLine();
            communicator.SendData(line);

            // read the growth time
            Debug.Log(input.ReadLine());
            line = input.ReadLine();
            communicator.SendData("GT," + line);
        }
        catch (Exception e)
        {
            Debug.Log("An error occured: " + e);
        }
        finally
        {
            if (input != null)
            {
                input.Close();
            }
        }
    }

    /// <summary>
    /// Save the file for later use
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
                statBars[0].value, statBars[1].value, statBars[2].value,
                statBars[3].value, statBars[4].value);

            // write out the growthstate
            output.WriteLine("GrowthState:");
            output.WriteLine(growthStage);

            // write out the remaining growth time
            output.WriteLine("Growth Time:");
            string[] splitGrowthTime = growthTime.Split(':');
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

    /// <summary>
    /// Applies the necessary output from received IPC messages.
    /// </summary>
    /// <param name="message"></param>
    private void ReceiveMessage(string message)
    {
        if (message == "0") growthStage = "egg";
        if (message == "1") growthStage = "baby";
        if (message == "2") growthStage = "child";
        if (message == "3") growthStage = "adult";
        if (message.Contains("Time")) growthTime = message;
    }

    /// <summary>
    /// Assigns read stat value to their corressponding statbar
    /// </summary>
    /// <param name="statbars"></param>
    /// <param name="statValues"></param>
    private void AssignStats(Slider[] statBars, string[] statValues)
    {
        for(int i = 0; i < statBars.Length; i++)
        {
            int value;
            int.TryParse(statValues[i], out value);
            statBars[i].value = value;
        }
    }
}
