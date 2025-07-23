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
    [SerializeField]
    private QuitApplication quitter;
    [SerializeField]
    private InterProcessCommunicator communicator;

    [SerializeField]
    private Slider[] statBars;
    private GameData gameData = new GameData();
    private string saveFilePath;
    private int tempGrowthStage;
    private float tempTime;

    private float time;

    void Awake()
    {
        communicator.OnMessageReceived += ReceiveMessage;
        saveFilePath = Application.persistentDataPath + "/hologotchisavegame.json";
    }

    // Start is called before the first frame update
    // Used to load in any saved data
    void Start() 
    {
        if (File.Exists(saveFilePath))
        {
            Load();
        }
    }

    /// <summary>
    /// Automatically save after some time
    /// </summary>
    void Update()
    {
        time += Time.deltaTime;

        //Debug.Log("THIS IS TIME FOR SAVING: " + time);
        if (time >= 30.0f)
        {
            Debug.Log("Saving");
            Save();
            time -= 30.0f;
        }
    }

    /// <summary>
    /// Load the file in at the beginning of the game
    /// </summary>
    public void Load()
    {
        // Check if the save file exists
        try
        {
            // Read the JSON data from the file
            string jsonData = File.ReadAllText(saveFilePath);

            // Deserialize the JSON data back into gameData
            gameData = JsonUtility.FromJson<GameData>(jsonData);

            // Update the game state with loaded data
            AssignValues();
        }
        catch (Exception error)
        {
            Debug.Log("Cannot Load: " + error.Message);
        }
    }

    /// <summary>
    /// Save the file for later use
    /// </summary>
    public void Save()
    {
        try
        {
            // Update the values of gameData
            UpdateValues();

            // Serialize the gameData to JSON
            string jsonData = JsonUtility.ToJson(gameData);

            // Write the JSON data to a file
            File.WriteAllText(saveFilePath, jsonData);

            Debug.Log("Game Saved!");
        }
        catch (Exception error)
        {
            Debug.Log("Cannot Save: " + error.Message);
        }
    }

    /// <summary>
    /// Saves and quits the game
    /// </summary>
    public void SaveQuit()
    {
        Save();
        quitter.Quit();
    }

    /// <summary>
    /// Used to keep gameData up to date
    /// </summary>
    private void UpdateValues()
    {
        // Stat values
        gameData.waterValue = statBars[0].value;
        gameData.hungerValue = statBars[1].value;
        gameData.playValue = statBars[2].value;
        gameData.chatValue = statBars[3].value;
        gameData.kemptValue = statBars[4].value;

        // growth time and stage
        gameData.growthStage = tempGrowthStage;
        gameData.growthTime = tempTime;
    }

    /// <summary>
    /// Used to load in and send out the starting values for the run
    /// </summary>
    private void AssignValues()
    {
        // Stat values
        statBars[0].value = gameData.waterValue;
        statBars[1].value = gameData.hungerValue;
        statBars[2].value = gameData.playValue;
        statBars[3].value = gameData.chatValue;
        statBars[4].value = gameData.kemptValue;

        // Send a message through ipc to Holopal
        communicator.SendData("{0}", gameData.growthStage);
        communicator.SendData("Time:" + gameData.growthTime);
        tempGrowthStage = gameData.growthStage;
        tempTime = gameData.growthTime;
    }

    /// <summary>
    /// For receiving messages through ipc
    /// </summary>
    /// <param name="message"></param>
    public void ReceiveMessage(string message)
    {
        //Debug.Log("Received IPC Message: " + message);
        if (message == "0" || message == "1" ||
            message == "2" || message == "3") 
            int.TryParse(message, out tempGrowthStage);
        if (message.Contains("Time"))
        {
            string[] splitMessage = message.Split(':');
            float.TryParse(splitMessage[1], out tempTime);
        }
    }
}

/// <summary>
/// The data that we want save
/// </summary>
[System.Serializable]
public class GameData
{
    public float waterValue;
    public float hungerValue;
    public float playValue;
    public float chatValue;
    public float kemptValue;

    public int growthStage;
    public float growthTime;
}
