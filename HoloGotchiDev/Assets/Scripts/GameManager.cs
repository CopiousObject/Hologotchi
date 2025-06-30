using LookingGlass;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] public GameObject Holopal;
    [SerializeField] public int objCount = 10;
    [SerializeField] private InterProcessCommunicator receiver;
    public GameObject foodPrefab;
    public GameObject waterPrefab;
    public List<GameObject> spawnedObjects;
    private GameObject currentObject;


    // Start is called before the first frame update
    void Start()
    {
        currentObject = foodPrefab;
        Debug.Log("Subscribing to IPC message receiver");
        if (receiver != null) receiver.OnMessageReceived += ReceiveMessage;
    }

    void Update()
    {
        if (spawnedObjects.Count > objCount)
        {
            Destroy(spawnedObjects[0]);
            spawnedObjects.Remove(spawnedObjects[0]);
        }

        if (Input.GetKeyUp(KeyCode.S))
        {
            SpawnItem(foodPrefab);
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            SpawnItem(waterPrefab);
        }
    }

    public void SpawnItem(GameObject current)
    {
        spawnedObjects.Add(Instantiate(current, this.gameObject.transform.position, Quaternion.identity));
    }

    /// <summary>
    /// Processing for IPC
    /// </summary>
    /// <param name="message"></param>
    public void ReceiveMessage(string message)
    {
        Debug.Log("Received IPC message: " + message);
        if (message == "Drop Food Icon") SpawnItem(foodPrefab);
        if (message == "Drop Water Icon") SpawnItem(waterPrefab);
        if (message == "Drop Chat Icon") SpawnItem(foodPrefab);
        if (message == "Drop Ball Icon") SpawnItem(foodPrefab);
        if (message == "Drop Kempt Icon") SpawnItem(foodPrefab);
    }
}
