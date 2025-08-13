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
    private HoloPal holopal;
    private GameData gameData = new GameData();
    private string saveFilePath;

    private float time;

    void Awake()
    {
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

    void OnApplicationQuit()
    {
        Save();
    }

    void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
        {
            Save();
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
    /// Used to keep gameData up to date
    /// </summary>
    private void UpdateValues()
    {
        // Stat values
        gameData.waterValue = holopal.water;
        gameData.hungerValue = holopal.food;
        gameData.playValue = holopal.play;
        gameData.chatValue = holopal.chat;
        gameData.kemptValue = holopal.clean;

        // growth time and stage
        gameData.growthStage = (int)holopal.current_stage;
        gameData.growthTime = holopal.growth;
    }

    /// <summary>
    /// Used to load in and send out the starting values for the run
    /// </summary>
    private void AssignValues()
    {
        for (var i = 0; i < gameData.growthStage; i++)
        {
            holopal.GoNextStage(); // too many systems rely on the logic in here so we can't just set the stage
        }

        // Stat values
        holopal.water = gameData.waterValue;
        holopal.food = gameData.hungerValue;
        holopal.play = gameData.playValue;
        holopal.chat = gameData.chatValue;
        holopal.clean = gameData.kemptValue;

        holopal.growth = gameData.growthTime;
    }
}

/// <summary>
/// The data that we want save
/// </summary>
[System.Serializable]
public class GameData
{
    public double waterValue;
    public double hungerValue;
    public double playValue;
    public double chatValue;
    public double kemptValue;

    public int growthStage;
    public double growthTime;
}
