using LookingGlass;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    // Reciever for IPC Messages
    [SerializeField] private ValholoIPC receiver;

    // Prefabs for duplicating
    [SerializeField]
    private GameObject food;
    [SerializeField]
    private GameObject water;
    [SerializeField]
    private GameObject clean;
    [SerializeField]
    private GameObject chat;
    [SerializeField]
    private GameObject play;

    //max integers for the lists
    [SerializeField]
    private int max;

    // Lists for each
    private List<GameObject> playObjects = new List<GameObject>();
    private List<GameObject> chatObjects = new List<GameObject>();
    private List<GameObject> cleanObjects = new List<GameObject>();
    private List<GameObject> waterObjects = new List<GameObject>();
    private List<GameObject> foodObjects = new List<GameObject>();

    //Properties
    public List<GameObject> FoodObjects => foodObjects;
    public List<GameObject> WaterObjects => waterObjects;
    public List<GameObject> CleanObjects => cleanObjects;
    public List<GameObject> PlayObjects => playObjects;
    public List<GameObject> ChatObjects => chatObjects;

    // Start is called before the first frame update
    void Start()
    {
        receiver.OnHandleMessage += HandleMessage;
    }

    // Update is called once per frame
    // TEMP: Used to have just basic key press functions
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.F))
        {
            Spawn(food, foodObjects);
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            Spawn(water, waterObjects);
        }
        if (Input.GetKeyUp(KeyCode.P))
        {
            Spawn(play, playObjects);
        }
        if (Input.GetKeyUp(KeyCode.C))
        {
            Spawn(clean, cleanObjects);
        }
        if (Input.GetKeyUp(KeyCode.H))
        {
            Spawn(chat, ChatObjects);
        }
    }

    /// <summary>
    /// Used to spawn in items from the tree
    /// </summary>
    /// <param name="spawn"></param>
    /// <param name="spawnList"></param>
    private void Spawn(GameObject spawn, List<GameObject> spawnList)
    {
        if (spawnList.Count == max)
        {
            var oldest_object = spawnList[0];

            oldest_object.transform.SetPositionAndRotation(gameObject.transform.position, Quaternion.identity);

            spawnList.RemoveAt(0);
            spawnList.Add(oldest_object);

            return;
        }
        spawnList.Add(Instantiate(spawn, gameObject.transform.position, Quaternion.identity));
    }

    void HandleMessage(IPCMessageId id, string message)
    {
        if (id == IPCMessageId.DropItem)
        {
            if (message == "Food Icon") Spawn(food, foodObjects);
            if (message == "Water Icon") Spawn(water, waterObjects);
            if (message == "Chat Icon") Spawn(chat, chatObjects);
            if (message == "Ball Icon") Spawn(play, playObjects);
            if (message == "Kempt Icon") Spawn(clean, cleanObjects);
        }
    }
}
